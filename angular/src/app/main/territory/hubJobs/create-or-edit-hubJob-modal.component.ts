import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { HubJobsServiceProxy, CreateOrEditHubJobDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubJobHubLookupTableModalComponent } from './hubJob-hub-lookup-table-modal.component';
import { HubJobJobLookupTableModalComponent } from './hubJob-job-lookup-table-modal.component';



@Component({
    selector: 'createOrEditHubJobModal',
    templateUrl: './create-or-edit-hubJob-modal.component.html'
})
export class CreateOrEditHubJobModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubJobHubLookupTableModal', { static: true }) hubJobHubLookupTableModal: HubJobHubLookupTableModalComponent;
    @ViewChild('hubJobJobLookupTableModal', { static: true }) hubJobJobLookupTableModal: HubJobJobLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubJob: CreateOrEditHubJobDto = new CreateOrEditHubJobDto();

    hubName = '';
    jobTitle = '';



    constructor(
        injector: Injector,
        private _hubJobsServiceProxy: HubJobsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(hubJobId?: number): void {
    

        if (!hubJobId) {
            this.hubJob = new CreateOrEditHubJobDto();
            this.hubJob.id = hubJobId;
            this.hubName = '';
            this.jobTitle = '';


            this.active = true;
            this.modal.show();
        } else {
            this._hubJobsServiceProxy.getHubJobForEdit(hubJobId).subscribe(result => {
                this.hubJob = result.hubJob;

                this.hubName = result.hubName;
                this.jobTitle = result.jobTitle;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._hubJobsServiceProxy.createOrEdit(this.hubJob)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectHubModal() {
        this.hubJobHubLookupTableModal.id = this.hubJob.hubId;
        this.hubJobHubLookupTableModal.displayName = this.hubName;
        this.hubJobHubLookupTableModal.show();
    }
    openSelectJobModal() {
        this.hubJobJobLookupTableModal.id = this.hubJob.jobId;
        this.hubJobJobLookupTableModal.displayName = this.jobTitle;
        this.hubJobJobLookupTableModal.show();
    }


    setHubIdNull() {
        this.hubJob.hubId = null;
        this.hubName = '';
    }
    setJobIdNull() {
        this.hubJob.jobId = null;
        this.jobTitle = '';
    }


    getNewHubId() {
        this.hubJob.hubId = this.hubJobHubLookupTableModal.id;
        this.hubName = this.hubJobHubLookupTableModal.displayName;
    }
    getNewJobId() {
        this.hubJob.jobId = this.hubJobJobLookupTableModal.id;
        this.jobTitle = this.hubJobJobLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
