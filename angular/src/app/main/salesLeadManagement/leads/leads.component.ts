import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LeadsServiceProxy, LeadDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditLeadModalComponent } from './create-or-edit-lead-modal.component';

import { ViewLeadModalComponent } from './view-lead-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './leads.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class LeadsComponent extends AppComponentBase {
    @ViewChild('createOrEditLeadModal', { static: true }) createOrEditLeadModal: CreateOrEditLeadModalComponent;
    @ViewChild('viewLeadModal', { static: true }) viewLeadModal: ViewLeadModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    titleFilter = '';
    firstNameFilter = '';
    lastNameFilter = '';
    emailFilter = '';
    phoneFilter = '';
    companyFilter = '';
    jobTitleFilter = '';
    industryFilter = '';
    maxLeadScoreFilter: number;
    maxLeadScoreFilterEmpty: number;
    minLeadScoreFilter: number;
    minLeadScoreFilterEmpty: number;
    maxExpectedSalesAmountFilter: number;
    maxExpectedSalesAmountFilterEmpty: number;
    minExpectedSalesAmountFilter: number;
    minExpectedSalesAmountFilterEmpty: number;
    maxCreatedDateFilter: DateTime;
    minCreatedDateFilter: DateTime;
    maxExpectedClosingDateFilter: DateTime;
    minExpectedClosingDateFilter: DateTime;
    contactFullNameFilter = '';
    businessNameFilter = '';
    productNameFilter = '';
    productCategoryNameFilter = '';
    storeNameFilter = '';
    employeeNameFilter = '';
    leadSourceNameFilter = '';
    leadPipelineStageNameFilter = '';
    hubNameFilter = '';

    constructor(
        injector: Injector,
        private _leadsServiceProxy: LeadsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getLeads(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._leadsServiceProxy
            .getAll(
                this.filterText,
                this.titleFilter,
                this.firstNameFilter,
                this.lastNameFilter,
                this.emailFilter,
                this.phoneFilter,
                this.companyFilter,
                this.jobTitleFilter,
                this.industryFilter,
                this.maxLeadScoreFilter == null ? this.maxLeadScoreFilterEmpty : this.maxLeadScoreFilter,
                this.minLeadScoreFilter == null ? this.minLeadScoreFilterEmpty : this.minLeadScoreFilter,
                this.maxExpectedSalesAmountFilter == null
                    ? this.maxExpectedSalesAmountFilterEmpty
                    : this.maxExpectedSalesAmountFilter,
                this.minExpectedSalesAmountFilter == null
                    ? this.minExpectedSalesAmountFilterEmpty
                    : this.minExpectedSalesAmountFilter,
                this.maxCreatedDateFilter === undefined
                    ? this.maxCreatedDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxCreatedDateFilter),
                this.minCreatedDateFilter === undefined
                    ? this.minCreatedDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minCreatedDateFilter),
                this.maxExpectedClosingDateFilter === undefined
                    ? this.maxExpectedClosingDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxExpectedClosingDateFilter),
                this.minExpectedClosingDateFilter === undefined
                    ? this.minExpectedClosingDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minExpectedClosingDateFilter),
                this.contactFullNameFilter,
                this.businessNameFilter,
                this.productNameFilter,
                this.productCategoryNameFilter,
                this.storeNameFilter,
                this.employeeNameFilter,
                this.leadSourceNameFilter,
                this.leadPipelineStageNameFilter,
                this.hubNameFilter,
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

    createLead(): void {
        this.createOrEditLeadModal.show();
    }

    deleteLead(lead: LeadDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._leadsServiceProxy.delete(lead.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._leadsServiceProxy
            .getLeadsToExcel(
                this.filterText,
                this.titleFilter,
                this.firstNameFilter,
                this.lastNameFilter,
                this.emailFilter,
                this.phoneFilter,
                this.companyFilter,
                this.jobTitleFilter,
                this.industryFilter,
                this.maxLeadScoreFilter == null ? this.maxLeadScoreFilterEmpty : this.maxLeadScoreFilter,
                this.minLeadScoreFilter == null ? this.minLeadScoreFilterEmpty : this.minLeadScoreFilter,
                this.maxExpectedSalesAmountFilter == null
                    ? this.maxExpectedSalesAmountFilterEmpty
                    : this.maxExpectedSalesAmountFilter,
                this.minExpectedSalesAmountFilter == null
                    ? this.minExpectedSalesAmountFilterEmpty
                    : this.minExpectedSalesAmountFilter,
                this.maxCreatedDateFilter === undefined
                    ? this.maxCreatedDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxCreatedDateFilter),
                this.minCreatedDateFilter === undefined
                    ? this.minCreatedDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minCreatedDateFilter),
                this.maxExpectedClosingDateFilter === undefined
                    ? this.maxExpectedClosingDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxExpectedClosingDateFilter),
                this.minExpectedClosingDateFilter === undefined
                    ? this.minExpectedClosingDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minExpectedClosingDateFilter),
                this.contactFullNameFilter,
                this.businessNameFilter,
                this.productNameFilter,
                this.productCategoryNameFilter,
                this.storeNameFilter,
                this.employeeNameFilter,
                this.leadSourceNameFilter,
                this.leadPipelineStageNameFilter,
                this.hubNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.titleFilter = '';
        this.firstNameFilter = '';
        this.lastNameFilter = '';
        this.emailFilter = '';
        this.phoneFilter = '';
        this.companyFilter = '';
        this.jobTitleFilter = '';
        this.industryFilter = '';
        this.maxLeadScoreFilter = this.maxLeadScoreFilterEmpty;
        this.minLeadScoreFilter = this.maxLeadScoreFilterEmpty;
        this.maxExpectedSalesAmountFilter = this.maxExpectedSalesAmountFilterEmpty;
        this.minExpectedSalesAmountFilter = this.maxExpectedSalesAmountFilterEmpty;
        this.maxCreatedDateFilter = undefined;
        this.minCreatedDateFilter = undefined;
        this.maxExpectedClosingDateFilter = undefined;
        this.minExpectedClosingDateFilter = undefined;
        this.contactFullNameFilter = '';
        this.businessNameFilter = '';
        this.productNameFilter = '';
        this.productCategoryNameFilter = '';
        this.storeNameFilter = '';
        this.employeeNameFilter = '';
        this.leadSourceNameFilter = '';
        this.leadPipelineStageNameFilter = '';
        this.hubNameFilter = '';

        this.getLeads();
    }
}
