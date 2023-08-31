import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    StoreMarketplaceCommissionSettingsServiceProxy,
    StoreMarketplaceCommissionSettingDto,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreMarketplaceCommissionSettingModalComponent } from './create-or-edit-storeMarketplaceCommissionSetting-modal.component';

import { ViewStoreMarketplaceCommissionSettingModalComponent } from './view-storeMarketplaceCommissionSetting-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector:'appStoreMarketplaceSettings',
    templateUrl: './storeMarketplaceCommissionSettings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreMarketplaceCommissionSettingsComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreMarketplaceCommissionSettingModal', { static: true })
    createOrEditStoreMarketplaceCommissionSettingModal: CreateOrEditStoreMarketplaceCommissionSettingModalComponent;
    @ViewChild('viewStoreMarketplaceCommissionSettingModal', { static: true })
    viewStoreMarketplaceCommissionSettingModal: ViewStoreMarketplaceCommissionSettingModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxPercentageFilter: number;
    maxPercentageFilterEmpty: number;
    minPercentageFilter: number;
    minPercentageFilterEmpty: number;
    maxFixedAmountFilter: number;
    maxFixedAmountFilterEmpty: number;
    minFixedAmountFilter: number;
    minFixedAmountFilterEmpty: number;
    maxStartDateFilter: DateTime;
    minStartDateFilter: DateTime;
    maxEndDateFilter: DateTime;
    minEndDateFilter: DateTime;
    storeNameFilter = '';
    marketplaceCommissionTypeNameFilter = '';
    productCategoryNameFilter = '';
    productNameFilter = '';

    constructor(
        injector: Injector,
        private _storeMarketplaceCommissionSettingsServiceProxy: StoreMarketplaceCommissionSettingsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreMarketplaceCommissionSettings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeMarketplaceCommissionSettingsServiceProxy
            .getAll(
                this.filterText,
                this.maxPercentageFilter == null ? this.maxPercentageFilterEmpty : this.maxPercentageFilter,
                this.minPercentageFilter == null ? this.minPercentageFilterEmpty : this.minPercentageFilter,
                this.maxFixedAmountFilter == null ? this.maxFixedAmountFilterEmpty : this.maxFixedAmountFilter,
                this.minFixedAmountFilter == null ? this.minFixedAmountFilterEmpty : this.minFixedAmountFilter,
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
                this.storeNameFilter,
                this.marketplaceCommissionTypeNameFilter,
                this.productCategoryNameFilter,
                this.productNameFilter,
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

    createStoreMarketplaceCommissionSetting(): void {
        this.createOrEditStoreMarketplaceCommissionSettingModal.show();
    }

    deleteStoreMarketplaceCommissionSetting(
        storeMarketplaceCommissionSetting: StoreMarketplaceCommissionSettingDto
    ): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeMarketplaceCommissionSettingsServiceProxy
                    .delete(storeMarketplaceCommissionSetting.id)
                    .subscribe(() => {
                        this.reloadPage();
                        this.notify.success(this.l('SuccessfullyDeleted'));
                    });
            }
        });
    }

    exportToExcel(): void {
        this._storeMarketplaceCommissionSettingsServiceProxy
            .getStoreMarketplaceCommissionSettingsToExcel(
                this.filterText,
                this.maxPercentageFilter == null ? this.maxPercentageFilterEmpty : this.maxPercentageFilter,
                this.minPercentageFilter == null ? this.minPercentageFilterEmpty : this.minPercentageFilter,
                this.maxFixedAmountFilter == null ? this.maxFixedAmountFilterEmpty : this.maxFixedAmountFilter,
                this.minFixedAmountFilter == null ? this.minFixedAmountFilterEmpty : this.minFixedAmountFilter,
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
                this.storeNameFilter,
                this.marketplaceCommissionTypeNameFilter,
                this.productCategoryNameFilter,
                this.productNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxPercentageFilter = this.maxPercentageFilterEmpty;
        this.minPercentageFilter = this.maxPercentageFilterEmpty;
        this.maxFixedAmountFilter = this.maxFixedAmountFilterEmpty;
        this.minFixedAmountFilter = this.maxFixedAmountFilterEmpty;
        this.maxStartDateFilter = undefined;
        this.minStartDateFilter = undefined;
        this.maxEndDateFilter = undefined;
        this.minEndDateFilter = undefined;
        this.storeNameFilter = '';
        this.marketplaceCommissionTypeNameFilter = '';
        this.productCategoryNameFilter = '';
        this.productNameFilter = '';

        this.getStoreMarketplaceCommissionSettings();
    }
}
