import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { WishListsServiceProxy, CreateOrEditWishListDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { WishListContactLookupTableModalComponent } from './wishList-contact-lookup-table-modal.component';
import { WishListProductLookupTableModalComponent } from './wishList-product-lookup-table-modal.component';
import { WishListStoreLookupTableModalComponent } from './wishList-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditWishListModal',
    templateUrl: './create-or-edit-wishList-modal.component.html',
})
export class CreateOrEditWishListModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('wishListContactLookupTableModal', { static: true })
    wishListContactLookupTableModal: WishListContactLookupTableModalComponent;
    @ViewChild('wishListProductLookupTableModal', { static: true })
    wishListProductLookupTableModal: WishListProductLookupTableModalComponent;
    @ViewChild('wishListStoreLookupTableModal', { static: true })
    wishListStoreLookupTableModal: WishListStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    wishList: CreateOrEditWishListDto = new CreateOrEditWishListDto();

    contactFullName = '';
    productName = '';
    storeName = '';

    constructor(
        injector: Injector,
        private _wishListsServiceProxy: WishListsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(wishListId?: number): void {
        if (!wishListId) {
            this.wishList = new CreateOrEditWishListDto();
            this.wishList.id = wishListId;
            this.wishList.date = this._dateTimeService.getStartOfDay();
            this.contactFullName = '';
            this.productName = '';
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._wishListsServiceProxy.getWishListForEdit(wishListId).subscribe((result) => {
                this.wishList = result.wishList;

                this.contactFullName = result.contactFullName;
                this.productName = result.productName;
                this.storeName = result.storeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._wishListsServiceProxy
            .createOrEdit(this.wishList)
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
        this.wishListContactLookupTableModal.id = this.wishList.contactId;
        this.wishListContactLookupTableModal.displayName = this.contactFullName;
        this.wishListContactLookupTableModal.show();
    }
    openSelectProductModal() {
        this.wishListProductLookupTableModal.id = this.wishList.productId;
        this.wishListProductLookupTableModal.displayName = this.productName;
        this.wishListProductLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.wishListStoreLookupTableModal.id = this.wishList.storeId;
        this.wishListStoreLookupTableModal.displayName = this.storeName;
        this.wishListStoreLookupTableModal.show();
    }

    setContactIdNull() {
        this.wishList.contactId = null;
        this.contactFullName = '';
    }
    setProductIdNull() {
        this.wishList.productId = null;
        this.productName = '';
    }
    setStoreIdNull() {
        this.wishList.storeId = null;
        this.storeName = '';
    }

    getNewContactId() {
        this.wishList.contactId = this.wishListContactLookupTableModal.id;
        this.contactFullName = this.wishListContactLookupTableModal.displayName;
    }
    getNewProductId() {
        this.wishList.productId = this.wishListProductLookupTableModal.id;
        this.productName = this.wishListProductLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.wishList.storeId = this.wishListStoreLookupTableModal.id;
        this.storeName = this.wishListStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
