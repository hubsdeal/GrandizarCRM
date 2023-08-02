import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HubStoresServiceProxy, HubStoreDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditHubStoreModalComponent } from './create-or-edit-hubStore-modal.component';

import { ViewHubStoreModalComponent } from './view-hubStore-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'app-hubStores',
    templateUrl: './hubStores.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class HubStoresComponent extends AppComponentBase {
    @ViewChild('createOrEditHubStoreModal', { static: true })
    createOrEditHubStoreModal: CreateOrEditHubStoreModalComponent;
    @ViewChild('viewHubStoreModal', { static: true }) viewHubStoreModal: ViewHubStoreModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    publishedFilter = -1;
    maxDisplaySequenceFilter: number;
    maxDisplaySequenceFilterEmpty: number;
    minDisplaySequenceFilter: number;
    minDisplaySequenceFilterEmpty: number;
    hubNameFilter = '';
    storeNameFilter = '';

    selectedAll: boolean = false;
    selectedInput: number[] = [];
    @Input() hubId:number;

    constructor(
        injector: Injector,
        private _hubStoresServiceProxy: HubStoresServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getHubStores(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._hubStoresServiceProxy
            .getAllByHubId(
                this.hubId,
                this.filterText,
                this.publishedFilter,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.hubNameFilter,
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

    onChangesSelectAll() {
        for (var i = 0; i < this.primengTableHelper.records.length; i++) {
            this.primengTableHelper.records[i].selected = this.selectedAll;
        }
    }

    checkIfAllSelected() {
        this.selectedAll = this.primengTableHelper.records.every(function (item: any) {
            return item.selected == true;
        })
    }

    refreshCheckboxReloadList() {
        this.selectedAll = false;
        for (var i = 0; i < this.primengTableHelper.records.length; i++) {
            this.primengTableHelper.records[i].selected = false;
        }
        this.reloadPage();
    }
    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createHubStore(): void {
        this.createOrEditHubStoreModal.hubId = this.hubId;
        this.createOrEditHubStoreModal.show();
    }

    deleteHubStore(hubStore: HubStoreDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._hubStoresServiceProxy.delete(hubStore.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._hubStoresServiceProxy
            .getHubStoresToExcel(
                this.filterText,
                this.publishedFilter,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.hubNameFilter,
                this.storeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.publishedFilter = -1;
        this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.hubNameFilter = '';
        this.storeNameFilter = '';

        this.getHubStores();
    }
}
