import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreWidgetMapsServiceProxy, StoreWidgetMapDto, WidgetType } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreWidgetMapModalComponent } from './create-or-edit-storeWidgetMap-modal.component';

import { ViewStoreWidgetMapModalComponent } from './view-storeWidgetMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeWidgetMaps.component.html',
    styleUrls: ['./storeWidgetMaps.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreWidgetMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreWidgetMapModal', { static: true })
    createOrEditStoreWidgetMapModal: CreateOrEditStoreWidgetMapModalComponent;
    @ViewChild('viewStoreWidgetMapModal', { static: true }) viewStoreWidgetMapModal: ViewStoreWidgetMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDisplaySequenceFilter: number;
    maxDisplaySequenceFilterEmpty: number;
    minDisplaySequenceFilter: number;
    minDisplaySequenceFilterEmpty: number;
    widgetTypeIdFilter = -1;
    customNameFilter = '';
    masterWidgetNameFilter = '';
    storeNameFilter = '';

    widgetType = WidgetType;

    constructor(
        injector: Injector,
        private _storeWidgetMapsServiceProxy: StoreWidgetMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreWidgetMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeWidgetMapsServiceProxy
            .getAll(
                this.filterText,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.widgetTypeIdFilter,
                this.customNameFilter,
                this.masterWidgetNameFilter,
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

    createStoreWidgetMap(): void {
        this.createOrEditStoreWidgetMapModal.show();
    }

    deleteStoreWidgetMap(storeWidgetMap: StoreWidgetMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeWidgetMapsServiceProxy.delete(storeWidgetMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeWidgetMapsServiceProxy
            .getStoreWidgetMapsToExcel(
                this.filterText,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.widgetTypeIdFilter,
                this.customNameFilter,
                this.masterWidgetNameFilter,
                this.storeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.widgetTypeIdFilter = -1;
        this.customNameFilter = '';
        this.masterWidgetNameFilter = '';
        this.storeNameFilter = '';

        this.getStoreWidgetMaps();
    }
}
