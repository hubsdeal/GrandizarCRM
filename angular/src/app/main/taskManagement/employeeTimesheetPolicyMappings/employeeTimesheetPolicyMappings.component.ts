import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { EmployeeTimesheetPolicyMappingsServiceProxy, EmployeeTimesheetPolicyMappingDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmployeeTimesheetPolicyMappingModalComponent } from './create-or-edit-employeeTimesheetPolicyMapping-modal.component';

import { ViewEmployeeTimesheetPolicyMappingModalComponent } from './view-employeeTimesheetPolicyMapping-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './employeeTimesheetPolicyMappings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class EmployeeTimesheetPolicyMappingsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditEmployeeTimesheetPolicyMappingModal', { static: true }) createOrEditEmployeeTimesheetPolicyMappingModal: CreateOrEditEmployeeTimesheetPolicyMappingModalComponent;
    @ViewChild('viewEmployeeTimesheetPolicyMappingModal', { static: true }) viewEmployeeTimesheetPolicyMappingModal: ViewEmployeeTimesheetPolicyMappingModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxStartDateFilter : DateTime;
		minStartDateFilter : DateTime;
    maxEndDateFilter : DateTime;
		minEndDateFilter : DateTime;
    isActiveFilter = -1;
        employeeNameFilter = '';
        timesheetPolicyPolicyNameFilter = '';






    constructor(
        injector: Injector,
        private _employeeTimesheetPolicyMappingsServiceProxy: EmployeeTimesheetPolicyMappingsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getEmployeeTimesheetPolicyMappings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._employeeTimesheetPolicyMappingsServiceProxy.getAll(
            this.filterText,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.isActiveFilter,
            this.employeeNameFilter,
            this.timesheetPolicyPolicyNameFilter,
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

    createEmployeeTimesheetPolicyMapping(): void {
        this.createOrEditEmployeeTimesheetPolicyMappingModal.show();        
    }


    deleteEmployeeTimesheetPolicyMapping(employeeTimesheetPolicyMapping: EmployeeTimesheetPolicyMappingDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._employeeTimesheetPolicyMappingsServiceProxy.delete(employeeTimesheetPolicyMapping.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._employeeTimesheetPolicyMappingsServiceProxy.getEmployeeTimesheetPolicyMappingsToExcel(
        this.filterText,
            this.maxStartDateFilter === undefined ? this.maxStartDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
            this.minStartDateFilter === undefined ? this.minStartDateFilter : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
            this.maxEndDateFilter === undefined ? this.maxEndDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
            this.minEndDateFilter === undefined ? this.minEndDateFilter : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
            this.isActiveFilter,
            this.employeeNameFilter,
            this.timesheetPolicyPolicyNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.maxStartDateFilter = undefined;
		this.minStartDateFilter = undefined;
    this.maxEndDateFilter = undefined;
		this.minEndDateFilter = undefined;
    this.isActiveFilter = -1;
		this.employeeNameFilter = '';
							this.timesheetPolicyPolicyNameFilter = '';
					
        this.getEmployeeTimesheetPolicyMappings();
    }
}
