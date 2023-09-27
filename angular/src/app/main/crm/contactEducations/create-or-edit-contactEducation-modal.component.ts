import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContactEducationsServiceProxy, CreateOrEditContactEducationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContactEducationContactLookupTableModalComponent } from './contactEducation-contact-lookup-table-modal.component';
import { ContactEducationEmployeeLookupTableModalComponent } from './contactEducation-employee-lookup-table-modal.component';
import { ContactEducationBusinessLookupTableModalComponent } from './contactEducation-business-lookup-table-modal.component';
import { ContactEducationContactDocumentLookupTableModalComponent } from './contactEducation-contactDocument-lookup-table-modal.component';



@Component({
    selector: 'createOrEditContactEducationModal',
    templateUrl: './create-or-edit-contactEducation-modal.component.html'
})
export class CreateOrEditContactEducationModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contactEducationContactLookupTableModal', { static: true }) contactEducationContactLookupTableModal: ContactEducationContactLookupTableModalComponent;
    @ViewChild('contactEducationEmployeeLookupTableModal', { static: true }) contactEducationEmployeeLookupTableModal: ContactEducationEmployeeLookupTableModalComponent;
    @ViewChild('contactEducationBusinessLookupTableModal', { static: true }) contactEducationBusinessLookupTableModal: ContactEducationBusinessLookupTableModalComponent;
    @ViewChild('contactEducationContactDocumentLookupTableModal', { static: true }) contactEducationContactDocumentLookupTableModal: ContactEducationContactDocumentLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contactEducation: CreateOrEditContactEducationDto = new CreateOrEditContactEducationDto();

    contactFullName = '';
    employeeName = '';
    businessName = '';
    contactDocumentDocumentTitle = '';



    constructor(
        injector: Injector,
        private _contactEducationsServiceProxy: ContactEducationsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(contactEducationId?: number): void {
    

        if (!contactEducationId) {
            this.contactEducation = new CreateOrEditContactEducationDto();
            this.contactEducation.id = contactEducationId;
            this.contactEducation.startDate = this._dateTimeService.getStartOfDay();
            this.contactEducation.endDate = this._dateTimeService.getStartOfDay();
            this.contactFullName = '';
            this.employeeName = '';
            this.businessName = '';
            this.contactDocumentDocumentTitle = '';


            this.active = true;
            this.modal.show();
        } else {
            this._contactEducationsServiceProxy.getContactEducationForEdit(contactEducationId).subscribe(result => {
                this.contactEducation = result.contactEducation;

                this.contactFullName = result.contactFullName;
                this.employeeName = result.employeeName;
                this.businessName = result.businessName;
                this.contactDocumentDocumentTitle = result.contactDocumentDocumentTitle;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._contactEducationsServiceProxy.createOrEdit(this.contactEducation)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectContactModal() {
        this.contactEducationContactLookupTableModal.id = this.contactEducation.contactId;
        this.contactEducationContactLookupTableModal.displayName = this.contactFullName;
        this.contactEducationContactLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.contactEducationEmployeeLookupTableModal.id = this.contactEducation.verifiedByEmployeeId;
        this.contactEducationEmployeeLookupTableModal.displayName = this.employeeName;
        this.contactEducationEmployeeLookupTableModal.show();
    }
    openSelectBusinessModal() {
        this.contactEducationBusinessLookupTableModal.id = this.contactEducation.instituteBusinessId;
        this.contactEducationBusinessLookupTableModal.displayName = this.businessName;
        this.contactEducationBusinessLookupTableModal.show();
    }
    openSelectContactDocumentModal() {
        this.contactEducationContactDocumentLookupTableModal.id = this.contactEducation.uploadedCertificateId;
        this.contactEducationContactDocumentLookupTableModal.displayName = this.contactDocumentDocumentTitle;
        this.contactEducationContactDocumentLookupTableModal.show();
    }


    setContactIdNull() {
        this.contactEducation.contactId = null;
        this.contactFullName = '';
    }
    setVerifiedByEmployeeIdNull() {
        this.contactEducation.verifiedByEmployeeId = null;
        this.employeeName = '';
    }
    setInstituteBusinessIdNull() {
        this.contactEducation.instituteBusinessId = null;
        this.businessName = '';
    }
    setUploadedCertificateIdNull() {
        this.contactEducation.uploadedCertificateId = null;
        this.contactDocumentDocumentTitle = '';
    }


    getNewContactId() {
        this.contactEducation.contactId = this.contactEducationContactLookupTableModal.id;
        this.contactFullName = this.contactEducationContactLookupTableModal.displayName;
    }
    getNewVerifiedByEmployeeId() {
        this.contactEducation.verifiedByEmployeeId = this.contactEducationEmployeeLookupTableModal.id;
        this.employeeName = this.contactEducationEmployeeLookupTableModal.displayName;
    }
    getNewInstituteBusinessId() {
        this.contactEducation.instituteBusinessId = this.contactEducationBusinessLookupTableModal.id;
        this.businessName = this.contactEducationBusinessLookupTableModal.displayName;
    }
    getNewUploadedCertificateId() {
        this.contactEducation.uploadedCertificateId = this.contactEducationContactDocumentLookupTableModal.id;
        this.contactDocumentDocumentTitle = this.contactEducationContactDocumentLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
