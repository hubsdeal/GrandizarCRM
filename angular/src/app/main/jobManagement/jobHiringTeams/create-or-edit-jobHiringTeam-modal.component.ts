import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { JobHiringTeamsServiceProxy, CreateOrEditJobHiringTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { JobHiringTeamBusinessLookupTableModalComponent } from './jobHiringTeam-business-lookup-table-modal.component';
import { JobHiringTeamContactLookupTableModalComponent } from './jobHiringTeam-contact-lookup-table-modal.component';
import { JobHiringTeamEmployeeLookupTableModalComponent } from './jobHiringTeam-employee-lookup-table-modal.component';
import { JobHiringTeamJobLookupTableModalComponent } from './jobHiringTeam-job-lookup-table-modal.component';



@Component({
    selector: 'createOrEditJobHiringTeamModal',
    templateUrl: './create-or-edit-jobHiringTeam-modal.component.html'
})
export class CreateOrEditJobHiringTeamModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('jobHiringTeamBusinessLookupTableModal', { static: true }) jobHiringTeamBusinessLookupTableModal: JobHiringTeamBusinessLookupTableModalComponent;
    @ViewChild('jobHiringTeamContactLookupTableModal', { static: true }) jobHiringTeamContactLookupTableModal: JobHiringTeamContactLookupTableModalComponent;
    @ViewChild('jobHiringTeamEmployeeLookupTableModal', { static: true }) jobHiringTeamEmployeeLookupTableModal: JobHiringTeamEmployeeLookupTableModalComponent;
    @ViewChild('jobHiringTeamJobLookupTableModal', { static: true }) jobHiringTeamJobLookupTableModal: JobHiringTeamJobLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobHiringTeam: CreateOrEditJobHiringTeamDto = new CreateOrEditJobHiringTeamDto();

    businessName = '';
    contactFullName = '';
    employeeName = '';
    jobTitle = '';



    constructor(
        injector: Injector,
        private _jobHiringTeamsServiceProxy: JobHiringTeamsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(jobHiringTeamId?: number): void {
    

        if (!jobHiringTeamId) {
            this.jobHiringTeam = new CreateOrEditJobHiringTeamDto();
            this.jobHiringTeam.id = jobHiringTeamId;
            this.businessName = '';
            this.contactFullName = '';
            this.employeeName = '';
            this.jobTitle = '';


            this.active = true;
            this.modal.show();
        } else {
            this._jobHiringTeamsServiceProxy.getJobHiringTeamForEdit(jobHiringTeamId).subscribe(result => {
                this.jobHiringTeam = result.jobHiringTeam;

                this.businessName = result.businessName;
                this.contactFullName = result.contactFullName;
                this.employeeName = result.employeeName;
                this.jobTitle = result.jobTitle;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._jobHiringTeamsServiceProxy.createOrEdit(this.jobHiringTeam)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectBusinessModal() {
        this.jobHiringTeamBusinessLookupTableModal.id = this.jobHiringTeam.businessId;
        this.jobHiringTeamBusinessLookupTableModal.displayName = this.businessName;
        this.jobHiringTeamBusinessLookupTableModal.show();
    }
    openSelectContactModal() {
        this.jobHiringTeamContactLookupTableModal.id = this.jobHiringTeam.contactId;
        this.jobHiringTeamContactLookupTableModal.displayName = this.contactFullName;
        this.jobHiringTeamContactLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.jobHiringTeamEmployeeLookupTableModal.id = this.jobHiringTeam.employeeId;
        this.jobHiringTeamEmployeeLookupTableModal.displayName = this.employeeName;
        this.jobHiringTeamEmployeeLookupTableModal.show();
    }
    openSelectJobModal() {
        this.jobHiringTeamJobLookupTableModal.id = this.jobHiringTeam.jobId;
        this.jobHiringTeamJobLookupTableModal.displayName = this.jobTitle;
        this.jobHiringTeamJobLookupTableModal.show();
    }


    setBusinessIdNull() {
        this.jobHiringTeam.businessId = null;
        this.businessName = '';
    }
    setContactIdNull() {
        this.jobHiringTeam.contactId = null;
        this.contactFullName = '';
    }
    setEmployeeIdNull() {
        this.jobHiringTeam.employeeId = null;
        this.employeeName = '';
    }
    setJobIdNull() {
        this.jobHiringTeam.jobId = null;
        this.jobTitle = '';
    }


    getNewBusinessId() {
        this.jobHiringTeam.businessId = this.jobHiringTeamBusinessLookupTableModal.id;
        this.businessName = this.jobHiringTeamBusinessLookupTableModal.displayName;
    }
    getNewContactId() {
        this.jobHiringTeam.contactId = this.jobHiringTeamContactLookupTableModal.id;
        this.contactFullName = this.jobHiringTeamContactLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.jobHiringTeam.employeeId = this.jobHiringTeamEmployeeLookupTableModal.id;
        this.employeeName = this.jobHiringTeamEmployeeLookupTableModal.displayName;
    }
    getNewJobId() {
        this.jobHiringTeam.jobId = this.jobHiringTeamJobLookupTableModal.id;
        this.jobTitle = this.jobHiringTeamJobLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
