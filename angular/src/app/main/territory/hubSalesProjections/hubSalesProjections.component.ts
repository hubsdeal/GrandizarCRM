import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HubSalesProjectionsServiceProxy, HubSalesProjectionDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditHubSalesProjectionModalComponent } from './create-or-edit-hubSalesProjection-modal.component';

import { ViewHubSalesProjectionModalComponent } from './view-hubSalesProjection-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'app-hubSalesProjections',
    templateUrl: './hubSalesProjections.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class HubSalesProjectionsComponent extends AppComponentBase {
    @ViewChild('createOrEditHubSalesProjectionModal', { static: true })
    createOrEditHubSalesProjectionModal: CreateOrEditHubSalesProjectionModalComponent;
    @ViewChild('viewHubSalesProjectionModal', { static: true })
    viewHubSalesProjectionModal: ViewHubSalesProjectionModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDurationTypeIdFilter: number;
    maxDurationTypeIdFilterEmpty: number;
    minDurationTypeIdFilter: number;
    minDurationTypeIdFilterEmpty: number;
    maxStartDateFilter: DateTime;
    minStartDateFilter: DateTime;
    maxEndDateFilter: DateTime;
    minEndDateFilter: DateTime;
    maxExpectedSalesAmountFilter: number;
    maxExpectedSalesAmountFilterEmpty: number;
    minExpectedSalesAmountFilter: number;
    minExpectedSalesAmountFilterEmpty: number;
    maxActualSalesAmountFilter: number;
    maxActualSalesAmountFilterEmpty: number;
    minActualSalesAmountFilter: number;
    minActualSalesAmountFilterEmpty: number;
    hubNameFilter = '';
    productCategoryNameFilter = '';
    storeNameFilter = '';
    currencyNameFilter = '';

    @Input() hubId:number;
    constructor(
        injector: Injector,
        private _hubSalesProjectionsServiceProxy: HubSalesProjectionsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getHubSalesProjections(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._hubSalesProjectionsServiceProxy
            .getAllByHubId(
                this.hubId,
                this.filterText,
                this.maxDurationTypeIdFilter == null ? this.maxDurationTypeIdFilterEmpty : this.maxDurationTypeIdFilter,
                this.minDurationTypeIdFilter == null ? this.minDurationTypeIdFilterEmpty : this.minDurationTypeIdFilter,
                this.maxStartDateFilter === undefined
                    ? this.maxStartDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
                this.minStartDateFilter === undefined
                    ? this.minStartDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
                this.maxEndDateFilter === undefined
                    ? this.maxEndDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
                this.minEndDateFilter === undefined
                    ? this.minEndDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
                this.maxExpectedSalesAmountFilter == null
                    ? this.maxExpectedSalesAmountFilterEmpty
                    : this.maxExpectedSalesAmountFilter,
                this.minExpectedSalesAmountFilter == null
                    ? this.minExpectedSalesAmountFilterEmpty
                    : this.minExpectedSalesAmountFilter,
                this.maxActualSalesAmountFilter == null
                    ? this.maxActualSalesAmountFilterEmpty
                    : this.maxActualSalesAmountFilter,
                this.minActualSalesAmountFilter == null
                    ? this.minActualSalesAmountFilterEmpty
                    : this.minActualSalesAmountFilter,
                this.hubNameFilter,
                this.productCategoryNameFilter,
                this.storeNameFilter,
                this.currencyNameFilter,
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

    createHubSalesProjection(): void {
        this.createOrEditHubSalesProjectionModal.hubId = this.hubId;
        this.createOrEditHubSalesProjectionModal.show();
    }

    deleteHubSalesProjection(hubSalesProjection: HubSalesProjectionDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._hubSalesProjectionsServiceProxy.delete(hubSalesProjection.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._hubSalesProjectionsServiceProxy
            .getHubSalesProjectionsToExcel(
                this.filterText,
                this.maxDurationTypeIdFilter == null ? this.maxDurationTypeIdFilterEmpty : this.maxDurationTypeIdFilter,
                this.minDurationTypeIdFilter == null ? this.minDurationTypeIdFilterEmpty : this.minDurationTypeIdFilter,
                this.maxStartDateFilter === undefined
                    ? this.maxStartDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
                this.minStartDateFilter === undefined
                    ? this.minStartDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
                this.maxEndDateFilter === undefined
                    ? this.maxEndDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
                this.minEndDateFilter === undefined
                    ? this.minEndDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
                this.maxExpectedSalesAmountFilter == null
                    ? this.maxExpectedSalesAmountFilterEmpty
                    : this.maxExpectedSalesAmountFilter,
                this.minExpectedSalesAmountFilter == null
                    ? this.minExpectedSalesAmountFilterEmpty
                    : this.minExpectedSalesAmountFilter,
                this.maxActualSalesAmountFilter == null
                    ? this.maxActualSalesAmountFilterEmpty
                    : this.maxActualSalesAmountFilter,
                this.minActualSalesAmountFilter == null
                    ? this.minActualSalesAmountFilterEmpty
                    : this.minActualSalesAmountFilter,
                this.hubNameFilter,
                this.productCategoryNameFilter,
                this.storeNameFilter,
                this.currencyNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxDurationTypeIdFilter = this.maxDurationTypeIdFilterEmpty;
        this.minDurationTypeIdFilter = this.maxDurationTypeIdFilterEmpty;
        this.maxStartDateFilter = undefined;
        this.minStartDateFilter = undefined;
        this.maxEndDateFilter = undefined;
        this.minEndDateFilter = undefined;
        this.maxExpectedSalesAmountFilter = this.maxExpectedSalesAmountFilterEmpty;
        this.minExpectedSalesAmountFilter = this.maxExpectedSalesAmountFilterEmpty;
        this.maxActualSalesAmountFilter = this.maxActualSalesAmountFilterEmpty;
        this.minActualSalesAmountFilter = this.maxActualSalesAmountFilterEmpty;
        this.hubNameFilter = '';
        this.productCategoryNameFilter = '';
        this.storeNameFilter = '';
        this.currencyNameFilter = '';

        this.getHubSalesProjections();
    }
}
