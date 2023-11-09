import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { OrdersServiceProxy, CreateOrEditOrderDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { OrderStateLookupTableModalComponent } from './order-state-lookup-table-modal.component';
import { OrderCountryLookupTableModalComponent } from './order-country-lookup-table-modal.component';
import { OrderContactLookupTableModalComponent } from './order-contact-lookup-table-modal.component';
import { OrderOrderStatusLookupTableModalComponent } from './order-orderStatus-lookup-table-modal.component';
import { OrderCurrencyLookupTableModalComponent } from './order-currency-lookup-table-modal.component';
import { OrderStoreLookupTableModalComponent } from './order-store-lookup-table-modal.component';
import { OrderOrderSalesChannelLookupTableModalComponent } from './order-orderSalesChannel-lookup-table-modal.component';
import { SelectItem } from 'primeng/api';

@Component({
    selector: 'createOrEditOrderModal',
    templateUrl: './create-or-edit-order-modal.component.html',
})
export class CreateOrEditOrderModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderStateLookupTableModal', { static: true })
    orderStateLookupTableModal: OrderStateLookupTableModalComponent;
    @ViewChild('orderCountryLookupTableModal', { static: true })
    orderCountryLookupTableModal: OrderCountryLookupTableModalComponent;
    @ViewChild('orderContactLookupTableModal', { static: true })
    orderContactLookupTableModal: OrderContactLookupTableModalComponent;
    @ViewChild('orderOrderStatusLookupTableModal', { static: true })
    orderOrderStatusLookupTableModal: OrderOrderStatusLookupTableModalComponent;
    @ViewChild('orderCurrencyLookupTableModal', { static: true })
    orderCurrencyLookupTableModal: OrderCurrencyLookupTableModalComponent;
    @ViewChild('orderStoreLookupTableModal', { static: true })
    orderStoreLookupTableModal: OrderStoreLookupTableModalComponent;
    @ViewChild('orderOrderSalesChannelLookupTableModal', { static: true })
    orderOrderSalesChannelLookupTableModal: OrderOrderSalesChannelLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    order: CreateOrEditOrderDto = new CreateOrEditOrderDto();

    stateName = '';
    countryName = '';
    contactFullName = '';
    orderStatusName = '';
    currencyName = '';
    storeName = '';
    orderSalesChannelName = '';

    deliveryOrPickupOptions: SelectItem[];
    constructor(
        injector: Injector,
        private _ordersServiceProxy: OrdersServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(orderId?: number): void {
        if (!orderId) {
            this.order = new CreateOrEditOrderDto();
            this.order.id = orderId;
            this.stateName = '';
            this.countryName = '';
            this.contactFullName = '';
            this.orderStatusName = '';
            this.currencyName = '';
            this.storeName = '';
            this.orderSalesChannelName = '';
            this.order.deliveryOrPickup = true;
            this.active = true;
            this.modal.show();
        } else {
            this._ordersServiceProxy.getOrderForEdit(orderId).subscribe((result) => {
                this.order = result.order;

                this.stateName = result.stateName;
                this.countryName = result.countryName;
                this.contactFullName = result.contactFullName;
                this.orderStatusName = result.orderStatusName;
                this.currencyName = result.currencyName;
                this.storeName = result.storeName;
                this.orderSalesChannelName = result.orderSalesChannelName;

                this.active = true;
                this.modal.show();
            });
        }
        this.deliveryOrPickupOptions = [{ label: 'Delivery', value: true }, { label: 'Pickup', value: false }];
    }

    save(): void {
        this.saving = true;

        this._ordersServiceProxy
            .createOrEdit(this.order)
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

    openSelectStateModal() {
        this.orderStateLookupTableModal.id = this.order.stateId;
        this.orderStateLookupTableModal.displayName = this.stateName;
        this.orderStateLookupTableModal.show();
    }
    openSelectCountryModal() {
        this.orderCountryLookupTableModal.id = this.order.countryId;
        this.orderCountryLookupTableModal.displayName = this.countryName;
        this.orderCountryLookupTableModal.show();
    }
    openSelectContactModal() {
        this.orderContactLookupTableModal.id = this.order.contactId;
        this.orderContactLookupTableModal.displayName = this.contactFullName;
        this.orderContactLookupTableModal.show();
    }
    openSelectOrderStatusModal() {
        this.orderOrderStatusLookupTableModal.id = this.order.orderStatusId;
        this.orderOrderStatusLookupTableModal.displayName = this.orderStatusName;
        this.orderOrderStatusLookupTableModal.show();
    }
    openSelectCurrencyModal() {
        this.orderCurrencyLookupTableModal.id = this.order.currencyId;
        this.orderCurrencyLookupTableModal.displayName = this.currencyName;
        this.orderCurrencyLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.orderStoreLookupTableModal.id = this.order.storeId;
        this.orderStoreLookupTableModal.displayName = this.storeName;
        this.orderStoreLookupTableModal.show();
    }
    openSelectOrderSalesChannelModal() {
        this.orderOrderSalesChannelLookupTableModal.id = this.order.orderSalesChannelId;
        this.orderOrderSalesChannelLookupTableModal.displayName = this.orderSalesChannelName;
        this.orderOrderSalesChannelLookupTableModal.show();
    }

    setStateIdNull() {
        this.order.stateId = null;
        this.stateName = '';
    }
    setCountryIdNull() {
        this.order.countryId = null;
        this.countryName = '';
    }
    setContactIdNull() {
        this.order.contactId = null;
        this.contactFullName = '';
    }
    setOrderStatusIdNull() {
        this.order.orderStatusId = null;
        this.orderStatusName = '';
    }
    setCurrencyIdNull() {
        this.order.currencyId = null;
        this.currencyName = '';
    }
    setStoreIdNull() {
        this.order.storeId = null;
        this.storeName = '';
    }
    setOrderSalesChannelIdNull() {
        this.order.orderSalesChannelId = null;
        this.orderSalesChannelName = '';
    }

    getNewStateId() {
        this.order.stateId = this.orderStateLookupTableModal.id;
        this.stateName = this.orderStateLookupTableModal.displayName;
    }
    getNewCountryId() {
        this.order.countryId = this.orderCountryLookupTableModal.id;
        this.countryName = this.orderCountryLookupTableModal.displayName;
    }
    getNewContactId() {
        this.order.contactId = this.orderContactLookupTableModal.id;
        this.contactFullName = this.orderContactLookupTableModal.displayName;
    }
    getNewOrderStatusId() {
        this.order.orderStatusId = this.orderOrderStatusLookupTableModal.id;
        this.orderStatusName = this.orderOrderStatusLookupTableModal.displayName;
    }
    getNewCurrencyId() {
        this.order.currencyId = this.orderCurrencyLookupTableModal.id;
        this.currencyName = this.orderCurrencyLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.order.storeId = this.orderStoreLookupTableModal.id;
        this.storeName = this.orderStoreLookupTableModal.displayName;
    }
    getNewOrderSalesChannelId() {
        this.order.orderSalesChannelId = this.orderOrderSalesChannelLookupTableModal.id;
        this.orderSalesChannelName = this.orderOrderSalesChannelLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
