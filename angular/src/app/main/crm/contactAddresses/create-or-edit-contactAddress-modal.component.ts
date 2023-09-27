import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContactAddressesServiceProxy, CreateOrEditContactAddressDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContactAddressContactLookupTableModalComponent } from './contactAddress-contact-lookup-table-modal.component';
import { ContactAddressCountryLookupTableModalComponent } from './contactAddress-country-lookup-table-modal.component';
import { ContactAddressStateLookupTableModalComponent } from './contactAddress-state-lookup-table-modal.component';



@Component({
    selector: 'createOrEditContactAddressModal',
    templateUrl: './create-or-edit-contactAddress-modal.component.html'
})
export class CreateOrEditContactAddressModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contactAddressContactLookupTableModal', { static: true }) contactAddressContactLookupTableModal: ContactAddressContactLookupTableModalComponent;
    @ViewChild('contactAddressCountryLookupTableModal', { static: true }) contactAddressCountryLookupTableModal: ContactAddressCountryLookupTableModalComponent;
    @ViewChild('contactAddressStateLookupTableModal', { static: true }) contactAddressStateLookupTableModal: ContactAddressStateLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contactAddress: CreateOrEditContactAddressDto = new CreateOrEditContactAddressDto();

    contactFullName = '';
    countryName = '';
    stateName = '';



    constructor(
        injector: Injector,
        private _contactAddressesServiceProxy: ContactAddressesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(contactAddressId?: number): void {
    

        if (!contactAddressId) {
            this.contactAddress = new CreateOrEditContactAddressDto();
            this.contactAddress.id = contactAddressId;
            this.contactFullName = '';
            this.countryName = '';
            this.stateName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._contactAddressesServiceProxy.getContactAddressForEdit(contactAddressId).subscribe(result => {
                this.contactAddress = result.contactAddress;

                this.contactFullName = result.contactFullName;
                this.countryName = result.countryName;
                this.stateName = result.stateName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._contactAddressesServiceProxy.createOrEdit(this.contactAddress)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectContactModal() {
        this.contactAddressContactLookupTableModal.id = this.contactAddress.contactId;
        this.contactAddressContactLookupTableModal.displayName = this.contactFullName;
        this.contactAddressContactLookupTableModal.show();
    }
    openSelectCountryModal() {
        this.contactAddressCountryLookupTableModal.id = this.contactAddress.countryId;
        this.contactAddressCountryLookupTableModal.displayName = this.countryName;
        this.contactAddressCountryLookupTableModal.show();
    }
    openSelectStateModal() {
        this.contactAddressStateLookupTableModal.id = this.contactAddress.stateId;
        this.contactAddressStateLookupTableModal.displayName = this.stateName;
        this.contactAddressStateLookupTableModal.show();
    }


    setContactIdNull() {
        this.contactAddress.contactId = null;
        this.contactFullName = '';
    }
    setCountryIdNull() {
        this.contactAddress.countryId = null;
        this.countryName = '';
    }
    setStateIdNull() {
        this.contactAddress.stateId = null;
        this.stateName = '';
    }


    getNewContactId() {
        this.contactAddress.contactId = this.contactAddressContactLookupTableModal.id;
        this.contactFullName = this.contactAddressContactLookupTableModal.displayName;
    }
    getNewCountryId() {
        this.contactAddress.countryId = this.contactAddressCountryLookupTableModal.id;
        this.countryName = this.contactAddressCountryLookupTableModal.displayName;
    }
    getNewStateId() {
        this.contactAddress.stateId = this.contactAddressStateLookupTableModal.id;
        this.stateName = this.contactAddressStateLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
