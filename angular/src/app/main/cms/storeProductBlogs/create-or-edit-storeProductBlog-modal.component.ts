import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreProductBlogsServiceProxy, CreateOrEditStoreProductBlogDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreProductBlogContentLookupTableModalComponent } from './storeProductBlog-content-lookup-table-modal.component';
import { StoreProductBlogProductLookupTableModalComponent } from './storeProductBlog-product-lookup-table-modal.component';
import { StoreProductBlogStoreLookupTableModalComponent } from './storeProductBlog-store-lookup-table-modal.component';
import { StoreProductBlogHubLookupTableModalComponent } from './storeProductBlog-hub-lookup-table-modal.component';



@Component({
    selector: 'createOrEditStoreProductBlogModal',
    templateUrl: './create-or-edit-storeProductBlog-modal.component.html'
})
export class CreateOrEditStoreProductBlogModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeProductBlogContentLookupTableModal', { static: true }) storeProductBlogContentLookupTableModal: StoreProductBlogContentLookupTableModalComponent;
    @ViewChild('storeProductBlogProductLookupTableModal', { static: true }) storeProductBlogProductLookupTableModal: StoreProductBlogProductLookupTableModalComponent;
    @ViewChild('storeProductBlogStoreLookupTableModal', { static: true }) storeProductBlogStoreLookupTableModal: StoreProductBlogStoreLookupTableModalComponent;
    @ViewChild('storeProductBlogHubLookupTableModal', { static: true }) storeProductBlogHubLookupTableModal: StoreProductBlogHubLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeProductBlog: CreateOrEditStoreProductBlogDto = new CreateOrEditStoreProductBlogDto();

    contentTitle = '';
    productName = '';
    storeName = '';
    hubName = '';



    constructor(
        injector: Injector,
        private _storeProductBlogsServiceProxy: StoreProductBlogsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(storeProductBlogId?: number): void {
    

        if (!storeProductBlogId) {
            this.storeProductBlog = new CreateOrEditStoreProductBlogDto();
            this.storeProductBlog.id = storeProductBlogId;
            this.contentTitle = '';
            this.productName = '';
            this.storeName = '';
            this.hubName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._storeProductBlogsServiceProxy.getStoreProductBlogForEdit(storeProductBlogId).subscribe(result => {
                this.storeProductBlog = result.storeProductBlog;

                this.contentTitle = result.contentTitle;
                this.productName = result.productName;
                this.storeName = result.storeName;
                this.hubName = result.hubName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._storeProductBlogsServiceProxy.createOrEdit(this.storeProductBlog)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectContentModal() {
        this.storeProductBlogContentLookupTableModal.id = this.storeProductBlog.contentId;
        this.storeProductBlogContentLookupTableModal.displayName = this.contentTitle;
        this.storeProductBlogContentLookupTableModal.show();
    }
    openSelectProductModal() {
        this.storeProductBlogProductLookupTableModal.id = this.storeProductBlog.productId;
        this.storeProductBlogProductLookupTableModal.displayName = this.productName;
        this.storeProductBlogProductLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.storeProductBlogStoreLookupTableModal.id = this.storeProductBlog.storeId;
        this.storeProductBlogStoreLookupTableModal.displayName = this.storeName;
        this.storeProductBlogStoreLookupTableModal.show();
    }
    openSelectHubModal() {
        this.storeProductBlogHubLookupTableModal.id = this.storeProductBlog.hubId;
        this.storeProductBlogHubLookupTableModal.displayName = this.hubName;
        this.storeProductBlogHubLookupTableModal.show();
    }


    setContentIdNull() {
        this.storeProductBlog.contentId = null;
        this.contentTitle = '';
    }
    setProductIdNull() {
        this.storeProductBlog.productId = null;
        this.productName = '';
    }
    setStoreIdNull() {
        this.storeProductBlog.storeId = null;
        this.storeName = '';
    }
    setHubIdNull() {
        this.storeProductBlog.hubId = null;
        this.hubName = '';
    }


    getNewContentId() {
        this.storeProductBlog.contentId = this.storeProductBlogContentLookupTableModal.id;
        this.contentTitle = this.storeProductBlogContentLookupTableModal.displayName;
    }
    getNewProductId() {
        this.storeProductBlog.productId = this.storeProductBlogProductLookupTableModal.id;
        this.productName = this.storeProductBlogProductLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.storeProductBlog.storeId = this.storeProductBlogStoreLookupTableModal.id;
        this.storeName = this.storeProductBlogStoreLookupTableModal.displayName;
    }
    getNewHubId() {
        this.storeProductBlog.hubId = this.storeProductBlogHubLookupTableModal.id;
        this.hubName = this.storeProductBlogHubLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
