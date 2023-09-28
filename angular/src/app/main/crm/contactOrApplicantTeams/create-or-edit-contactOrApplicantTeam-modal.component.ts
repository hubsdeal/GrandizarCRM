import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContactOrApplicantTeamsServiceProxy, CreateOrEditContactOrApplicantTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContactOrApplicantTeamContactLookupTableModalComponent } from './contactOrApplicantTeam-contact-lookup-table-modal.component';
import { ContactOrApplicantTeamEmployeeLookupTableModalComponent } from './contactOrApplicantTeam-employee-lookup-table-modal.component';



@Component({
    selector: 'createOrEditContactOrApplicantTeamModal',
    templateUrl: './create-or-edit-contactOrApplicantTeam-modal.component.html'
})
export class CreateOrEditContactOrApplicantTeamModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contactOrApplicantTeamContactLookupTableModal', { static: true }) contactOrApplicantTeamContactLookupTableModal: ContactOrApplicantTeamContactLookupTableModalComponent;
    @ViewChild('contactOrApplicantTeamEmployeeLookupTableModal', { static: true }) contactOrApplicantTeamEmployeeLookupTableModal: ContactOrApplicantTeamEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contactOrApplicantTeam: CreateOrEditContactOrApplicantTeamDto = new CreateOrEditContactOrApplicantTeamDto();

    contactFullName = '';
    employeeName = '';



    constructor(
        injector: Injector,
        private _contactOrApplicantTeamsServiceProxy: ContactOrApplicantTeamsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(contactOrApplicantTeamId?: number): void {
    

        if (!contactOrApplicantTeamId) {
            this.contactOrApplicantTeam = new CreateOrEditContactOrApplicantTeamDto();
            this.contactOrApplicantTeam.id = contactOrApplicantTeamId;
            this.contactFullName = '';
            this.employeeName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._contactOrApplicantTeamsServiceProxy.getContactOrApplicantTeamForEdit(contactOrApplicantTeamId).subscribe(result => {
                this.contactOrApplicantTeam = result.contactOrApplicantTeam;

                this.contactFullName = result.contactFullName;
                this.employeeName = result.employeeName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._contactOrApplicantTeamsServiceProxy.createOrEdit(this.contactOrApplicantTeam)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectContactModal() {
        this.contactOrApplicantTeamContactLookupTableModal.id = this.contactOrApplicantTeam.contactId;
        this.contactOrApplicantTeamContactLookupTableModal.displayName = this.contactFullName;
        this.contactOrApplicantTeamContactLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.contactOrApplicantTeamEmployeeLookupTableModal.id = this.contactOrApplicantTeam.employeeId;
        this.contactOrApplicantTeamEmployeeLookupTableModal.displayName = this.employeeName;
        this.contactOrApplicantTeamEmployeeLookupTableModal.show();
    }


    setContactIdNull() {
        this.contactOrApplicantTeam.contactId = null;
        this.contactFullName = '';
    }
    setEmployeeIdNull() {
        this.contactOrApplicantTeam.employeeId = null;
        this.employeeName = '';
    }


    getNewContactId() {
        this.contactOrApplicantTeam.contactId = this.contactOrApplicantTeamContactLookupTableModal.id;
        this.contactFullName = this.contactOrApplicantTeamContactLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.contactOrApplicantTeam.employeeId = this.contactOrApplicantTeamEmployeeLookupTableModal.id;
        this.employeeName = this.contactOrApplicantTeamEmployeeLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
