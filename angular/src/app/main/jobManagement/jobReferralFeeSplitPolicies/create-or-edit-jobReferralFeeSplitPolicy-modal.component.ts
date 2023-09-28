import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { JobReferralFeeSplitPoliciesServiceProxy, CreateOrEditJobReferralFeeSplitPolicyDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'createOrEditJobReferralFeeSplitPolicyModal',
    templateUrl: './create-or-edit-jobReferralFeeSplitPolicy-modal.component.html'
})
export class CreateOrEditJobReferralFeeSplitPolicyModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobReferralFeeSplitPolicy: CreateOrEditJobReferralFeeSplitPolicyDto = new CreateOrEditJobReferralFeeSplitPolicyDto();




    constructor(
        injector: Injector,
        private _jobReferralFeeSplitPoliciesServiceProxy: JobReferralFeeSplitPoliciesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(jobReferralFeeSplitPolicyId?: number): void {
    

        if (!jobReferralFeeSplitPolicyId) {
            this.jobReferralFeeSplitPolicy = new CreateOrEditJobReferralFeeSplitPolicyDto();
            this.jobReferralFeeSplitPolicy.id = jobReferralFeeSplitPolicyId;


            this.active = true;
            this.modal.show();
        } else {
            this._jobReferralFeeSplitPoliciesServiceProxy.getJobReferralFeeSplitPolicyForEdit(jobReferralFeeSplitPolicyId).subscribe(result => {
                this.jobReferralFeeSplitPolicy = result.jobReferralFeeSplitPolicy;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._jobReferralFeeSplitPoliciesServiceProxy.createOrEdit(this.jobReferralFeeSplitPolicy)
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
