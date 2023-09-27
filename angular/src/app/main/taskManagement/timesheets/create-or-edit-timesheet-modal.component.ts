import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TimesheetsServiceProxy, CreateOrEditTimesheetDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { TimesheetEmployeeLookupTableModalComponent } from './timesheet-employee-lookup-table-modal.component';
import { TimesheetStoreLookupTableModalComponent } from './timesheet-store-lookup-table-modal.component';
import { TimesheetBusinessLookupTableModalComponent } from './timesheet-business-lookup-table-modal.component';
import { TimesheetJobLookupTableModalComponent } from './timesheet-job-lookup-table-modal.component';



@Component({
    selector: 'createOrEditTimesheetModal',
    templateUrl: './create-or-edit-timesheet-modal.component.html'
})
export class CreateOrEditTimesheetModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('timesheetEmployeeLookupTableModal', { static: true }) timesheetEmployeeLookupTableModal: TimesheetEmployeeLookupTableModalComponent;
    @ViewChild('timesheetStoreLookupTableModal', { static: true }) timesheetStoreLookupTableModal: TimesheetStoreLookupTableModalComponent;
    @ViewChild('timesheetBusinessLookupTableModal', { static: true }) timesheetBusinessLookupTableModal: TimesheetBusinessLookupTableModalComponent;
    @ViewChild('timesheetJobLookupTableModal', { static: true }) timesheetJobLookupTableModal: TimesheetJobLookupTableModalComponent;
    @ViewChild('timesheetEmployeeLookupTableModal2', { static: true }) timesheetEmployeeLookupTableModal2: TimesheetEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    timesheet: CreateOrEditTimesheetDto = new CreateOrEditTimesheetDto();

    employeeName = '';
    storeName = '';
    businessName = '';
    jobTitle = '';
    employeeName2 = '';



    constructor(
        injector: Injector,
        private _timesheetsServiceProxy: TimesheetsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(timesheetId?: number): void {
    

        if (!timesheetId) {
            this.timesheet = new CreateOrEditTimesheetDto();
            this.timesheet.id = timesheetId;
            this.timesheet.timeSheetDate = this._dateTimeService.getStartOfDay();
            this.employeeName = '';
            this.storeName = '';
            this.businessName = '';
            this.jobTitle = '';
            this.employeeName2 = '';


            this.active = true;
            this.modal.show();
        } else {
            this._timesheetsServiceProxy.getTimesheetForEdit(timesheetId).subscribe(result => {
                this.timesheet = result.timesheet;

                this.employeeName = result.employeeName;
                this.storeName = result.storeName;
                this.businessName = result.businessName;
                this.jobTitle = result.jobTitle;
                this.employeeName2 = result.employeeName2;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._timesheetsServiceProxy.createOrEdit(this.timesheet)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectEmployeeModal() {
        this.timesheetEmployeeLookupTableModal.id = this.timesheet.employeeId;
        this.timesheetEmployeeLookupTableModal.displayName = this.employeeName;
        this.timesheetEmployeeLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.timesheetStoreLookupTableModal.id = this.timesheet.storeId;
        this.timesheetStoreLookupTableModal.displayName = this.storeName;
        this.timesheetStoreLookupTableModal.show();
    }
    openSelectBusinessModal() {
        this.timesheetBusinessLookupTableModal.id = this.timesheet.businessId;
        this.timesheetBusinessLookupTableModal.displayName = this.businessName;
        this.timesheetBusinessLookupTableModal.show();
    }
    openSelectJobModal() {
        this.timesheetJobLookupTableModal.id = this.timesheet.jobId;
        this.timesheetJobLookupTableModal.displayName = this.jobTitle;
        this.timesheetJobLookupTableModal.show();
    }
    openSelectEmployeeModal2() {
        this.timesheetEmployeeLookupTableModal2.id = this.timesheet.timesheetApprovalManagerEmployeeId;
        this.timesheetEmployeeLookupTableModal2.displayName = this.employeeName;
        this.timesheetEmployeeLookupTableModal2.show();
    }


    setEmployeeIdNull() {
        this.timesheet.employeeId = null;
        this.employeeName = '';
    }
    setStoreIdNull() {
        this.timesheet.storeId = null;
        this.storeName = '';
    }
    setBusinessIdNull() {
        this.timesheet.businessId = null;
        this.businessName = '';
    }
    setJobIdNull() {
        this.timesheet.jobId = null;
        this.jobTitle = '';
    }
    setTimesheetApprovalManagerEmployeeIdNull() {
        this.timesheet.timesheetApprovalManagerEmployeeId = null;
        this.employeeName2 = '';
    }


    getNewEmployeeId() {
        this.timesheet.employeeId = this.timesheetEmployeeLookupTableModal.id;
        this.employeeName = this.timesheetEmployeeLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.timesheet.storeId = this.timesheetStoreLookupTableModal.id;
        this.storeName = this.timesheetStoreLookupTableModal.displayName;
    }
    getNewBusinessId() {
        this.timesheet.businessId = this.timesheetBusinessLookupTableModal.id;
        this.businessName = this.timesheetBusinessLookupTableModal.displayName;
    }
    getNewJobId() {
        this.timesheet.jobId = this.timesheetJobLookupTableModal.id;
        this.jobTitle = this.timesheetJobLookupTableModal.displayName;
    }
    getNewTimesheetApprovalManagerEmployeeId() {
        this.timesheet.timesheetApprovalManagerEmployeeId = this.timesheetEmployeeLookupTableModal2.id;
        this.employeeName2 = this.timesheetEmployeeLookupTableModal2.displayName;
    }

    startTimeValue(value: any) {
        this.timesheet.startTime = value;
    }

    endTimeValue(value: any) {
        this.timesheet.endTime = value;
    }






    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
