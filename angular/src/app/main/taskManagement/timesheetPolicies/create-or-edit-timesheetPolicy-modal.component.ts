import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TimesheetPoliciesServiceProxy, CreateOrEditTimesheetPolicyDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'createOrEditTimesheetPolicyModal',
    templateUrl: './create-or-edit-timesheetPolicy-modal.component.html'
})
export class CreateOrEditTimesheetPolicyModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    timesheetPolicy: CreateOrEditTimesheetPolicyDto = new CreateOrEditTimesheetPolicyDto();




    constructor(
        injector: Injector,
        private _timesheetPoliciesServiceProxy: TimesheetPoliciesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(timesheetPolicyId?: number): void {
    

        if (!timesheetPolicyId) {
            this.timesheetPolicy = new CreateOrEditTimesheetPolicyDto();
            this.timesheetPolicy.id = timesheetPolicyId;
            this.timesheetPolicy.effectiveStartDate = this._dateTimeService.getStartOfDay();
            this.timesheetPolicy.effectiveEndDate = this._dateTimeService.getStartOfDay();


            this.active = true;
            this.modal.show();
        } else {
            this._timesheetPoliciesServiceProxy.getTimesheetPolicyForEdit(timesheetPolicyId).subscribe(result => {
                this.timesheetPolicy = result.timesheetPolicy;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._timesheetPoliciesServiceProxy.createOrEdit(this.timesheetPolicy)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }













    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
