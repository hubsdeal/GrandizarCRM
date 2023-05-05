import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ShoppingCartsServiceProxy, CreateOrEditShoppingCartDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ShoppingCartContactLookupTableModalComponent } from './shoppingCart-contact-lookup-table-modal.component';
import { ShoppingCartOrderLookupTableModalComponent } from './shoppingCart-order-lookup-table-modal.component';
import { ShoppingCartStoreLookupTableModalComponent } from './shoppingCart-store-lookup-table-modal.component';
import { ShoppingCartProductLookupTableModalComponent } from './shoppingCart-product-lookup-table-modal.component';
import { ShoppingCartCurrencyLookupTableModalComponent } from './shoppingCart-currency-lookup-table-modal.component';

@Component({
    selector: 'createOrEditShoppingCartModal',
    templateUrl: './create-or-edit-shoppingCart-modal.component.html',
})
export class CreateOrEditShoppingCartModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('shoppingCartContactLookupTableModal', { static: true })
    shoppingCartContactLookupTableModal: ShoppingCartContactLookupTableModalComponent;
    @ViewChild('shoppingCartOrderLookupTableModal', { static: true })
    shoppingCartOrderLookupTableModal: ShoppingCartOrderLookupTableModalComponent;
    @ViewChild('shoppingCartStoreLookupTableModal', { static: true })
    shoppingCartStoreLookupTableModal: ShoppingCartStoreLookupTableModalComponent;
    @ViewChild('shoppingCartProductLookupTableModal', { static: true })
    shoppingCartProductLookupTableModal: ShoppingCartProductLookupTableModalComponent;
    @ViewChild('shoppingCartCurrencyLookupTableModal', { static: true })
    shoppingCartCurrencyLookupTableModal: ShoppingCartCurrencyLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    shoppingCart: CreateOrEditShoppingCartDto = new CreateOrEditShoppingCartDto();

    contactFullName = '';
    orderInvoiceNumber = '';
    storeName = '';
    productName = '';
    currencyName = '';

    constructor(
        injector: Injector,
        private _shoppingCartsServiceProxy: ShoppingCartsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(shoppingCartId?: number): void {
        if (!shoppingCartId) {
            this.shoppingCart = new CreateOrEditShoppingCartDto();
            this.shoppingCart.id = shoppingCartId;
            this.contactFullName = '';
            this.orderInvoiceNumber = '';
            this.storeName = '';
            this.productName = '';
            this.currencyName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._shoppingCartsServiceProxy.getShoppingCartForEdit(shoppingCartId).subscribe((result) => {
                this.shoppingCart = result.shoppingCart;

                this.contactFullName = result.contactFullName;
                this.orderInvoiceNumber = result.orderInvoiceNumber;
                this.storeName = result.storeName;
                this.productName = result.productName;
                this.currencyName = result.currencyName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._shoppingCartsServiceProxy
            .createOrEdit(this.shoppingCart)
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
        this.shoppingCartContactLookupTableModal.id = this.shoppingCart.contactId;
        this.shoppingCartContactLookupTableModal.displayName = this.contactFullName;
        this.shoppingCartContactLookupTableModal.show();
    }
    openSelectOrderModal() {
        this.shoppingCartOrderLookupTableModal.id = this.shoppingCart.orderId;
        this.shoppingCartOrderLookupTableModal.displayName = this.orderInvoiceNumber;
        this.shoppingCartOrderLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.shoppingCartStoreLookupTableModal.id = this.shoppingCart.storeId;
        this.shoppingCartStoreLookupTableModal.displayName = this.storeName;
        this.shoppingCartStoreLookupTableModal.show();
    }
    openSelectProductModal() {
        this.shoppingCartProductLookupTableModal.id = this.shoppingCart.productId;
        this.shoppingCartProductLookupTableModal.displayName = this.productName;
        this.shoppingCartProductLookupTableModal.show();
    }
    openSelectCurrencyModal() {
        this.shoppingCartCurrencyLookupTableModal.id = this.shoppingCart.currencyId;
        this.shoppingCartCurrencyLookupTableModal.displayName = this.currencyName;
        this.shoppingCartCurrencyLookupTableModal.show();
    }

    setContactIdNull() {
        this.shoppingCart.contactId = null;
        this.contactFullName = '';
    }
    setOrderIdNull() {
        this.shoppingCart.orderId = null;
        this.orderInvoiceNumber = '';
    }
    setStoreIdNull() {
        this.shoppingCart.storeId = null;
        this.storeName = '';
    }
    setProductIdNull() {
        this.shoppingCart.productId = null;
        this.productName = '';
    }
    setCurrencyIdNull() {
        this.shoppingCart.currencyId = null;
        this.currencyName = '';
    }

    getNewContactId() {
        this.shoppingCart.contactId = this.shoppingCartContactLookupTableModal.id;
        this.contactFullName = this.shoppingCartContactLookupTableModal.displayName;
    }
    getNewOrderId() {
        this.shoppingCart.orderId = this.shoppingCartOrderLookupTableModal.id;
        this.orderInvoiceNumber = this.shoppingCartOrderLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.shoppingCart.storeId = this.shoppingCartStoreLookupTableModal.id;
        this.storeName = this.shoppingCartStoreLookupTableModal.displayName;
    }
    getNewProductId() {
        this.shoppingCart.productId = this.shoppingCartProductLookupTableModal.id;
        this.productName = this.shoppingCartProductLookupTableModal.displayName;
    }
    getNewCurrencyId() {
        this.shoppingCart.currencyId = this.shoppingCartCurrencyLookupTableModal.id;
        this.currencyName = this.shoppingCartCurrencyLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
