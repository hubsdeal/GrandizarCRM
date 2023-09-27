import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { JobApplicantHireStatusTypesServiceProxy, CreateOrEditJobApplicantHireStatusTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'createOrEditJobApplicantHireStatusTypeModal',
    templateUrl: './create-or-edit-jobApplicantHireStatusType-modal.component.html'
})
export class CreateOrEditJobApplicantHireStatusTypeModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobApplicantHireStatusType: CreateOrEditJobApplicantHireStatusTypeDto = new CreateOrEditJobApplicantHireStatusTypeDto();




    constructor(
        injector: Injector,
        private _jobApplicantHireStatusTypesServiceProxy: JobApplicantHireStatusTypesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(jobApplicantHireStatusTypeId?: number): void {
    

        if (!jobApplicantHireStatusTypeId) {
            this.jobApplicantHireStatusType = new CreateOrEditJobApplicantHireStatusTypeDto();
            this.jobApplicantHireStatusType.id = jobApplicantHireStatusTypeId;


            this.active = true;
            this.modal.show();
        } else {
            this._jobApplicantHireStatusTypesServiceProxy.getJobApplicantHireStatusTypeForEdit(jobApplicantHireStatusTypeId).subscribe(result => {
                this.jobApplicantHireStatusType = result.jobApplicantHireStatusType;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._jobApplicantHireStatusTypesServiceProxy.createOrEdit(this.jobApplicantHireStatusType)
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
