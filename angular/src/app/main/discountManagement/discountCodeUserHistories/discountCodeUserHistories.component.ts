﻿import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    DiscountCodeUserHistoriesServiceProxy,
    DiscountCodeUserHistoryDto,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditDiscountCodeUserHistoryModalComponent } from './create-or-edit-discountCodeUserHistory-modal.component';

import { ViewDiscountCodeUserHistoryModalComponent } from './view-discountCodeUserHistory-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './discountCodeUserHistories.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class DiscountCodeUserHistoriesComponent extends AppComponentBase {
    @ViewChild('createOrEditDiscountCodeUserHistoryModal', { static: true })
    createOrEditDiscountCodeUserHistoryModal: CreateOrEditDiscountCodeUserHistoryModalComponent;
    @ViewChild('viewDiscountCodeUserHistoryModal', { static: true })
    viewDiscountCodeUserHistoryModal: ViewDiscountCodeUserHistoryModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxAmountFilter: DateTime;
    minAmountFilter: DateTime;
    maxDateFilter: DateTime;
    minDateFilter: DateTime;
    discountCodeGeneratorNameFilter = '';
    orderInvoiceNumberFilter = '';
    contactFullNameFilter = '';

    constructor(
        injector: Injector,
        private _discountCodeUserHistoriesServiceProxy: DiscountCodeUserHistoriesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getDiscountCodeUserHistories(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._discountCodeUserHistoriesServiceProxy
            .getAll(
                this.filterText,
                this.maxAmountFilter === undefined
                    ? this.maxAmountFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxAmountFilter),
                this.minAmountFilter === undefined
                    ? this.minAmountFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minAmountFilter),
                this.maxDateFilter === undefined
                    ? this.maxDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxDateFilter),
                this.minDateFilter === undefined
                    ? this.minDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minDateFilter),
                this.discountCodeGeneratorNameFilter,
                this.orderInvoiceNumberFilter,
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

    createDiscountCodeUserHistory(): void {
        this.createOrEditDiscountCodeUserHistoryModal.show();
    }

    deleteDiscountCodeUserHistory(discountCodeUserHistory: DiscountCodeUserHistoryDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._discountCodeUserHistoriesServiceProxy.delete(discountCodeUserHistory.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._discountCodeUserHistoriesServiceProxy
            .getDiscountCodeUserHistoriesToExcel(
                this.filterText,
                this.maxAmountFilter === undefined
                    ? this.maxAmountFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxAmountFilter),
                this.minAmountFilter === undefined
                    ? this.minAmountFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minAmountFilter),
                this.maxDateFilter === undefined
                    ? this.maxDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxDateFilter),
                this.minDateFilter === undefined
                    ? this.minDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minDateFilter),
                this.discountCodeGeneratorNameFilter,
                this.orderInvoiceNumberFilter,
                this.contactFullNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxAmountFilter = undefined;
        this.minAmountFilter = undefined;
        this.maxDateFilter = undefined;
        this.minDateFilter = undefined;
        this.discountCodeGeneratorNameFilter = '';
        this.orderInvoiceNumberFilter = '';
        this.contactFullNameFilter = '';

        this.getDiscountCodeUserHistories();
    }
}
