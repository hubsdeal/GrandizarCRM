import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductOwnerPublicContactInfosServiceProxy,
    CreateOrEditProductOwnerPublicContactInfoDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductOwnerPublicContactInfoContactLookupTableModalComponent } from './productOwnerPublicContactInfo-contact-lookup-table-modal.component';
import { ProductOwnerPublicContactInfoProductLookupTableModalComponent } from './productOwnerPublicContactInfo-product-lookup-table-modal.component';
import { ProductOwnerPublicContactInfoStoreLookupTableModalComponent } from './productOwnerPublicContactInfo-store-lookup-table-modal.component';
import { ProductOwnerPublicContactInfoUserLookupTableModalComponent } from './productOwnerPublicContactInfo-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductOwnerPublicContactInfoModal',
    templateUrl: './create-or-edit-productOwnerPublicContactInfo-modal.component.html',
})
export class CreateOrEditProductOwnerPublicContactInfoModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productOwnerPublicContactInfoContactLookupTableModal', { static: true })
    productOwnerPublicContactInfoContactLookupTableModal: ProductOwnerPublicContactInfoContactLookupTableModalComponent;
    @ViewChild('productOwnerPublicContactInfoProductLookupTableModal', { static: true })
    productOwnerPublicContactInfoProductLookupTableModal: ProductOwnerPublicContactInfoProductLookupTableModalComponent;
    @ViewChild('productOwnerPublicContactInfoStoreLookupTableModal', { static: true })
    productOwnerPublicContactInfoStoreLookupTableModal: ProductOwnerPublicContactInfoStoreLookupTableModalComponent;
    @ViewChild('productOwnerPublicContactInfoUserLookupTableModal', { static: true })
    productOwnerPublicContactInfoUserLookupTableModal: ProductOwnerPublicContactInfoUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productOwnerPublicContactInfo: CreateOrEditProductOwnerPublicContactInfoDto =
        new CreateOrEditProductOwnerPublicContactInfoDto();

    contactFullName = '';
    productName = '';
    storeName = '';
    userName = '';

    constructor(
        injector: Injector,
        private _productOwnerPublicContactInfosServiceProxy: ProductOwnerPublicContactInfosServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productOwnerPublicContactInfoId?: number): void {
        if (!productOwnerPublicContactInfoId) {
            this.productOwnerPublicContactInfo = new CreateOrEditProductOwnerPublicContactInfoDto();
            this.productOwnerPublicContactInfo.id = productOwnerPublicContactInfoId;
            this.contactFullName = '';
            this.productName = '';
            this.storeName = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productOwnerPublicContactInfosServiceProxy
                .getProductOwnerPublicContactInfoForEdit(productOwnerPublicContactInfoId)
                .subscribe((result) => {
                    this.productOwnerPublicContactInfo = result.productOwnerPublicContactInfo;

                    this.contactFullName = result.contactFullName;
                    this.productName = result.productName;
                    this.storeName = result.storeName;
                    this.userName = result.userName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productOwnerPublicContactInfosServiceProxy
            .createOrEdit(this.productOwnerPublicContactInfo)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectContactModal() {
        this.productOwnerPublicContactInfoContactLookupTableModal.id = this.productOwnerPublicContactInfo.contactId;
        this.productOwnerPublicContactInfoContactLookupTableModal.displayName = this.contactFullName;
        this.productOwnerPublicContactInfoContactLookupTableModal.show();
    }
    openSelectProductModal() {
        this.productOwnerPublicContactInfoProductLookupTableModal.id = this.productOwnerPublicContactInfo.productId;
        this.productOwnerPublicContactInfoProductLookupTableModal.displayName = this.productName;
        this.productOwnerPublicContactInfoProductLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.productOwnerPublicContactInfoStoreLookupTableModal.id = this.productOwnerPublicContactInfo.storeId;
        this.productOwnerPublicContactInfoStoreLookupTableModal.displayName = this.storeName;
        this.productOwnerPublicContactInfoStoreLookupTableModal.show();
    }
    openSelectUserModal() {
        this.productOwnerPublicContactInfoUserLookupTableModal.id = this.productOwnerPublicContactInfo.userId;
        this.productOwnerPublicContactInfoUserLookupTableModal.displayName = this.userName;
        this.productOwnerPublicContactInfoUserLookupTableModal.show();
    }

    setContactIdNull() {
        this.productOwnerPublicContactInfo.contactId = null;
        this.contactFullName = '';
    }
    setProductIdNull() {
        this.productOwnerPublicContactInfo.productId = null;
        this.productName = '';
    }
    setStoreIdNull() {
        this.productOwnerPublicContactInfo.storeId = null;
        this.storeName = '';
    }
    setUserIdNull() {
        this.productOwnerPublicContactInfo.userId = null;
        this.userName = '';
    }

    getNewContactId() {
        this.productOwnerPublicContactInfo.contactId = this.productOwnerPublicContactInfoContactLookupTableModal.id;
        this.contactFullName = this.productOwnerPublicContactInfoContactLookupTableModal.displayName;
    }
    getNewProductId() {
        this.productOwnerPublicContactInfo.productId = this.productOwnerPublicContactInfoProductLookupTableModal.id;
        this.productName = this.productOwnerPublicContactInfoProductLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.productOwnerPublicContactInfo.storeId = this.productOwnerPublicContactInfoStoreLookupTableModal.id;
        this.storeName = this.productOwnerPublicContactInfoStoreLookupTableModal.displayName;
    }
    getNewUserId() {
        this.productOwnerPublicContactInfo.userId = this.productOwnerPublicContactInfoUserLookupTableModal.id;
        this.userName = this.productOwnerPublicContactInfoUserLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
