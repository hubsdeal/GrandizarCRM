import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreZipCodeMapsServiceProxy, StoreZipCodeMapDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreZipCodeMapModalComponent } from './create-or-edit-storeZipCodeMap-modal.component';

import { ViewStoreZipCodeMapModalComponent } from './view-storeZipCodeMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeZipCodeMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreZipCodeMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreZipCodeMapModal', { static: true })
    createOrEditStoreZipCodeMapModal: CreateOrEditStoreZipCodeMapModalComponent;
    @ViewChild('viewStoreZipCodeMapModal', { static: true })
    viewStoreZipCodeMapModal: ViewStoreZipCodeMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    zipCodeFilter = '';
    storeNameFilter = '';
    zipCodeNameFilter = '';

    constructor(
        injector: Injector,
        private _storeZipCodeMapsServiceProxy: StoreZipCodeMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreZipCodeMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeZipCodeMapsServiceProxy
            .getAll(
                this.filterText,
                this.zipCodeFilter,
                this.storeNameFilter,
                this.zipCodeNameFilter,
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

    createStoreZipCodeMap(): void {
        this.createOrEditStoreZipCodeMapModal.show();
    }

    deleteStoreZipCodeMap(storeZipCodeMap: StoreZipCodeMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeZipCodeMapsServiceProxy.delete(storeZipCodeMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeZipCodeMapsServiceProxy
            .getStoreZipCodeMapsToExcel(
                this.filterText,
                this.zipCodeFilter,
                this.storeNameFilter,
                this.zipCodeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.zipCodeFilter = '';
        this.storeNameFilter = '';
        this.zipCodeNameFilter = '';

        this.getStoreZipCodeMaps();
    }
}
