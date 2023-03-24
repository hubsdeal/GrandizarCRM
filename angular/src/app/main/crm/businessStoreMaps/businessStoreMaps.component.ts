import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BusinessStoreMapsServiceProxy, BusinessStoreMapDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditBusinessStoreMapModalComponent } from './create-or-edit-businessStoreMap-modal.component';

import { ViewBusinessStoreMapModalComponent } from './view-businessStoreMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './businessStoreMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class BusinessStoreMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditBusinessStoreMapModal', { static: true })
    createOrEditBusinessStoreMapModal: CreateOrEditBusinessStoreMapModalComponent;
    @ViewChild('viewBusinessStoreMapModal', { static: true })
    viewBusinessStoreMapModal: ViewBusinessStoreMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    businessNameFilter = '';
    storeNameFilter = '';

    constructor(
        injector: Injector,
        private _businessStoreMapsServiceProxy: BusinessStoreMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getBusinessStoreMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._businessStoreMapsServiceProxy
            .getAll(
                this.filterText,
                this.businessNameFilter,
                this.storeNameFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createBusinessStoreMap(): void {
        this.createOrEditBusinessStoreMapModal.show();
    }

    deleteBusinessStoreMap(businessStoreMap: BusinessStoreMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._businessStoreMapsServiceProxy.delete(businessStoreMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._businessStoreMapsServiceProxy
            .getBusinessStoreMapsToExcel(this.filterText, this.businessNameFilter, this.storeNameFilter)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.businessNameFilter = '';
        this.storeNameFilter = '';

        this.getBusinessStoreMaps();
    }
}
