import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DiscountCodeGeneratorsServiceProxy, DiscountCodeGeneratorDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditDiscountCodeGeneratorModalComponent } from './create-or-edit-discountCodeGenerator-modal.component';

import { ViewDiscountCodeGeneratorModalComponent } from './view-discountCodeGenerator-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './discountCodeGenerators.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class DiscountCodeGeneratorsComponent extends AppComponentBase {
    @ViewChild('createOrEditDiscountCodeGeneratorModal', { static: true })
    createOrEditDiscountCodeGeneratorModal: CreateOrEditDiscountCodeGeneratorModalComponent;
    @ViewChild('viewDiscountCodeGeneratorModal', { static: true })
    viewDiscountCodeGeneratorModal: ViewDiscountCodeGeneratorModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    couponCodeFilter = '';
    percentageOrFixedAmountFilter = -1;
    maxDiscountPercentageFilter: number;
    maxDiscountPercentageFilterEmpty: number;
    minDiscountPercentageFilter: number;
    minDiscountPercentageFilterEmpty: number;
    maxDiscountAmountFilter: number;
    maxDiscountAmountFilterEmpty: number;
    minDiscountAmountFilter: number;
    minDiscountAmountFilterEmpty: number;
    maxStartDateFilter: DateTime;
    minStartDateFilter: DateTime;
    maxEndDateFilter: DateTime;
    minEndDateFilter: DateTime;
    adminNotesFilter = '';
    isActiveFilter = -1;
    startTimeFilter = '';
    endTimeFilter = '';

    constructor(
        injector: Injector,
        private _discountCodeGeneratorsServiceProxy: DiscountCodeGeneratorsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getDiscountCodeGenerators(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._discountCodeGeneratorsServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.couponCodeFilter,
                this.percentageOrFixedAmountFilter,
                this.maxDiscountPercentageFilter == null
                    ? this.maxDiscountPercentageFilterEmpty
                    : this.maxDiscountPercentageFilter,
                this.minDiscountPercentageFilter == null
                    ? this.minDiscountPercentageFilterEmpty
                    : this.minDiscountPercentageFilter,
                this.maxDiscountAmountFilter == null ? this.maxDiscountAmountFilterEmpty : this.maxDiscountAmountFilter,
                this.minDiscountAmountFilter == null ? this.minDiscountAmountFilterEmpty : this.minDiscountAmountFilter,
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
                this.adminNotesFilter,
                this.isActiveFilter,
                this.startTimeFilter,
                this.endTimeFilter,
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

    createDiscountCodeGenerator(): void {
        this.createOrEditDiscountCodeGeneratorModal.show();
    }

    deleteDiscountCodeGenerator(discountCodeGenerator: DiscountCodeGeneratorDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._discountCodeGeneratorsServiceProxy.delete(discountCodeGenerator.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._discountCodeGeneratorsServiceProxy
            .getDiscountCodeGeneratorsToExcel(
                this.filterText,
                this.nameFilter,
                this.couponCodeFilter,
                this.percentageOrFixedAmountFilter,
                this.maxDiscountPercentageFilter == null
                    ? this.maxDiscountPercentageFilterEmpty
                    : this.maxDiscountPercentageFilter,
                this.minDiscountPercentageFilter == null
                    ? this.minDiscountPercentageFilterEmpty
                    : this.minDiscountPercentageFilter,
                this.maxDiscountAmountFilter == null ? this.maxDiscountAmountFilterEmpty : this.maxDiscountAmountFilter,
                this.minDiscountAmountFilter == null ? this.minDiscountAmountFilterEmpty : this.minDiscountAmountFilter,
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
                this.adminNotesFilter,
                this.isActiveFilter,
                this.startTimeFilter,
                this.endTimeFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.couponCodeFilter = '';
        this.percentageOrFixedAmountFilter = -1;
        this.maxDiscountPercentageFilter = this.maxDiscountPercentageFilterEmpty;
        this.minDiscountPercentageFilter = this.maxDiscountPercentageFilterEmpty;
        this.maxDiscountAmountFilter = this.maxDiscountAmountFilterEmpty;
        this.minDiscountAmountFilter = this.maxDiscountAmountFilterEmpty;
        this.maxStartDateFilter = undefined;
        this.minStartDateFilter = undefined;
        this.maxEndDateFilter = undefined;
        this.minEndDateFilter = undefined;
        this.adminNotesFilter = '';
        this.isActiveFilter = -1;
        this.startTimeFilter = '';
        this.endTimeFilter = '';

        this.getDiscountCodeGenerators();
    }
}
