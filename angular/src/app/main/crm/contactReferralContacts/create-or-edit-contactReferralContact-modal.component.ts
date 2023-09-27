import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContactReferralContactsServiceProxy, CreateOrEditContactReferralContactDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContactReferralContactContactLookupTableModalComponent } from './contactReferralContact-contact-lookup-table-modal.component';



@Component({
    selector: 'createOrEditContactReferralContactModal',
    templateUrl: './create-or-edit-contactReferralContact-modal.component.html'
})
export class CreateOrEditContactReferralContactModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contactReferralContactContactLookupTableModal', { static: true }) contactReferralContactContactLookupTableModal: ContactReferralContactContactLookupTableModalComponent;
    @ViewChild('contactReferralContactContactLookupTableModal2', { static: true }) contactReferralContactContactLookupTableModal2: ContactReferralContactContactLookupTableModalComponent;
    @ViewChild('contactReferralContactContactLookupTableModal3', { static: true }) contactReferralContactContactLookupTableModal3: ContactReferralContactContactLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contactReferralContact: CreateOrEditContactReferralContactDto = new CreateOrEditContactReferralContactDto();

    contactFullName = '';
    contactFullName2 = '';
    contactFullName3 = '';



    constructor(
        injector: Injector,
        private _contactReferralContactsServiceProxy: ContactReferralContactsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(contactReferralContactId?: number): void {
    

        if (!contactReferralContactId) {
            this.contactReferralContact = new CreateOrEditContactReferralContactDto();
            this.contactReferralContact.id = contactReferralContactId;
            this.contactReferralContact.referredDateTime = this._dateTimeService.getStartOfDay();
            this.contactFullName = '';
            this.contactFullName2 = '';
            this.contactFullName3 = '';


            this.active = true;
            this.modal.show();
        } else {
            this._contactReferralContactsServiceProxy.getContactReferralContactForEdit(contactReferralContactId).subscribe(result => {
                this.contactReferralContact = result.contactReferralContact;

                this.contactFullName = result.contactFullName;
                this.contactFullName2 = result.contactFullName2;
                this.contactFullName3 = result.contactFullName3;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._contactReferralContactsServiceProxy.createOrEdit(this.contactReferralContact)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectContactModal() {
        this.contactReferralContactContactLookupTableModal.id = this.contactReferralContact.referredByContactId;
        this.contactReferralContactContactLookupTableModal.displayName = this.contactFullName;
        this.contactReferralContactContactLookupTableModal.show();
    }
    openSelectContactModal2() {
        this.contactReferralContactContactLookupTableModal2.id = this.contactReferralContact.syncedByContactId;
        this.contactReferralContactContactLookupTableModal2.displayName = this.contactFullName;
        this.contactReferralContactContactLookupTableModal2.show();
    }
    openSelectContactModal3() {
        this.contactReferralContactContactLookupTableModal3.id = this.contactReferralContact.mappedContactId;
        this.contactReferralContactContactLookupTableModal3.displayName = this.contactFullName;
        this.contactReferralContactContactLookupTableModal3.show();
    }


    setReferredByContactIdNull() {
        this.contactReferralContact.referredByContactId = null;
        this.contactFullName = '';
    }
    setSyncedByContactIdNull() {
        this.contactReferralContact.syncedByContactId = null;
        this.contactFullName2 = '';
    }
    setMappedContactIdNull() {
        this.contactReferralContact.mappedContactId = null;
        this.contactFullName3 = '';
    }


    getNewReferredByContactId() {
        this.contactReferralContact.referredByContactId = this.contactReferralContactContactLookupTableModal.id;
        this.contactFullName = this.contactReferralContactContactLookupTableModal.displayName;
    }
    getNewSyncedByContactId() {
        this.contactReferralContact.syncedByContactId = this.contactReferralContactContactLookupTableModal2.id;
        this.contactFullName2 = this.contactReferralContactContactLookupTableModal2.displayName;
    }
    getNewMappedContactId() {
        this.contactReferralContact.mappedContactId = this.contactReferralContactContactLookupTableModal3.id;
        this.contactFullName3 = this.contactReferralContactContactLookupTableModal3.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
