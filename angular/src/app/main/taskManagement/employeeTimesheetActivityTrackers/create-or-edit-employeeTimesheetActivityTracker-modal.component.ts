import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { EmployeeTimesheetActivityTrackersServiceProxy, CreateOrEditEmployeeTimesheetActivityTrackerDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { EmployeeTimesheetActivityTrackerTimesheetLookupTableModalComponent } from './employeeTimesheetActivityTracker-timesheet-lookup-table-modal.component';
import { EmployeeTimesheetActivityTrackerTaskEventLookupTableModalComponent } from './employeeTimesheetActivityTracker-taskEvent-lookup-table-modal.component';
import { EmployeeTimesheetActivityTrackerEmployeeLookupTableModalComponent } from './employeeTimesheetActivityTracker-employee-lookup-table-modal.component';
import { EmployeeTimesheetActivityTrackerTimesheetPolicyLookupTableModalComponent } from './employeeTimesheetActivityTracker-timesheetPolicy-lookup-table-modal.component';
import { EmployeeTimesheetActivityTrackerTaskWorkItemLookupTableModalComponent } from './employeeTimesheetActivityTracker-taskWorkItem-lookup-table-modal.component';



@Component({
    selector: 'createOrEditEmployeeTimesheetActivityTrackerModal',
    templateUrl: './create-or-edit-employeeTimesheetActivityTracker-modal.component.html'
})
export class CreateOrEditEmployeeTimesheetActivityTrackerModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeeTimesheetActivityTrackerTimesheetLookupTableModal', { static: true }) employeeTimesheetActivityTrackerTimesheetLookupTableModal: EmployeeTimesheetActivityTrackerTimesheetLookupTableModalComponent;
    @ViewChild('employeeTimesheetActivityTrackerTaskEventLookupTableModal', { static: true }) employeeTimesheetActivityTrackerTaskEventLookupTableModal: EmployeeTimesheetActivityTrackerTaskEventLookupTableModalComponent;
    @ViewChild('employeeTimesheetActivityTrackerEmployeeLookupTableModal', { static: true }) employeeTimesheetActivityTrackerEmployeeLookupTableModal: EmployeeTimesheetActivityTrackerEmployeeLookupTableModalComponent;
    @ViewChild('employeeTimesheetActivityTrackerTimesheetPolicyLookupTableModal', { static: true }) employeeTimesheetActivityTrackerTimesheetPolicyLookupTableModal: EmployeeTimesheetActivityTrackerTimesheetPolicyLookupTableModalComponent;
    @ViewChild('employeeTimesheetActivityTrackerTaskWorkItemLookupTableModal', { static: true }) employeeTimesheetActivityTrackerTaskWorkItemLookupTableModal: EmployeeTimesheetActivityTrackerTaskWorkItemLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employeeTimesheetActivityTracker: CreateOrEditEmployeeTimesheetActivityTrackerDto = new CreateOrEditEmployeeTimesheetActivityTrackerDto();

    timesheetWorkDetails = '';
    taskEventName = '';
    employeeName = '';
    timesheetPolicyPolicyName = '';
    taskWorkItemName = '';



    constructor(
        injector: Injector,
        private _employeeTimesheetActivityTrackersServiceProxy: EmployeeTimesheetActivityTrackersServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(employeeTimesheetActivityTrackerId?: number): void {
    

        if (!employeeTimesheetActivityTrackerId) {
            this.employeeTimesheetActivityTracker = new CreateOrEditEmployeeTimesheetActivityTrackerDto();
            this.employeeTimesheetActivityTracker.id = employeeTimesheetActivityTrackerId;
            this.timesheetWorkDetails = '';
            this.taskEventName = '';
            this.employeeName = '';
            this.timesheetPolicyPolicyName = '';
            this.taskWorkItemName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._employeeTimesheetActivityTrackersServiceProxy.getEmployeeTimesheetActivityTrackerForEdit(employeeTimesheetActivityTrackerId).subscribe(result => {
                this.employeeTimesheetActivityTracker = result.employeeTimesheetActivityTracker;

                this.timesheetWorkDetails = result.timesheetWorkDetails;
                this.taskEventName = result.taskEventName;
                this.employeeName = result.employeeName;
                this.timesheetPolicyPolicyName = result.timesheetPolicyPolicyName;
                this.taskWorkItemName = result.taskWorkItemName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._employeeTimesheetActivityTrackersServiceProxy.createOrEdit(this.employeeTimesheetActivityTracker)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectTimesheetModal() {
        this.employeeTimesheetActivityTrackerTimesheetLookupTableModal.id = this.employeeTimesheetActivityTracker.timesheetId;
        this.employeeTimesheetActivityTrackerTimesheetLookupTableModal.displayName = this.timesheetWorkDetails;
        this.employeeTimesheetActivityTrackerTimesheetLookupTableModal.show();
    }
    openSelectTaskEventModal() {
        this.employeeTimesheetActivityTrackerTaskEventLookupTableModal.id = this.employeeTimesheetActivityTracker.taskEventId;
        this.employeeTimesheetActivityTrackerTaskEventLookupTableModal.displayName = this.taskEventName;
        this.employeeTimesheetActivityTrackerTaskEventLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.employeeTimesheetActivityTrackerEmployeeLookupTableModal.id = this.employeeTimesheetActivityTracker.employeeId;
        this.employeeTimesheetActivityTrackerEmployeeLookupTableModal.displayName = this.employeeName;
        this.employeeTimesheetActivityTrackerEmployeeLookupTableModal.show();
    }
    openSelectTimesheetPolicyModal() {
        this.employeeTimesheetActivityTrackerTimesheetPolicyLookupTableModal.id = this.employeeTimesheetActivityTracker.timesheetPolicyId;
        this.employeeTimesheetActivityTrackerTimesheetPolicyLookupTableModal.displayName = this.timesheetPolicyPolicyName;
        this.employeeTimesheetActivityTrackerTimesheetPolicyLookupTableModal.show();
    }
    openSelectTaskWorkItemModal() {
        this.employeeTimesheetActivityTrackerTaskWorkItemLookupTableModal.id = this.employeeTimesheetActivityTracker.taskWorkItemId;
        this.employeeTimesheetActivityTrackerTaskWorkItemLookupTableModal.displayName = this.taskWorkItemName;
        this.employeeTimesheetActivityTrackerTaskWorkItemLookupTableModal.show();
    }


    setTimesheetIdNull() {
        this.employeeTimesheetActivityTracker.timesheetId = null;
        this.timesheetWorkDetails = '';
    }
    setTaskEventIdNull() {
        this.employeeTimesheetActivityTracker.taskEventId = null;
        this.taskEventName = '';
    }
    setEmployeeIdNull() {
        this.employeeTimesheetActivityTracker.employeeId = null;
        this.employeeName = '';
    }
    setTimesheetPolicyIdNull() {
        this.employeeTimesheetActivityTracker.timesheetPolicyId = null;
        this.timesheetPolicyPolicyName = '';
    }
    setTaskWorkItemIdNull() {
        this.employeeTimesheetActivityTracker.taskWorkItemId = null;
        this.taskWorkItemName = '';
    }


    getNewTimesheetId() {
        this.employeeTimesheetActivityTracker.timesheetId = this.employeeTimesheetActivityTrackerTimesheetLookupTableModal.id;
        this.timesheetWorkDetails = this.employeeTimesheetActivityTrackerTimesheetLookupTableModal.displayName;
    }
    getNewTaskEventId() {
        this.employeeTimesheetActivityTracker.taskEventId = this.employeeTimesheetActivityTrackerTaskEventLookupTableModal.id;
        this.taskEventName = this.employeeTimesheetActivityTrackerTaskEventLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.employeeTimesheetActivityTracker.employeeId = this.employeeTimesheetActivityTrackerEmployeeLookupTableModal.id;
        this.employeeName = this.employeeTimesheetActivityTrackerEmployeeLookupTableModal.displayName;
    }
    getNewTimesheetPolicyId() {
        this.employeeTimesheetActivityTracker.timesheetPolicyId = this.employeeTimesheetActivityTrackerTimesheetPolicyLookupTableModal.id;
        this.timesheetPolicyPolicyName = this.employeeTimesheetActivityTrackerTimesheetPolicyLookupTableModal.displayName;
    }
    getNewTaskWorkItemId() {
        this.employeeTimesheetActivityTracker.taskWorkItemId = this.employeeTimesheetActivityTrackerTaskWorkItemLookupTableModal.id;
        this.taskWorkItemName = this.employeeTimesheetActivityTrackerTaskWorkItemLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
