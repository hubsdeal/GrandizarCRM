import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreRelevantStoresServiceProxy, StoreRelevantStoreDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreRelevantStoreModalComponent } from './create-or-edit-storeRelevantStore-modal.component';

import { ViewStoreRelevantStoreModalComponent } from './view-storeRelevantStore-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeRelevantStores.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreRelevantStoresComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreRelevantStoreModal', { static: true })
    createOrEditStoreRelevantStoreModal: CreateOrEditStoreRelevantStoreModalComponent;
    @ViewChild('viewStoreRelevantStoreModal', { static: true })
    viewStoreRelevantStoreModal: ViewStoreRelevantStoreModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxRelevantStoreIdFilter: number;
    maxRelevantStoreIdFilterEmpty: number;
    minRelevantStoreIdFilter: number;
    minRelevantStoreIdFilterEmpty: number;
    storeNameFilter = '';

    constructor(
        injector: Injector,
        private _storeRelevantStoresServiceProxy: StoreRelevantStoresServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreRelevantStores(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeRelevantStoresServiceProxy
            .getAll(
                this.filterText,
                this.maxRelevantStoreIdFilter == null
                    ? this.maxRelevantStoreIdFilterEmpty
                    : this.maxRelevantStoreIdFilter,
                this.minRelevantStoreIdFilter == null
                    ? this.minRelevantStoreIdFilterEmpty
                    : this.minRelevantStoreIdFilter,
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

    createStoreRelevantStore(): void {
        this.createOrEditStoreRelevantStoreModal.show();
    }

    deleteStoreRelevantStore(storeRelevantStore: StoreRelevantStoreDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeRelevantStoresServiceProxy.delete(storeRelevantStore.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeRelevantStoresServiceProxy
            .getStoreRelevantStoresToExcel(
                this.filterText,
                this.maxRelevantStoreIdFilter == null
                    ? this.maxRelevantStoreIdFilterEmpty
                    : this.maxRelevantStoreIdFilter,
                this.minRelevantStoreIdFilter == null
                    ? this.minRelevantStoreIdFilterEmpty
                    : this.minRelevantStoreIdFilter,
                this.storeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxRelevantStoreIdFilter = this.maxRelevantStoreIdFilterEmpty;
        this.minRelevantStoreIdFilter = this.maxRelevantStoreIdFilterEmpty;
        this.storeNameFilter = '';

        this.getStoreRelevantStores();
    }
}
