import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    StoreBusinessCustomerMapsServiceProxy,
    StoreBusinessCustomerMapDto,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreBusinessCustomerMapModalComponent } from './create-or-edit-storeBusinessCustomerMap-modal.component';

import { ViewStoreBusinessCustomerMapModalComponent } from './view-storeBusinessCustomerMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeBusinessCustomerMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreBusinessCustomerMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreBusinessCustomerMapModal', { static: true })
    createOrEditStoreBusinessCustomerMapModal: CreateOrEditStoreBusinessCustomerMapModalComponent;
    @ViewChild('viewStoreBusinessCustomerMapModal', { static: true })
    viewStoreBusinessCustomerMapModal: ViewStoreBusinessCustomerMapModalComponent;

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
    businessNameFilter = '';

    constructor(
        injector: Injector,
        private _storeBusinessCustomerMapsServiceProxy: StoreBusinessCustomerMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreBusinessCustomerMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeBusinessCustomerMapsServiceProxy
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
                this.businessNameFilter,
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

    createStoreBusinessCustomerMap(): void {
        this.createOrEditStoreBusinessCustomerMapModal.show();
    }

    deleteStoreBusinessCustomerMap(storeBusinessCustomerMap: StoreBusinessCustomerMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeBusinessCustomerMapsServiceProxy.delete(storeBusinessCustomerMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeBusinessCustomerMapsServiceProxy
            .getStoreBusinessCustomerMapsToExcel(
                this.filterText,
                this.paidCustomerFilter,
                this.maxLifeTimeSalesAmountFilter == null
                    ? this.maxLifeTimeSalesAmountFilterEmpty
                    : this.maxLifeTimeSalesAmountFilter,
                this.minLifeTimeSalesAmountFilter == null
                    ? this.minLifeTimeSalesAmountFilterEmpty
                    : this.minLifeTimeSalesAmountFilter,
                this.storeNameFilter,
                this.businessNameFilter
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
        this.businessNameFilter = '';

        this.getStoreBusinessCustomerMaps();
    }
}
