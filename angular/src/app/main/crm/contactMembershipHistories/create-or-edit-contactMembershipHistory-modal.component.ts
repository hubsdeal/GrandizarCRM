import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContactMembershipHistoriesServiceProxy, CreateOrEditContactMembershipHistoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContactMembershipHistoryContactLookupTableModalComponent } from './contactMembershipHistory-contact-lookup-table-modal.component';
import { ContactMembershipHistoryMembershipTypeLookupTableModalComponent } from './contactMembershipHistory-membershipType-lookup-table-modal.component';
import { ContactMembershipHistoryProductLookupTableModalComponent } from './contactMembershipHistory-product-lookup-table-modal.component';



@Component({
    selector: 'createOrEditContactMembershipHistoryModal',
    templateUrl: './create-or-edit-contactMembershipHistory-modal.component.html'
})
export class CreateOrEditContactMembershipHistoryModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contactMembershipHistoryContactLookupTableModal', { static: true }) contactMembershipHistoryContactLookupTableModal: ContactMembershipHistoryContactLookupTableModalComponent;
    @ViewChild('contactMembershipHistoryMembershipTypeLookupTableModal', { static: true }) contactMembershipHistoryMembershipTypeLookupTableModal: ContactMembershipHistoryMembershipTypeLookupTableModalComponent;
    @ViewChild('contactMembershipHistoryProductLookupTableModal', { static: true }) contactMembershipHistoryProductLookupTableModal: ContactMembershipHistoryProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contactMembershipHistory: CreateOrEditContactMembershipHistoryDto = new CreateOrEditContactMembershipHistoryDto();

    contactFullName = '';
    membershipTypeName = '';
    productName = '';



    constructor(
        injector: Injector,
        private _contactMembershipHistoriesServiceProxy: ContactMembershipHistoriesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(contactMembershipHistoryId?: number): void {
    

        if (!contactMembershipHistoryId) {
            this.contactMembershipHistory = new CreateOrEditContactMembershipHistoryDto();
            this.contactMembershipHistory.id = contactMembershipHistoryId;
            this.contactMembershipHistory.startDate = this._dateTimeService.getStartOfDay();
            this.contactMembershipHistory.expirationDate = this._dateTimeService.getStartOfDay();
            this.contactFullName = '';
            this.membershipTypeName = '';
            this.productName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._contactMembershipHistoriesServiceProxy.getContactMembershipHistoryForEdit(contactMembershipHistoryId).subscribe(result => {
                this.contactMembershipHistory = result.contactMembershipHistory;

                this.contactFullName = result.contactFullName;
                this.membershipTypeName = result.membershipTypeName;
                this.productName = result.productName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._contactMembershipHistoriesServiceProxy.createOrEdit(this.contactMembershipHistory)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectContactModal() {
        this.contactMembershipHistoryContactLookupTableModal.id = this.contactMembershipHistory.contactId;
        this.contactMembershipHistoryContactLookupTableModal.displayName = this.contactFullName;
        this.contactMembershipHistoryContactLookupTableModal.show();
    }
    openSelectMembershipTypeModal() {
        this.contactMembershipHistoryMembershipTypeLookupTableModal.id = this.contactMembershipHistory.membershipTypeId;
        this.contactMembershipHistoryMembershipTypeLookupTableModal.displayName = this.membershipTypeName;
        this.contactMembershipHistoryMembershipTypeLookupTableModal.show();
    }
    openSelectProductModal() {
        this.contactMembershipHistoryProductLookupTableModal.id = this.contactMembershipHistory.productId;
        this.contactMembershipHistoryProductLookupTableModal.displayName = this.productName;
        this.contactMembershipHistoryProductLookupTableModal.show();
    }


    setContactIdNull() {
        this.contactMembershipHistory.contactId = null;
        this.contactFullName = '';
    }
    setMembershipTypeIdNull() {
        this.contactMembershipHistory.membershipTypeId = null;
        this.membershipTypeName = '';
    }
    setProductIdNull() {
        this.contactMembershipHistory.productId = null;
        this.productName = '';
    }


    getNewContactId() {
        this.contactMembershipHistory.contactId = this.contactMembershipHistoryContactLookupTableModal.id;
        this.contactFullName = this.contactMembershipHistoryContactLookupTableModal.displayName;
    }
    getNewMembershipTypeId() {
        this.contactMembershipHistory.membershipTypeId = this.contactMembershipHistoryMembershipTypeLookupTableModal.id;
        this.membershipTypeName = this.contactMembershipHistoryMembershipTypeLookupTableModal.displayName;
    }
    getNewProductId() {
        this.contactMembershipHistory.productId = this.contactMembershipHistoryProductLookupTableModal.id;
        this.productName = this.contactMembershipHistoryProductLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
