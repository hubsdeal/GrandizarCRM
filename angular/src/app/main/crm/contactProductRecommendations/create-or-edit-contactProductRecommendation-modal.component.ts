import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContactProductRecommendationsServiceProxy, CreateOrEditContactProductRecommendationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContactProductRecommendationUserLookupTableModalComponent } from './contactProductRecommendation-user-lookup-table-modal.component';
import { ContactProductRecommendationContactLookupTableModalComponent } from './contactProductRecommendation-contact-lookup-table-modal.component';
import { ContactProductRecommendationHubLookupTableModalComponent } from './contactProductRecommendation-hub-lookup-table-modal.component';
import { ContactProductRecommendationStoreLookupTableModalComponent } from './contactProductRecommendation-store-lookup-table-modal.component';
import { ContactProductRecommendationProductLookupTableModalComponent } from './contactProductRecommendation-product-lookup-table-modal.component';
import { ContactProductRecommendationProductCategoryLookupTableModalComponent } from './contactProductRecommendation-productCategory-lookup-table-modal.component';
import { ContactProductRecommendationJobLookupTableModalComponent } from './contactProductRecommendation-job-lookup-table-modal.component';



@Component({
    selector: 'createOrEditContactProductRecommendationModal',
    templateUrl: './create-or-edit-contactProductRecommendation-modal.component.html'
})
export class CreateOrEditContactProductRecommendationModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contactProductRecommendationUserLookupTableModal', { static: true }) contactProductRecommendationUserLookupTableModal: ContactProductRecommendationUserLookupTableModalComponent;
    @ViewChild('contactProductRecommendationContactLookupTableModal', { static: true }) contactProductRecommendationContactLookupTableModal: ContactProductRecommendationContactLookupTableModalComponent;
    @ViewChild('contactProductRecommendationHubLookupTableModal', { static: true }) contactProductRecommendationHubLookupTableModal: ContactProductRecommendationHubLookupTableModalComponent;
    @ViewChild('contactProductRecommendationStoreLookupTableModal', { static: true }) contactProductRecommendationStoreLookupTableModal: ContactProductRecommendationStoreLookupTableModalComponent;
    @ViewChild('contactProductRecommendationProductLookupTableModal', { static: true }) contactProductRecommendationProductLookupTableModal: ContactProductRecommendationProductLookupTableModalComponent;
    @ViewChild('contactProductRecommendationProductCategoryLookupTableModal', { static: true }) contactProductRecommendationProductCategoryLookupTableModal: ContactProductRecommendationProductCategoryLookupTableModalComponent;
    @ViewChild('contactProductRecommendationJobLookupTableModal', { static: true }) contactProductRecommendationJobLookupTableModal: ContactProductRecommendationJobLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contactProductRecommendation: CreateOrEditContactProductRecommendationDto = new CreateOrEditContactProductRecommendationDto();

    userName = '';
    contactFullName = '';
    hubName = '';
    storeName = '';
    productName = '';
    productCategoryName = '';
    jobTitle = '';



    constructor(
        injector: Injector,
        private _contactProductRecommendationsServiceProxy: ContactProductRecommendationsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(contactProductRecommendationId?: number): void {
    

        if (!contactProductRecommendationId) {
            this.contactProductRecommendation = new CreateOrEditContactProductRecommendationDto();
            this.contactProductRecommendation.id = contactProductRecommendationId;
            this.userName = '';
            this.contactFullName = '';
            this.hubName = '';
            this.storeName = '';
            this.productName = '';
            this.productCategoryName = '';
            this.jobTitle = '';


            this.active = true;
            this.modal.show();
        } else {
            this._contactProductRecommendationsServiceProxy.getContactProductRecommendationForEdit(contactProductRecommendationId).subscribe(result => {
                this.contactProductRecommendation = result.contactProductRecommendation;

                this.userName = result.userName;
                this.contactFullName = result.contactFullName;
                this.hubName = result.hubName;
                this.storeName = result.storeName;
                this.productName = result.productName;
                this.productCategoryName = result.productCategoryName;
                this.jobTitle = result.jobTitle;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._contactProductRecommendationsServiceProxy.createOrEdit(this.contactProductRecommendation)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUserModal() {
        this.contactProductRecommendationUserLookupTableModal.id = this.contactProductRecommendation.userId;
        this.contactProductRecommendationUserLookupTableModal.displayName = this.userName;
        this.contactProductRecommendationUserLookupTableModal.show();
    }
    openSelectContactModal() {
        this.contactProductRecommendationContactLookupTableModal.id = this.contactProductRecommendation.contactId;
        this.contactProductRecommendationContactLookupTableModal.displayName = this.contactFullName;
        this.contactProductRecommendationContactLookupTableModal.show();
    }
    openSelectHubModal() {
        this.contactProductRecommendationHubLookupTableModal.id = this.contactProductRecommendation.hubId;
        this.contactProductRecommendationHubLookupTableModal.displayName = this.hubName;
        this.contactProductRecommendationHubLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.contactProductRecommendationStoreLookupTableModal.id = this.contactProductRecommendation.storeId;
        this.contactProductRecommendationStoreLookupTableModal.displayName = this.storeName;
        this.contactProductRecommendationStoreLookupTableModal.show();
    }
    openSelectProductModal() {
        this.contactProductRecommendationProductLookupTableModal.id = this.contactProductRecommendation.productId;
        this.contactProductRecommendationProductLookupTableModal.displayName = this.productName;
        this.contactProductRecommendationProductLookupTableModal.show();
    }
    openSelectProductCategoryModal() {
        this.contactProductRecommendationProductCategoryLookupTableModal.id = this.contactProductRecommendation.productCategoryId;
        this.contactProductRecommendationProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.contactProductRecommendationProductCategoryLookupTableModal.show();
    }
    openSelectJobModal() {
        this.contactProductRecommendationJobLookupTableModal.id = this.contactProductRecommendation.jobId;
        this.contactProductRecommendationJobLookupTableModal.displayName = this.jobTitle;
        this.contactProductRecommendationJobLookupTableModal.show();
    }


    setUserIdNull() {
        this.contactProductRecommendation.userId = null;
        this.userName = '';
    }
    setContactIdNull() {
        this.contactProductRecommendation.contactId = null;
        this.contactFullName = '';
    }
    setHubIdNull() {
        this.contactProductRecommendation.hubId = null;
        this.hubName = '';
    }
    setStoreIdNull() {
        this.contactProductRecommendation.storeId = null;
        this.storeName = '';
    }
    setProductIdNull() {
        this.contactProductRecommendation.productId = null;
        this.productName = '';
    }
    setProductCategoryIdNull() {
        this.contactProductRecommendation.productCategoryId = null;
        this.productCategoryName = '';
    }
    setJobIdNull() {
        this.contactProductRecommendation.jobId = null;
        this.jobTitle = '';
    }


    getNewUserId() {
        this.contactProductRecommendation.userId = this.contactProductRecommendationUserLookupTableModal.id;
        this.userName = this.contactProductRecommendationUserLookupTableModal.displayName;
    }
    getNewContactId() {
        this.contactProductRecommendation.contactId = this.contactProductRecommendationContactLookupTableModal.id;
        this.contactFullName = this.contactProductRecommendationContactLookupTableModal.displayName;
    }
    getNewHubId() {
        this.contactProductRecommendation.hubId = this.contactProductRecommendationHubLookupTableModal.id;
        this.hubName = this.contactProductRecommendationHubLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.contactProductRecommendation.storeId = this.contactProductRecommendationStoreLookupTableModal.id;
        this.storeName = this.contactProductRecommendationStoreLookupTableModal.displayName;
    }
    getNewProductId() {
        this.contactProductRecommendation.productId = this.contactProductRecommendationProductLookupTableModal.id;
        this.productName = this.contactProductRecommendationProductLookupTableModal.displayName;
    }
    getNewProductCategoryId() {
        this.contactProductRecommendation.productCategoryId = this.contactProductRecommendationProductCategoryLookupTableModal.id;
        this.productCategoryName = this.contactProductRecommendationProductCategoryLookupTableModal.displayName;
    }
    getNewJobId() {
        this.contactProductRecommendation.jobId = this.contactProductRecommendationJobLookupTableModal.id;
        this.jobTitle = this.contactProductRecommendationJobLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
