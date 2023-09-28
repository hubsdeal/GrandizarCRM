import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContactVettedInfosServiceProxy, CreateOrEditContactVettedInfoDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContactVettedInfoContactLookupTableModalComponent } from './contactVettedInfo-contact-lookup-table-modal.component';
import { ContactVettedInfoContactEducationLookupTableModalComponent } from './contactVettedInfo-contactEducation-lookup-table-modal.component';
import { ContactVettedInfoContactExperienceLookupTableModalComponent } from './contactVettedInfo-contactExperience-lookup-table-modal.component';
import { ContactVettedInfoContactCertificationLicenseLookupTableModalComponent } from './contactVettedInfo-contactCertificationLicense-lookup-table-modal.component';
import { ContactVettedInfoEmployeeLookupTableModalComponent } from './contactVettedInfo-employee-lookup-table-modal.component';



@Component({
    selector: 'createOrEditContactVettedInfoModal',
    templateUrl: './create-or-edit-contactVettedInfo-modal.component.html'
})
export class CreateOrEditContactVettedInfoModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contactVettedInfoContactLookupTableModal', { static: true }) contactVettedInfoContactLookupTableModal: ContactVettedInfoContactLookupTableModalComponent;
    @ViewChild('contactVettedInfoContactEducationLookupTableModal', { static: true }) contactVettedInfoContactEducationLookupTableModal: ContactVettedInfoContactEducationLookupTableModalComponent;
    @ViewChild('contactVettedInfoContactExperienceLookupTableModal', { static: true }) contactVettedInfoContactExperienceLookupTableModal: ContactVettedInfoContactExperienceLookupTableModalComponent;
    @ViewChild('contactVettedInfoContactCertificationLicenseLookupTableModal', { static: true }) contactVettedInfoContactCertificationLicenseLookupTableModal: ContactVettedInfoContactCertificationLicenseLookupTableModalComponent;
    @ViewChild('contactVettedInfoEmployeeLookupTableModal', { static: true }) contactVettedInfoEmployeeLookupTableModal: ContactVettedInfoEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contactVettedInfo: CreateOrEditContactVettedInfoDto = new CreateOrEditContactVettedInfoDto();

    contactFullName = '';
    contactEducationName = '';
    contactExperienceJobTitle = '';
    contactCertificationLicenseName = '';
    employeeName = '';



    constructor(
        injector: Injector,
        private _contactVettedInfosServiceProxy: ContactVettedInfosServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(contactVettedInfoId?: number): void {
    

        if (!contactVettedInfoId) {
            this.contactVettedInfo = new CreateOrEditContactVettedInfoDto();
            this.contactVettedInfo.id = contactVettedInfoId;
            this.contactVettedInfo.dateVerification = this._dateTimeService.getStartOfDay();
            this.contactFullName = '';
            this.contactEducationName = '';
            this.contactExperienceJobTitle = '';
            this.contactCertificationLicenseName = '';
            this.employeeName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._contactVettedInfosServiceProxy.getContactVettedInfoForEdit(contactVettedInfoId).subscribe(result => {
                this.contactVettedInfo = result.contactVettedInfo;

                this.contactFullName = result.contactFullName;
                this.contactEducationName = result.contactEducationName;
                this.contactExperienceJobTitle = result.contactExperienceJobTitle;
                this.contactCertificationLicenseName = result.contactCertificationLicenseName;
                this.employeeName = result.employeeName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._contactVettedInfosServiceProxy.createOrEdit(this.contactVettedInfo)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectContactModal() {
        this.contactVettedInfoContactLookupTableModal.id = this.contactVettedInfo.contactId;
        this.contactVettedInfoContactLookupTableModal.displayName = this.contactFullName;
        this.contactVettedInfoContactLookupTableModal.show();
    }
    openSelectContactEducationModal() {
        this.contactVettedInfoContactEducationLookupTableModal.id = this.contactVettedInfo.contactEducationId;
        this.contactVettedInfoContactEducationLookupTableModal.displayName = this.contactEducationName;
        this.contactVettedInfoContactEducationLookupTableModal.show();
    }
    openSelectContactExperienceModal() {
        this.contactVettedInfoContactExperienceLookupTableModal.id = this.contactVettedInfo.contactExperienceId;
        this.contactVettedInfoContactExperienceLookupTableModal.displayName = this.contactExperienceJobTitle;
        this.contactVettedInfoContactExperienceLookupTableModal.show();
    }
    openSelectContactCertificationLicenseModal() {
        this.contactVettedInfoContactCertificationLicenseLookupTableModal.id = this.contactVettedInfo.contactCertificationLicenseId;
        this.contactVettedInfoContactCertificationLicenseLookupTableModal.displayName = this.contactCertificationLicenseName;
        this.contactVettedInfoContactCertificationLicenseLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.contactVettedInfoEmployeeLookupTableModal.id = this.contactVettedInfo.verifiedEmployeeId;
        this.contactVettedInfoEmployeeLookupTableModal.displayName = this.employeeName;
        this.contactVettedInfoEmployeeLookupTableModal.show();
    }


    setContactIdNull() {
        this.contactVettedInfo.contactId = null;
        this.contactFullName = '';
    }
    setContactEducationIdNull() {
        this.contactVettedInfo.contactEducationId = null;
        this.contactEducationName = '';
    }
    setContactExperienceIdNull() {
        this.contactVettedInfo.contactExperienceId = null;
        this.contactExperienceJobTitle = '';
    }
    setContactCertificationLicenseIdNull() {
        this.contactVettedInfo.contactCertificationLicenseId = null;
        this.contactCertificationLicenseName = '';
    }
    setVerifiedEmployeeIdNull() {
        this.contactVettedInfo.verifiedEmployeeId = null;
        this.employeeName = '';
    }


    getNewContactId() {
        this.contactVettedInfo.contactId = this.contactVettedInfoContactLookupTableModal.id;
        this.contactFullName = this.contactVettedInfoContactLookupTableModal.displayName;
    }
    getNewContactEducationId() {
        this.contactVettedInfo.contactEducationId = this.contactVettedInfoContactEducationLookupTableModal.id;
        this.contactEducationName = this.contactVettedInfoContactEducationLookupTableModal.displayName;
    }
    getNewContactExperienceId() {
        this.contactVettedInfo.contactExperienceId = this.contactVettedInfoContactExperienceLookupTableModal.id;
        this.contactExperienceJobTitle = this.contactVettedInfoContactExperienceLookupTableModal.displayName;
    }
    getNewContactCertificationLicenseId() {
        this.contactVettedInfo.contactCertificationLicenseId = this.contactVettedInfoContactCertificationLicenseLookupTableModal.id;
        this.contactCertificationLicenseName = this.contactVettedInfoContactCertificationLicenseLookupTableModal.displayName;
    }
    getNewVerifiedEmployeeId() {
        this.contactVettedInfo.verifiedEmployeeId = this.contactVettedInfoEmployeeLookupTableModal.id;
        this.employeeName = this.contactVettedInfoEmployeeLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
