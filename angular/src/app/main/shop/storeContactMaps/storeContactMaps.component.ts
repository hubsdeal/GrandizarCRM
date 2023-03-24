import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreContactMapsServiceProxy, StoreContactMapDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreContactMapModalComponent } from './create-or-edit-storeContactMap-modal.component';

import { ViewStoreContactMapModalComponent } from './view-storeContactMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeContactMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreContactMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreContactMapModal', { static: true })
    createOrEditStoreContactMapModal: CreateOrEditStoreContactMapModalComponent;
    @ViewChild('viewStoreContactMapModal', { static: true })
    viewStoreContactMapModal: ViewStoreContactMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    paidCustomerFilter = -1;
    maxLifeTimeSalesAmountFilter: number;
    maxLifeTimeSalesAmountFilterEmpty: number;
    minLifeTimeSalesAmountFilter: number;
    minLifeTimeSalesAmountFilterEmpty: number;
    storeNameFilter = '';
    contactFullNameFilter = '';

    constructor(
        injector: Injector,
        private _storeContactMapsServiceProxy: StoreContactMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreContactMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeContactMapsServiceProxy
            .getAll(
                this.filterText,
                this.paidCustomerFilter,
                this.maxLifeTimeSalesAmountFilter == null
                    ? this.maxLifeTimeSalesAmountFilterEmpty
                    : this.maxLifeTimeSalesAmountFilter,
                this.minLifeTimeSalesAmountFilter == null
                    ? this.minLifeTimeSalesAmountFilterEmpty
                    : this.minLifeTimeSalesAmountFilter,
                this.storeNameFilter,
                this.contactFullNameFilter,
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

    createStoreContactMap(): void {
        this.createOrEditStoreContactMapModal.show();
    }

    deleteStoreContactMap(storeContactMap: StoreContactMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeContactMapsServiceProxy.delete(storeContactMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeContactMapsServiceProxy
            .getStoreContactMapsToExcel(
                this.filterText,
                this.paidCustomerFilter,
                this.maxLifeTimeSalesAmountFilter == null
                    ? this.maxLifeTimeSalesAmountFilterEmpty
                    : this.maxLifeTimeSalesAmountFilter,
                this.minLifeTimeSalesAmountFilter == null
                    ? this.minLifeTimeSalesAmountFilterEmpty
                    : this.minLifeTimeSalesAmountFilter,
                this.storeNameFilter,
                this.contactFullNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.paidCustomerFilter = -1;
        this.maxLifeTimeSalesAmountFilter = this.maxLifeTimeSalesAmountFilterEmpty;
        this.minLifeTimeSalesAmountFilter = this.maxLifeTimeSalesAmountFilterEmpty;
        this.storeNameFilter = '';
        this.contactFullNameFilter = '';

        this.getStoreContactMaps();
    }
}
