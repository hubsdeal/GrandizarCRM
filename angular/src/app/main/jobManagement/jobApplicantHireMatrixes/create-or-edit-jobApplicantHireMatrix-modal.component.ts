import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { JobApplicantHireMatrixesServiceProxy, CreateOrEditJobApplicantHireMatrixDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { JobApplicantHireMatrixJobLookupTableModalComponent } from './jobApplicantHireMatrix-job-lookup-table-modal.component';
import { JobApplicantHireMatrixContactLookupTableModalComponent } from './jobApplicantHireMatrix-contact-lookup-table-modal.component';
import { JobApplicantHireMatrixJobHiringTeamLookupTableModalComponent } from './jobApplicantHireMatrix-jobHiringTeam-lookup-table-modal.component';
import { JobApplicantHireMatrixJobApplicantHireStatusTypeLookupTableModalComponent } from './jobApplicantHireMatrix-jobApplicantHireStatusType-lookup-table-modal.component';



@Component({
    selector: 'createOrEditJobApplicantHireMatrixModal',
    templateUrl: './create-or-edit-jobApplicantHireMatrix-modal.component.html'
})
export class CreateOrEditJobApplicantHireMatrixModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('jobApplicantHireMatrixJobLookupTableModal', { static: true }) jobApplicantHireMatrixJobLookupTableModal: JobApplicantHireMatrixJobLookupTableModalComponent;
    @ViewChild('jobApplicantHireMatrixContactLookupTableModal', { static: true }) jobApplicantHireMatrixContactLookupTableModal: JobApplicantHireMatrixContactLookupTableModalComponent;
    @ViewChild('jobApplicantHireMatrixJobHiringTeamLookupTableModal', { static: true }) jobApplicantHireMatrixJobHiringTeamLookupTableModal: JobApplicantHireMatrixJobHiringTeamLookupTableModalComponent;
    @ViewChild('jobApplicantHireMatrixJobApplicantHireStatusTypeLookupTableModal', { static: true }) jobApplicantHireMatrixJobApplicantHireStatusTypeLookupTableModal: JobApplicantHireMatrixJobApplicantHireStatusTypeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobApplicantHireMatrix: CreateOrEditJobApplicantHireMatrixDto = new CreateOrEditJobApplicantHireMatrixDto();

    jobTitle = '';
    contactFullName = '';
    jobHiringTeamFirstName = '';
    jobApplicantHireStatusTypeName = '';



    constructor(
        injector: Injector,
        private _jobApplicantHireMatrixesServiceProxy: JobApplicantHireMatrixesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(jobApplicantHireMatrixId?: number): void {
    

        if (!jobApplicantHireMatrixId) {
            this.jobApplicantHireMatrix = new CreateOrEditJobApplicantHireMatrixDto();
            this.jobApplicantHireMatrix.id = jobApplicantHireMatrixId;
            this.jobTitle = '';
            this.contactFullName = '';
            this.jobHiringTeamFirstName = '';
            this.jobApplicantHireStatusTypeName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._jobApplicantHireMatrixesServiceProxy.getJobApplicantHireMatrixForEdit(jobApplicantHireMatrixId).subscribe(result => {
                this.jobApplicantHireMatrix = result.jobApplicantHireMatrix;

                this.jobTitle = result.jobTitle;
                this.contactFullName = result.contactFullName;
                this.jobHiringTeamFirstName = result.jobHiringTeamFirstName;
                this.jobApplicantHireStatusTypeName = result.jobApplicantHireStatusTypeName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._jobApplicantHireMatrixesServiceProxy.createOrEdit(this.jobApplicantHireMatrix)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectJobModal() {
        this.jobApplicantHireMatrixJobLookupTableModal.id = this.jobApplicantHireMatrix.jobId;
        this.jobApplicantHireMatrixJobLookupTableModal.displayName = this.jobTitle;
        this.jobApplicantHireMatrixJobLookupTableModal.show();
    }
    openSelectContactModal() {
        this.jobApplicantHireMatrixContactLookupTableModal.id = this.jobApplicantHireMatrix.contactId;
        this.jobApplicantHireMatrixContactLookupTableModal.displayName = this.contactFullName;
        this.jobApplicantHireMatrixContactLookupTableModal.show();
    }
    openSelectJobHiringTeamModal() {
        this.jobApplicantHireMatrixJobHiringTeamLookupTableModal.id = this.jobApplicantHireMatrix.jobHiringTeamId;
        this.jobApplicantHireMatrixJobHiringTeamLookupTableModal.displayName = this.jobHiringTeamFirstName;
        this.jobApplicantHireMatrixJobHiringTeamLookupTableModal.show();
    }
    openSelectJobApplicantHireStatusTypeModal() {
        this.jobApplicantHireMatrixJobApplicantHireStatusTypeLookupTableModal.id = this.jobApplicantHireMatrix.jobApplicantHireStatusTypeId;
        this.jobApplicantHireMatrixJobApplicantHireStatusTypeLookupTableModal.displayName = this.jobApplicantHireStatusTypeName;
        this.jobApplicantHireMatrixJobApplicantHireStatusTypeLookupTableModal.show();
    }


    setJobIdNull() {
        this.jobApplicantHireMatrix.jobId = null;
        this.jobTitle = '';
    }
    setContactIdNull() {
        this.jobApplicantHireMatrix.contactId = null;
        this.contactFullName = '';
    }
    setJobHiringTeamIdNull() {
        this.jobApplicantHireMatrix.jobHiringTeamId = null;
        this.jobHiringTeamFirstName = '';
    }
    setJobApplicantHireStatusTypeIdNull() {
        this.jobApplicantHireMatrix.jobApplicantHireStatusTypeId = null;
        this.jobApplicantHireStatusTypeName = '';
    }


    getNewJobId() {
        this.jobApplicantHireMatrix.jobId = this.jobApplicantHireMatrixJobLookupTableModal.id;
        this.jobTitle = this.jobApplicantHireMatrixJobLookupTableModal.displayName;
    }
    getNewContactId() {
        this.jobApplicantHireMatrix.contactId = this.jobApplicantHireMatrixContactLookupTableModal.id;
        this.contactFullName = this.jobApplicantHireMatrixContactLookupTableModal.displayName;
    }
    getNewJobHiringTeamId() {
        this.jobApplicantHireMatrix.jobHiringTeamId = this.jobApplicantHireMatrixJobHiringTeamLookupTableModal.id;
        this.jobHiringTeamFirstName = this.jobApplicantHireMatrixJobHiringTeamLookupTableModal.displayName;
    }
    getNewJobApplicantHireStatusTypeId() {
        this.jobApplicantHireMatrix.jobApplicantHireStatusTypeId = this.jobApplicantHireMatrixJobApplicantHireStatusTypeLookupTableModal.id;
        this.jobApplicantHireStatusTypeName = this.jobApplicantHireMatrixJobApplicantHireStatusTypeLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
