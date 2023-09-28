import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContactCertificationLicensesServiceProxy, CreateOrEditContactCertificationLicenseDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContactCertificationLicenseContactLookupTableModalComponent } from './contactCertificationLicense-contact-lookup-table-modal.component';
import { ContactCertificationLicenseContactDocumentLookupTableModalComponent } from './contactCertificationLicense-contactDocument-lookup-table-modal.component';
import { ContactCertificationLicenseEmployeeLookupTableModalComponent } from './contactCertificationLicense-employee-lookup-table-modal.component';
import { ContactCertificationLicenseBusinessLookupTableModalComponent } from './contactCertificationLicense-business-lookup-table-modal.component';



@Component({
    selector: 'createOrEditContactCertificationLicenseModal',
    templateUrl: './create-or-edit-contactCertificationLicense-modal.component.html'
})
export class CreateOrEditContactCertificationLicenseModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contactCertificationLicenseContactLookupTableModal', { static: true }) contactCertificationLicenseContactLookupTableModal: ContactCertificationLicenseContactLookupTableModalComponent;
    @ViewChild('contactCertificationLicenseContactDocumentLookupTableModal', { static: true }) contactCertificationLicenseContactDocumentLookupTableModal: ContactCertificationLicenseContactDocumentLookupTableModalComponent;
    @ViewChild('contactCertificationLicenseEmployeeLookupTableModal', { static: true }) contactCertificationLicenseEmployeeLookupTableModal: ContactCertificationLicenseEmployeeLookupTableModalComponent;
    @ViewChild('contactCertificationLicenseBusinessLookupTableModal', { static: true }) contactCertificationLicenseBusinessLookupTableModal: ContactCertificationLicenseBusinessLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contactCertificationLicense: CreateOrEditContactCertificationLicenseDto = new CreateOrEditContactCertificationLicenseDto();

    contactFullName = '';
    contactDocumentDocumentTitle = '';
    employeeName = '';
    businessName = '';



    constructor(
        injector: Injector,
        private _contactCertificationLicensesServiceProxy: ContactCertificationLicensesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(contactCertificationLicenseId?: number): void {
    

        if (!contactCertificationLicenseId) {
            this.contactCertificationLicense = new CreateOrEditContactCertificationLicenseDto();
            this.contactCertificationLicense.id = contactCertificationLicenseId;
            this.contactCertificationLicense.fromDate = this._dateTimeService.getStartOfDay();
            this.contactCertificationLicense.toDate = this._dateTimeService.getStartOfDay();
            this.contactCertificationLicense.issueDate = this._dateTimeService.getStartOfDay();
            this.contactCertificationLicense.expirationDate = this._dateTimeService.getStartOfDay();
            this.contactFullName = '';
            this.contactDocumentDocumentTitle = '';
            this.employeeName = '';
            this.businessName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._contactCertificationLicensesServiceProxy.getContactCertificationLicenseForEdit(contactCertificationLicenseId).subscribe(result => {
                this.contactCertificationLicense = result.contactCertificationLicense;

                this.contactFullName = result.contactFullName;
                this.contactDocumentDocumentTitle = result.contactDocumentDocumentTitle;
                this.employeeName = result.employeeName;
                this.businessName = result.businessName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._contactCertificationLicensesServiceProxy.createOrEdit(this.contactCertificationLicense)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectContactModal() {
        this.contactCertificationLicenseContactLookupTableModal.id = this.contactCertificationLicense.contactId;
        this.contactCertificationLicenseContactLookupTableModal.displayName = this.contactFullName;
        this.contactCertificationLicenseContactLookupTableModal.show();
    }
    openSelectContactDocumentModal() {
        this.contactCertificationLicenseContactDocumentLookupTableModal.id = this.contactCertificationLicense.uploadedCertificatetId;
        this.contactCertificationLicenseContactDocumentLookupTableModal.displayName = this.contactDocumentDocumentTitle;
        this.contactCertificationLicenseContactDocumentLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.contactCertificationLicenseEmployeeLookupTableModal.id = this.contactCertificationLicense.verifiedByEmployeeId;
        this.contactCertificationLicenseEmployeeLookupTableModal.displayName = this.employeeName;
        this.contactCertificationLicenseEmployeeLookupTableModal.show();
    }
    openSelectBusinessModal() {
        this.contactCertificationLicenseBusinessLookupTableModal.id = this.contactCertificationLicense.instituteBusinessId;
        this.contactCertificationLicenseBusinessLookupTableModal.displayName = this.businessName;
        this.contactCertificationLicenseBusinessLookupTableModal.show();
    }


    setContactIdNull() {
        this.contactCertificationLicense.contactId = null;
        this.contactFullName = '';
    }
    setUploadedCertificatetIdNull() {
        this.contactCertificationLicense.uploadedCertificatetId = null;
        this.contactDocumentDocumentTitle = '';
    }
    setVerifiedByEmployeeIdNull() {
        this.contactCertificationLicense.verifiedByEmployeeId = null;
        this.employeeName = '';
    }
    setInstituteBusinessIdNull() {
        this.contactCertificationLicense.instituteBusinessId = null;
        this.businessName = '';
    }


    getNewContactId() {
        this.contactCertificationLicense.contactId = this.contactCertificationLicenseContactLookupTableModal.id;
        this.contactFullName = this.contactCertificationLicenseContactLookupTableModal.displayName;
    }
    getNewUploadedCertificatetId() {
        this.contactCertificationLicense.uploadedCertificatetId = this.contactCertificationLicenseContactDocumentLookupTableModal.id;
        this.contactDocumentDocumentTitle = this.contactCertificationLicenseContactDocumentLookupTableModal.displayName;
    }
    getNewVerifiedByEmployeeId() {
        this.contactCertificationLicense.verifiedByEmployeeId = this.contactCertificationLicenseEmployeeLookupTableModal.id;
        this.employeeName = this.contactCertificationLicenseEmployeeLookupTableModal.displayName;
    }
    getNewInstituteBusinessId() {
        this.contactCertificationLicense.instituteBusinessId = this.contactCertificationLicenseBusinessLookupTableModal.id;
        this.businessName = this.contactCertificationLicenseBusinessLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
