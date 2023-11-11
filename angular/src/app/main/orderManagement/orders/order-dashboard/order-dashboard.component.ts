import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditOrderDto, OrdersServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { SelectItem } from 'primeng/api';
import { finalize } from 'rxjs';
import { OrderContactLookupTableModalComponent } from '../order-contact-lookup-table-modal.component';
import { OrderCountryLookupTableModalComponent } from '../order-country-lookup-table-modal.component';
import { OrderCurrencyLookupTableModalComponent } from '../order-currency-lookup-table-modal.component';
import { OrderOrderSalesChannelLookupTableModalComponent } from '../order-orderSalesChannel-lookup-table-modal.component';
import { OrderOrderStatusLookupTableModalComponent } from '../order-orderStatus-lookup-table-modal.component';
import { OrderStateLookupTableModalComponent } from '../order-state-lookup-table-modal.component';
import { OrderStoreLookupTableModalComponent } from '../order-store-lookup-table-modal.component';

@Component({
  selector: 'app-order-dashboard',
  templateUrl: './order-dashboard.component.html',
  styleUrls: ['./order-dashboard.component.css']
})
export class OrderDashboardComponent extends AppComponentBase implements OnInit {
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

  order: CreateOrEditOrderDto = new CreateOrEditOrderDto();

  stateName = '';
  countryName = '';
  contactFullName = '';
  orderStatusName = '';
  currencyName = '';
  storeName = '';
  orderSalesChannelName = '';

  deliveryOrPickupOptions: SelectItem[];
  shippingOptions: SelectItem[];

  localDelivery = true;
  ratingValue:5;
  reviewPublishedOptions: SelectItem[];
  constructor(
    injector: Injector,
    private _ordersServiceProxy: OrdersServiceProxy,
    private _dateTimeService: DateTimeService
  ) {
    super(injector);
  }

 

  save(): void {

    this._ordersServiceProxy
      .createOrEdit(this.order)
      .pipe(
        finalize(() => {
        })
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
      });
  }

  onAddNewCustomer() {
    
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



  ngOnInit(): void {
    this.deliveryOrPickupOptions = [{ label: 'Delivery', value: true }, { label: 'Store Pickup', value: false }];
    this.shippingOptions = [{ label: 'Local Delivery', value: true }, { label: 'Shipping', value: false }];
    this.reviewPublishedOptions = [{ label: 'Un Published', value: false }, { label: 'Published', value: true }];
   }
}
