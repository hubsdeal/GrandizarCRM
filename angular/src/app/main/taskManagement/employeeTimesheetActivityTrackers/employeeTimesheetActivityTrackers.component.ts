import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { EmployeeTimesheetActivityTrackersServiceProxy, EmployeeTimesheetActivityTrackerDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmployeeTimesheetActivityTrackerModalComponent } from './create-or-edit-employeeTimesheetActivityTracker-modal.component';

import { ViewEmployeeTimesheetActivityTrackerModalComponent } from './view-employeeTimesheetActivityTracker-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './employeeTimesheetActivityTrackers.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class EmployeeTimesheetActivityTrackersComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditEmployeeTimesheetActivityTrackerModal', { static: true }) createOrEditEmployeeTimesheetActivityTrackerModal: CreateOrEditEmployeeTimesheetActivityTrackerModalComponent;
    @ViewChild('viewEmployeeTimesheetActivityTrackerModal', { static: true }) viewEmployeeTimesheetActivityTrackerModal: ViewEmployeeTimesheetActivityTrackerModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    workNotesFilter = '';
        timesheetWorkDetailsFilter = '';
        taskEventNameFilter = '';
        employeeNameFilter = '';
        timesheetPolicyPolicyNameFilter = '';
        taskWorkItemNameFilter = '';






    constructor(
        injector: Injector,
        private _employeeTimesheetActivityTrackersServiceProxy: EmployeeTimesheetActivityTrackersServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getEmployeeTimesheetActivityTrackers(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._employeeTimesheetActivityTrackersServiceProxy.getAll(
            this.filterText,
            this.workNotesFilter,
            this.timesheetWorkDetailsFilter,
            this.taskEventNameFilter,
            this.employeeNameFilter,
            this.timesheetPolicyPolicyNameFilter,
            this.taskWorkItemNameFilter,
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

    createEmployeeTimesheetActivityTracker(): void {
        this.createOrEditEmployeeTimesheetActivityTrackerModal.show();        
    }


    deleteEmployeeTimesheetActivityTracker(employeeTimesheetActivityTracker: EmployeeTimesheetActivityTrackerDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._employeeTimesheetActivityTrackersServiceProxy.delete(employeeTimesheetActivityTracker.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._employeeTimesheetActivityTrackersServiceProxy.getEmployeeTimesheetActivityTrackersToExcel(
        this.filterText,
            this.workNotesFilter,
            this.timesheetWorkDetailsFilter,
            this.taskEventNameFilter,
            this.employeeNameFilter,
            this.timesheetPolicyPolicyNameFilter,
            this.taskWorkItemNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.workNotesFilter = '';
		this.timesheetWorkDetailsFilter = '';
							this.taskEventNameFilter = '';
							this.employeeNameFilter = '';
							this.timesheetPolicyPolicyNameFilter = '';
							this.taskWorkItemNameFilter = '';
					
        this.getEmployeeTimesheetActivityTrackers();
    }
}
