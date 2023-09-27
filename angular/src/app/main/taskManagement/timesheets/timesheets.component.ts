import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { TimesheetsServiceProxy, TimesheetDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTimesheetModalComponent } from './create-or-edit-timesheet-modal.component';

import { ViewTimesheetModalComponent } from './view-timesheet-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './timesheets.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TimesheetsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditTimesheetModal', { static: true }) createOrEditTimesheetModal: CreateOrEditTimesheetModalComponent;
    @ViewChild('viewTimesheetModal', { static: true }) viewTimesheetModal: ViewTimesheetModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxTimeSheetDateFilter : DateTime;
		minTimeSheetDateFilter : DateTime;
    startTimeFilter = '';
    endTimeFilter = '';
    maxTotalWorkHoursFilter : number;
		maxTotalWorkHoursFilterEmpty : number;
		minTotalWorkHoursFilter : number;
		minTotalWorkHoursFilterEmpty : number;
    maxTimeOffOrBreakTimeHoursFilter : number;
		maxTimeOffOrBreakTimeHoursFilterEmpty : number;
		minTimeOffOrBreakTimeHoursFilter : number;
		minTimeOffOrBreakTimeHoursFilterEmpty : number;
    workDetailsFilter = '';
    managerRemarksFilter = '';
    paymentPaidFilter = -1;
    clockInIpAddressFilter = '';
    clockOutIpAddressFilter = '';
    macAddressFilter = '';
    maxWorkDayStatusIdFilter : number;
		maxWorkDayStatusIdFilterEmpty : number;
		minWorkDayStatusIdFilter : number;
		minWorkDayStatusIdFilterEmpty : number;
        employeeNameFilter = '';
        storeNameFilter = '';
        businessNameFilter = '';
        jobTitleFilter = '';
        employeeName2Filter = '';






    constructor(
        injector: Injector,
        private _timesheetsServiceProxy: TimesheetsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getTimesheets(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._timesheetsServiceProxy.getAll(
            this.filterText,
            this.maxTimeSheetDateFilter === undefined ? this.maxTimeSheetDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxTimeSheetDateFilter),
            this.minTimeSheetDateFilter === undefined ? this.minTimeSheetDateFilter : this._dateTimeService.getStartOfDayForDate(this.minTimeSheetDateFilter),
            this.startTimeFilter,
            this.endTimeFilter,
            this.maxTotalWorkHoursFilter == null ? this.maxTotalWorkHoursFilterEmpty: this.maxTotalWorkHoursFilter,
            this.minTotalWorkHoursFilter == null ? this.minTotalWorkHoursFilterEmpty: this.minTotalWorkHoursFilter,
            this.maxTimeOffOrBreakTimeHoursFilter == null ? this.maxTimeOffOrBreakTimeHoursFilterEmpty: this.maxTimeOffOrBreakTimeHoursFilter,
            this.minTimeOffOrBreakTimeHoursFilter == null ? this.minTimeOffOrBreakTimeHoursFilterEmpty: this.minTimeOffOrBreakTimeHoursFilter,
            this.workDetailsFilter,
            this.managerRemarksFilter,
            this.paymentPaidFilter,
            this.clockInIpAddressFilter,
            this.clockOutIpAddressFilter,
            this.macAddressFilter,
            this.maxWorkDayStatusIdFilter == null ? this.maxWorkDayStatusIdFilterEmpty: this.maxWorkDayStatusIdFilter,
            this.minWorkDayStatusIdFilter == null ? this.minWorkDayStatusIdFilterEmpty: this.minWorkDayStatusIdFilter,
            this.employeeNameFilter,
            this.storeNameFilter,
            this.businessNameFilter,
            this.jobTitleFilter,
            this.employeeName2Filter,
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

    createTimesheet(): void {
        this.createOrEditTimesheetModal.show();        
    }


    deleteTimesheet(timesheet: TimesheetDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._timesheetsServiceProxy.delete(timesheet.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._timesheetsServiceProxy.getTimesheetsToExcel(
        this.filterText,
            this.maxTimeSheetDateFilter === undefined ? this.maxTimeSheetDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxTimeSheetDateFilter),
            this.minTimeSheetDateFilter === undefined ? this.minTimeSheetDateFilter : this._dateTimeService.getStartOfDayForDate(this.minTimeSheetDateFilter),
            this.startTimeFilter,
            this.endTimeFilter,
            this.maxTotalWorkHoursFilter == null ? this.maxTotalWorkHoursFilterEmpty: this.maxTotalWorkHoursFilter,
            this.minTotalWorkHoursFilter == null ? this.minTotalWorkHoursFilterEmpty: this.minTotalWorkHoursFilter,
            this.maxTimeOffOrBreakTimeHoursFilter == null ? this.maxTimeOffOrBreakTimeHoursFilterEmpty: this.maxTimeOffOrBreakTimeHoursFilter,
            this.minTimeOffOrBreakTimeHoursFilter == null ? this.minTimeOffOrBreakTimeHoursFilterEmpty: this.minTimeOffOrBreakTimeHoursFilter,
            this.workDetailsFilter,
            this.managerRemarksFilter,
            this.paymentPaidFilter,
            this.clockInIpAddressFilter,
            this.clockOutIpAddressFilter,
            this.macAddressFilter,
            this.maxWorkDayStatusIdFilter == null ? this.maxWorkDayStatusIdFilterEmpty: this.maxWorkDayStatusIdFilter,
            this.minWorkDayStatusIdFilter == null ? this.minWorkDayStatusIdFilterEmpty: this.minWorkDayStatusIdFilter,
            this.employeeNameFilter,
            this.storeNameFilter,
            this.businessNameFilter,
            this.jobTitleFilter,
            this.employeeName2Filter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.maxTimeSheetDateFilter = undefined;
		this.minTimeSheetDateFilter = undefined;
    this.startTimeFilter = '';
    this.endTimeFilter = '';
    this.maxTotalWorkHoursFilter = this.maxTotalWorkHoursFilterEmpty;
		this.minTotalWorkHoursFilter = this.maxTotalWorkHoursFilterEmpty;
    this.maxTimeOffOrBreakTimeHoursFilter = this.maxTimeOffOrBreakTimeHoursFilterEmpty;
		this.minTimeOffOrBreakTimeHoursFilter = this.maxTimeOffOrBreakTimeHoursFilterEmpty;
    this.workDetailsFilter = '';
    this.managerRemarksFilter = '';
    this.paymentPaidFilter = -1;
    this.clockInIpAddressFilter = '';
    this.clockOutIpAddressFilter = '';
    this.macAddressFilter = '';
    this.maxWorkDayStatusIdFilter = this.maxWorkDayStatusIdFilterEmpty;
		this.minWorkDayStatusIdFilter = this.maxWorkDayStatusIdFilterEmpty;
		this.employeeNameFilter = '';
							this.storeNameFilter = '';
							this.businessNameFilter = '';
							this.jobTitleFilter = '';
							this.employeeName2Filter = '';
					
        this.getTimesheets();
    }
}
