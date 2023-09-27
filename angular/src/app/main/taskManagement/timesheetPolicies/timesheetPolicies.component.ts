import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { TimesheetPoliciesServiceProxy, TimesheetPolicyDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTimesheetPolicyModalComponent } from './create-or-edit-timesheetPolicy-modal.component';

import { ViewTimesheetPolicyModalComponent } from './view-timesheetPolicy-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './timesheetPolicies.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TimesheetPoliciesComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditTimesheetPolicyModal', { static: true }) createOrEditTimesheetPolicyModal: CreateOrEditTimesheetPolicyModalComponent;
    @ViewChild('viewTimesheetPolicyModal', { static: true }) viewTimesheetPolicyModal: ViewTimesheetPolicyModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    policyNameFilter = '';
    descriptionFilter = '';
    maxMaxWorkHoursPerDayFilter : number;
		maxMaxWorkHoursPerDayFilterEmpty : number;
		minMaxWorkHoursPerDayFilter : number;
		minMaxWorkHoursPerDayFilterEmpty : number;
    maxMaxWorkHoursPerWeekFilter : number;
		maxMaxWorkHoursPerWeekFilterEmpty : number;
		minMaxWorkHoursPerWeekFilter : number;
		minMaxWorkHoursPerWeekFilterEmpty : number;
    maxMinWorkHoursPerDayFilter : number;
		maxMinWorkHoursPerDayFilterEmpty : number;
		minMinWorkHoursPerDayFilter : number;
		minMinWorkHoursPerDayFilterEmpty : number;
    maxBreakTimePerDayInMinutesFilter : number;
		maxBreakTimePerDayInMinutesFilterEmpty : number;
		minBreakTimePerDayInMinutesFilter : number;
		minBreakTimePerDayInMinutesFilterEmpty : number;
    requireClockInFilter = -1;
    requireClockOutFilter = -1;
    requireManagerApprovalFilter = -1;
    maxRequiredWorkCheckinFrequencyMinutesFilter : number;
		maxRequiredWorkCheckinFrequencyMinutesFilterEmpty : number;
		minRequiredWorkCheckinFrequencyMinutesFilter : number;
		minRequiredWorkCheckinFrequencyMinutesFilterEmpty : number;
    maxEffectiveStartDateFilter : DateTime;
		minEffectiveStartDateFilter : DateTime;
    maxEffectiveEndDateFilter : DateTime;
		minEffectiveEndDateFilter : DateTime;
    isDefaultPolicyFilter = -1;






    constructor(
        injector: Injector,
        private _timesheetPoliciesServiceProxy: TimesheetPoliciesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getTimesheetPolicies(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._timesheetPoliciesServiceProxy.getAll(
            this.filterText,
            this.policyNameFilter,
            this.descriptionFilter,
            this.maxMaxWorkHoursPerDayFilter == null ? this.maxMaxWorkHoursPerDayFilterEmpty: this.maxMaxWorkHoursPerDayFilter,
            this.minMaxWorkHoursPerDayFilter == null ? this.minMaxWorkHoursPerDayFilterEmpty: this.minMaxWorkHoursPerDayFilter,
            this.maxMaxWorkHoursPerWeekFilter == null ? this.maxMaxWorkHoursPerWeekFilterEmpty: this.maxMaxWorkHoursPerWeekFilter,
            this.minMaxWorkHoursPerWeekFilter == null ? this.minMaxWorkHoursPerWeekFilterEmpty: this.minMaxWorkHoursPerWeekFilter,
            this.maxMinWorkHoursPerDayFilter == null ? this.maxMinWorkHoursPerDayFilterEmpty: this.maxMinWorkHoursPerDayFilter,
            this.minMinWorkHoursPerDayFilter == null ? this.minMinWorkHoursPerDayFilterEmpty: this.minMinWorkHoursPerDayFilter,
            this.maxBreakTimePerDayInMinutesFilter == null ? this.maxBreakTimePerDayInMinutesFilterEmpty: this.maxBreakTimePerDayInMinutesFilter,
            this.minBreakTimePerDayInMinutesFilter == null ? this.minBreakTimePerDayInMinutesFilterEmpty: this.minBreakTimePerDayInMinutesFilter,
            this.requireClockInFilter,
            this.requireClockOutFilter,
            this.requireManagerApprovalFilter,
            this.maxRequiredWorkCheckinFrequencyMinutesFilter == null ? this.maxRequiredWorkCheckinFrequencyMinutesFilterEmpty: this.maxRequiredWorkCheckinFrequencyMinutesFilter,
            this.minRequiredWorkCheckinFrequencyMinutesFilter == null ? this.minRequiredWorkCheckinFrequencyMinutesFilterEmpty: this.minRequiredWorkCheckinFrequencyMinutesFilter,
            this.maxEffectiveStartDateFilter === undefined ? this.maxEffectiveStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEffectiveStartDateFilter),
            this.minEffectiveStartDateFilter === undefined ? this.minEffectiveStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEffectiveStartDateFilter),
            this.maxEffectiveEndDateFilter === undefined ? this.maxEffectiveEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEffectiveEndDateFilter),
            this.minEffectiveEndDateFilter === undefined ? this.minEffectiveEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEffectiveEndDateFilter),
            this.isDefaultPolicyFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createTimesheetPolicy(): void {
        this.createOrEditTimesheetPolicyModal.show();        
    }


    deleteTimesheetPolicy(timesheetPolicy: TimesheetPolicyDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._timesheetPoliciesServiceProxy.delete(timesheetPolicy.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._timesheetPoliciesServiceProxy.getTimesheetPoliciesToExcel(
        this.filterText,
            this.policyNameFilter,
            this.descriptionFilter,
            this.maxMaxWorkHoursPerDayFilter == null ? this.maxMaxWorkHoursPerDayFilterEmpty: this.maxMaxWorkHoursPerDayFilter,
            this.minMaxWorkHoursPerDayFilter == null ? this.minMaxWorkHoursPerDayFilterEmpty: this.minMaxWorkHoursPerDayFilter,
            this.maxMaxWorkHoursPerWeekFilter == null ? this.maxMaxWorkHoursPerWeekFilterEmpty: this.maxMaxWorkHoursPerWeekFilter,
            this.minMaxWorkHoursPerWeekFilter == null ? this.minMaxWorkHoursPerWeekFilterEmpty: this.minMaxWorkHoursPerWeekFilter,
            this.maxMinWorkHoursPerDayFilter == null ? this.maxMinWorkHoursPerDayFilterEmpty: this.maxMinWorkHoursPerDayFilter,
            this.minMinWorkHoursPerDayFilter == null ? this.minMinWorkHoursPerDayFilterEmpty: this.minMinWorkHoursPerDayFilter,
            this.maxBreakTimePerDayInMinutesFilter == null ? this.maxBreakTimePerDayInMinutesFilterEmpty: this.maxBreakTimePerDayInMinutesFilter,
            this.minBreakTimePerDayInMinutesFilter == null ? this.minBreakTimePerDayInMinutesFilterEmpty: this.minBreakTimePerDayInMinutesFilter,
            this.requireClockInFilter,
            this.requireClockOutFilter,
            this.requireManagerApprovalFilter,
            this.maxRequiredWorkCheckinFrequencyMinutesFilter == null ? this.maxRequiredWorkCheckinFrequencyMinutesFilterEmpty: this.maxRequiredWorkCheckinFrequencyMinutesFilter,
            this.minRequiredWorkCheckinFrequencyMinutesFilter == null ? this.minRequiredWorkCheckinFrequencyMinutesFilterEmpty: this.minRequiredWorkCheckinFrequencyMinutesFilter,
            this.maxEffectiveStartDateFilter === undefined ? this.maxEffectiveStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEffectiveStartDateFilter),
            this.minEffectiveStartDateFilter === undefined ? this.minEffectiveStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEffectiveStartDateFilter),
            this.maxEffectiveEndDateFilter === undefined ? this.maxEffectiveEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEffectiveEndDateFilter),
            this.minEffectiveEndDateFilter === undefined ? this.minEffectiveEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEffectiveEndDateFilter),
            this.isDefaultPolicyFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.policyNameFilter = '';
    this.descriptionFilter = '';
    this.maxMaxWorkHoursPerDayFilter = this.maxMaxWorkHoursPerDayFilterEmpty;
		this.minMaxWorkHoursPerDayFilter = this.maxMaxWorkHoursPerDayFilterEmpty;
    this.maxMaxWorkHoursPerWeekFilter = this.maxMaxWorkHoursPerWeekFilterEmpty;
		this.minMaxWorkHoursPerWeekFilter = this.maxMaxWorkHoursPerWeekFilterEmpty;
    this.maxMinWorkHoursPerDayFilter = this.maxMinWorkHoursPerDayFilterEmpty;
		this.minMinWorkHoursPerDayFilter = this.maxMinWorkHoursPerDayFilterEmpty;
    this.maxBreakTimePerDayInMinutesFilter = this.maxBreakTimePerDayInMinutesFilterEmpty;
		this.minBreakTimePerDayInMinutesFilter = this.maxBreakTimePerDayInMinutesFilterEmpty;
    this.requireClockInFilter = -1;
    this.requireClockOutFilter = -1;
    this.requireManagerApprovalFilter = -1;
    this.maxRequiredWorkCheckinFrequencyMinutesFilter = this.maxRequiredWorkCheckinFrequencyMinutesFilterEmpty;
		this.minRequiredWorkCheckinFrequencyMinutesFilter = this.maxRequiredWorkCheckinFrequencyMinutesFilterEmpty;
    this.maxEffectiveStartDateFilter = undefined;
		this.minEffectiveStartDateFilter = undefined;
    this.maxEffectiveEndDateFilter = undefined;
		this.minEffectiveEndDateFilter = undefined;
    this.isDefaultPolicyFilter = -1;

        this.getTimesheetPolicies();
    }
}
