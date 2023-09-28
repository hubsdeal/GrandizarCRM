import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { EmployeeTimesheetPolicyMappingsServiceProxy, CreateOrEditEmployeeTimesheetPolicyMappingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { EmployeeTimesheetPolicyMappingEmployeeLookupTableModalComponent } from './employeeTimesheetPolicyMapping-employee-lookup-table-modal.component';
import { EmployeeTimesheetPolicyMappingTimesheetPolicyLookupTableModalComponent } from './employeeTimesheetPolicyMapping-timesheetPolicy-lookup-table-modal.component';



@Component({
    selector: 'createOrEditEmployeeTimesheetPolicyMappingModal',
    templateUrl: './create-or-edit-employeeTimesheetPolicyMapping-modal.component.html'
})
export class CreateOrEditEmployeeTimesheetPolicyMappingModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeeTimesheetPolicyMappingEmployeeLookupTableModal', { static: true }) employeeTimesheetPolicyMappingEmployeeLookupTableModal: EmployeeTimesheetPolicyMappingEmployeeLookupTableModalComponent;
    @ViewChild('employeeTimesheetPolicyMappingTimesheetPolicyLookupTableModal', { static: true }) employeeTimesheetPolicyMappingTimesheetPolicyLookupTableModal: EmployeeTimesheetPolicyMappingTimesheetPolicyLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employeeTimesheetPolicyMapping: CreateOrEditEmployeeTimesheetPolicyMappingDto = new CreateOrEditEmployeeTimesheetPolicyMappingDto();

    employeeName = '';
    timesheetPolicyPolicyName = '';



    constructor(
        injector: Injector,
        private _employeeTimesheetPolicyMappingsServiceProxy: EmployeeTimesheetPolicyMappingsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(employeeTimesheetPolicyMappingId?: number): void {
    

        if (!employeeTimesheetPolicyMappingId) {
            this.employeeTimesheetPolicyMapping = new CreateOrEditEmployeeTimesheetPolicyMappingDto();
            this.employeeTimesheetPolicyMapping.id = employeeTimesheetPolicyMappingId;
            this.employeeTimesheetPolicyMapping.startDate = this._dateTimeService.getStartOfDay();
            this.employeeTimesheetPolicyMapping.endDate = this._dateTimeService.getStartOfDay();
            this.employeeName = '';
            this.timesheetPolicyPolicyName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._employeeTimesheetPolicyMappingsServiceProxy.getEmployeeTimesheetPolicyMappingForEdit(employeeTimesheetPolicyMappingId).subscribe(result => {
                this.employeeTimesheetPolicyMapping = result.employeeTimesheetPolicyMapping;

                this.employeeName = result.employeeName;
                this.timesheetPolicyPolicyName = result.timesheetPolicyPolicyName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._employeeTimesheetPolicyMappingsServiceProxy.createOrEdit(this.employeeTimesheetPolicyMapping)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectEmployeeModal() {
        this.employeeTimesheetPolicyMappingEmployeeLookupTableModal.id = this.employeeTimesheetPolicyMapping.employeeId;
        this.employeeTimesheetPolicyMappingEmployeeLookupTableModal.displayName = this.employeeName;
        this.employeeTimesheetPolicyMappingEmployeeLookupTableModal.show();
    }
    openSelectTimesheetPolicyModal() {
        this.employeeTimesheetPolicyMappingTimesheetPolicyLookupTableModal.id = this.employeeTimesheetPolicyMapping.timesheetPolicyId;
        this.employeeTimesheetPolicyMappingTimesheetPolicyLookupTableModal.displayName = this.timesheetPolicyPolicyName;
        this.employeeTimesheetPolicyMappingTimesheetPolicyLookupTableModal.show();
    }


    setEmployeeIdNull() {
        this.employeeTimesheetPolicyMapping.employeeId = null;
        this.employeeName = '';
    }
    setTimesheetPolicyIdNull() {
        this.employeeTimesheetPolicyMapping.timesheetPolicyId = null;
        this.timesheetPolicyPolicyName = '';
    }


    getNewEmployeeId() {
        this.employeeTimesheetPolicyMapping.employeeId = this.employeeTimesheetPolicyMappingEmployeeLookupTableModal.id;
        this.employeeName = this.employeeTimesheetPolicyMappingEmployeeLookupTableModal.displayName;
    }
    getNewTimesheetPolicyId() {
        this.employeeTimesheetPolicyMapping.timesheetPolicyId = this.employeeTimesheetPolicyMappingTimesheetPolicyLookupTableModal.id;
        this.timesheetPolicyPolicyName = this.employeeTimesheetPolicyMappingTimesheetPolicyLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
