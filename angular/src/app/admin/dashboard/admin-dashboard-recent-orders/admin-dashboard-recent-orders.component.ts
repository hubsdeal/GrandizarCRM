import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CreateOrEditOrderModalComponent } from '@app/main/orderManagement/orders/create-or-edit-order-modal.component';
import { ViewOrderModalComponent } from '@app/main/orderManagement/orders/view-order-modal.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { OrdersServiceProxy, OrderOrderSalesChannelLookupTableDto, OrderDto, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { NotifyService } from 'abp-ng2-module';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-admin-dashboard-recent-orders',
  templateUrl: './admin-dashboard-recent-orders.component.html',
  styleUrls: ['./admin-dashboard-recent-orders.component.css'],
  animations: [appModuleAnimation()]
})
export class AdminDashboardRecentOrdersComponent  extends AppComponentBase  {
  @ViewChild('createOrEditOrderModal', { static: true }) createOrEditOrderModal: CreateOrEditOrderModalComponent;
  @ViewChild('viewOrderModal', { static: true }) viewOrderModal: ViewOrderModalComponent;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  invoiceNumberFilter = '';
  deliveryOrPickupFilter = -1;
  paymentCompletedFilter = -1;
  fullNameFilter = '';
  deliveryAddressFilter = '';
  cityFilter = '';
  zipCodeFilter = '';
  notesFilter = '';
  maxDeliveryFeeFilter: number;
  maxDeliveryFeeFilterEmpty: number;
  minDeliveryFeeFilter: number;
  minDeliveryFeeFilterEmpty: number;
  maxSubTotalExcludedTaxFilter: number;
  maxSubTotalExcludedTaxFilterEmpty: number;
  minSubTotalExcludedTaxFilter: number;
  minSubTotalExcludedTaxFilterEmpty: number;
  maxTotalDiscountAmountFilter: number;
  maxTotalDiscountAmountFilterEmpty: number;
  minTotalDiscountAmountFilter: number;
  minTotalDiscountAmountFilterEmpty: number;
  maxTotalTaxAmountFilter: number;
  maxTotalTaxAmountFilterEmpty: number;
  minTotalTaxAmountFilter: number;
  minTotalTaxAmountFilterEmpty: number;
  maxTotalAmountFilter: number;
  maxTotalAmountFilterEmpty: number;
  minTotalAmountFilter: number;
  minTotalAmountFilterEmpty: number;
  emailFilter = '';
  maxDiscountAmountByCodeFilter: number;
  maxDiscountAmountByCodeFilterEmpty: number;
  minDiscountAmountByCodeFilter: number;
  minDiscountAmountByCodeFilterEmpty: number;
  maxGratuityAmountFilter: number;
  maxGratuityAmountFilterEmpty: number;
  minGratuityAmountFilter: number;
  minGratuityAmountFilterEmpty: number;
  maxGratuityPercentageFilter: number;
  maxGratuityPercentageFilterEmpty: number;
  minGratuityPercentageFilter: number;
  minGratuityPercentageFilterEmpty: number;
  maxServiceChargeFilter: number;
  maxServiceChargeFilterEmpty: number;
  minServiceChargeFilter: number;
  minServiceChargeFilterEmpty: number;
  stateNameFilter = '';
  countryNameFilter = '';
  contactFullNameFilter = '';
  orderStatusNameFilter = '';
  currencyNameFilter = '';
  storeNameFilter = '';
  orderSalesChannelNameFilter = '';

  constructor(
      injector: Injector,
      private _ordersServiceProxy: OrdersServiceProxy,
      private _notifyService: NotifyService,
      private _tokenAuth: TokenAuthServiceProxy,
      private _activatedRoute: ActivatedRoute,
      private _fileDownloadService: FileDownloadService,
      private _dateTimeService: DateTimeService
  ) {
      super(injector);
  }

  getOrders(event?: LazyLoadEvent) {
      // if (this.primengTableHelper.shouldResetPaging(event)) {
      //     this.paginator.changePage(0);
      //     if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
      //         return;
      //     }
      // }

      this.primengTableHelper.showLoadingIndicator();

      this._ordersServiceProxy
          .getAll(
              this.filterText,
              this.invoiceNumberFilter,
              this.deliveryOrPickupFilter,
              this.paymentCompletedFilter,
              this.fullNameFilter,
              this.deliveryAddressFilter,
              this.cityFilter,
              this.zipCodeFilter,
              this.notesFilter,
              this.maxDeliveryFeeFilter == null ? this.maxDeliveryFeeFilterEmpty : this.maxDeliveryFeeFilter,
              this.minDeliveryFeeFilter == null ? this.minDeliveryFeeFilterEmpty : this.minDeliveryFeeFilter,
              this.maxSubTotalExcludedTaxFilter == null
                  ? this.maxSubTotalExcludedTaxFilterEmpty
                  : this.maxSubTotalExcludedTaxFilter,
              this.minSubTotalExcludedTaxFilter == null
                  ? this.minSubTotalExcludedTaxFilterEmpty
                  : this.minSubTotalExcludedTaxFilter,
              this.maxTotalDiscountAmountFilter == null
                  ? this.maxTotalDiscountAmountFilterEmpty
                  : this.maxTotalDiscountAmountFilter,
              this.minTotalDiscountAmountFilter == null
                  ? this.minTotalDiscountAmountFilterEmpty
                  : this.minTotalDiscountAmountFilter,
              this.maxTotalTaxAmountFilter == null ? this.maxTotalTaxAmountFilterEmpty : this.maxTotalTaxAmountFilter,
              this.minTotalTaxAmountFilter == null ? this.minTotalTaxAmountFilterEmpty : this.minTotalTaxAmountFilter,
              this.maxTotalAmountFilter == null ? this.maxTotalAmountFilterEmpty : this.maxTotalAmountFilter,
              this.minTotalAmountFilter == null ? this.minTotalAmountFilterEmpty : this.minTotalAmountFilter,
              this.emailFilter,
              this.maxDiscountAmountByCodeFilter == null
                  ? this.maxDiscountAmountByCodeFilterEmpty
                  : this.maxDiscountAmountByCodeFilter,
              this.minDiscountAmountByCodeFilter == null
                  ? this.minDiscountAmountByCodeFilterEmpty
                  : this.minDiscountAmountByCodeFilter,
              this.maxGratuityAmountFilter == null ? this.maxGratuityAmountFilterEmpty : this.maxGratuityAmountFilter,
              this.minGratuityAmountFilter == null ? this.minGratuityAmountFilterEmpty : this.minGratuityAmountFilter,
              this.maxGratuityPercentageFilter == null
                  ? this.maxGratuityPercentageFilterEmpty
                  : this.maxGratuityPercentageFilter,
              this.minGratuityPercentageFilter == null
                  ? this.minGratuityPercentageFilterEmpty
                  : this.minGratuityPercentageFilter,
              this.maxServiceChargeFilter == null ? this.maxServiceChargeFilterEmpty : this.maxServiceChargeFilter,
              this.minServiceChargeFilter == null ? this.minServiceChargeFilterEmpty : this.minServiceChargeFilter,
              this.stateNameFilter,
              this.countryNameFilter,
              this.contactFullNameFilter,
              this.orderStatusNameFilter,
              this.currencyNameFilter,
              this.storeNameFilter,
              this.orderSalesChannelNameFilter,
              '',
              0,
             10
          )
          .subscribe((result) => {
              this.primengTableHelper.totalRecordsCount = result.totalCount;
              this.primengTableHelper.records = result.items;
              this.primengTableHelper.hideLoadingIndicator();
          });
  }

  reloadPage(): void {
      //this.paginator.changePage(this.paginator.getPage());
      this.getOrders();
  }

  createOrder(): void {
      this.createOrEditOrderModal.show();
  }

  deleteOrder(order: OrderDto): void {
      this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
          if (isConfirmed) {
              this._ordersServiceProxy.delete(order.id).subscribe(() => {
                  this.reloadPage();
                  this.notify.success(this.l('SuccessfullyDeleted'));
              });
          }
      });
  }

  exportToExcel(): void {
      this._ordersServiceProxy
          .getOrdersToExcel(
              this.filterText,
              this.invoiceNumberFilter,
              this.deliveryOrPickupFilter,
              this.paymentCompletedFilter,
              this.fullNameFilter,
              this.deliveryAddressFilter,
              this.cityFilter,
              this.zipCodeFilter,
              this.notesFilter,
              this.maxDeliveryFeeFilter == null ? this.maxDeliveryFeeFilterEmpty : this.maxDeliveryFeeFilter,
              this.minDeliveryFeeFilter == null ? this.minDeliveryFeeFilterEmpty : this.minDeliveryFeeFilter,
              this.maxSubTotalExcludedTaxFilter == null
                  ? this.maxSubTotalExcludedTaxFilterEmpty
                  : this.maxSubTotalExcludedTaxFilter,
              this.minSubTotalExcludedTaxFilter == null
                  ? this.minSubTotalExcludedTaxFilterEmpty
                  : this.minSubTotalExcludedTaxFilter,
              this.maxTotalDiscountAmountFilter == null
                  ? this.maxTotalDiscountAmountFilterEmpty
                  : this.maxTotalDiscountAmountFilter,
              this.minTotalDiscountAmountFilter == null
                  ? this.minTotalDiscountAmountFilterEmpty
                  : this.minTotalDiscountAmountFilter,
              this.maxTotalTaxAmountFilter == null ? this.maxTotalTaxAmountFilterEmpty : this.maxTotalTaxAmountFilter,
              this.minTotalTaxAmountFilter == null ? this.minTotalTaxAmountFilterEmpty : this.minTotalTaxAmountFilter,
              this.maxTotalAmountFilter == null ? this.maxTotalAmountFilterEmpty : this.maxTotalAmountFilter,
              this.minTotalAmountFilter == null ? this.minTotalAmountFilterEmpty : this.minTotalAmountFilter,
              this.emailFilter,
              this.maxDiscountAmountByCodeFilter == null
                  ? this.maxDiscountAmountByCodeFilterEmpty
                  : this.maxDiscountAmountByCodeFilter,
              this.minDiscountAmountByCodeFilter == null
                  ? this.minDiscountAmountByCodeFilterEmpty
                  : this.minDiscountAmountByCodeFilter,
              this.maxGratuityAmountFilter == null ? this.maxGratuityAmountFilterEmpty : this.maxGratuityAmountFilter,
              this.minGratuityAmountFilter == null ? this.minGratuityAmountFilterEmpty : this.minGratuityAmountFilter,
              this.maxGratuityPercentageFilter == null
                  ? this.maxGratuityPercentageFilterEmpty
                  : this.maxGratuityPercentageFilter,
              this.minGratuityPercentageFilter == null
                  ? this.minGratuityPercentageFilterEmpty
                  : this.minGratuityPercentageFilter,
              this.maxServiceChargeFilter == null ? this.maxServiceChargeFilterEmpty : this.maxServiceChargeFilter,
              this.minServiceChargeFilter == null ? this.minServiceChargeFilterEmpty : this.minServiceChargeFilter,
              this.stateNameFilter,
              this.countryNameFilter,
              this.contactFullNameFilter,
              this.orderStatusNameFilter,
              this.currencyNameFilter,
              this.storeNameFilter,
              this.orderSalesChannelNameFilter
          )
          .subscribe((result) => {
              this._fileDownloadService.downloadTempFile(result);
          });
  }

  resetFilters(): void {
      this.filterText = '';
      this.invoiceNumberFilter = '';
      this.deliveryOrPickupFilter = -1;
      this.paymentCompletedFilter = -1;
      this.fullNameFilter = '';
      this.deliveryAddressFilter = '';
      this.cityFilter = '';
      this.zipCodeFilter = '';
      this.notesFilter = '';
      this.maxDeliveryFeeFilter = this.maxDeliveryFeeFilterEmpty;
      this.minDeliveryFeeFilter = this.maxDeliveryFeeFilterEmpty;
      this.maxSubTotalExcludedTaxFilter = this.maxSubTotalExcludedTaxFilterEmpty;
      this.minSubTotalExcludedTaxFilter = this.maxSubTotalExcludedTaxFilterEmpty;
      this.maxTotalDiscountAmountFilter = this.maxTotalDiscountAmountFilterEmpty;
      this.minTotalDiscountAmountFilter = this.maxTotalDiscountAmountFilterEmpty;
      this.maxTotalTaxAmountFilter = this.maxTotalTaxAmountFilterEmpty;
      this.minTotalTaxAmountFilter = this.maxTotalTaxAmountFilterEmpty;
      this.maxTotalAmountFilter = this.maxTotalAmountFilterEmpty;
      this.minTotalAmountFilter = this.maxTotalAmountFilterEmpty;
      this.emailFilter = '';
      this.maxDiscountAmountByCodeFilter = this.maxDiscountAmountByCodeFilterEmpty;
      this.minDiscountAmountByCodeFilter = this.maxDiscountAmountByCodeFilterEmpty;
      this.maxGratuityAmountFilter = this.maxGratuityAmountFilterEmpty;
      this.minGratuityAmountFilter = this.maxGratuityAmountFilterEmpty;
      this.maxGratuityPercentageFilter = this.maxGratuityPercentageFilterEmpty;
      this.minGratuityPercentageFilter = this.maxGratuityPercentageFilterEmpty;
      this.maxServiceChargeFilter = this.maxServiceChargeFilterEmpty;
      this.minServiceChargeFilter = this.maxServiceChargeFilterEmpty;
      this.stateNameFilter = '';
      this.countryNameFilter = '';
      this.contactFullNameFilter = '';
      this.orderStatusNameFilter = '';
      this.currencyNameFilter = '';
      this.storeNameFilter = '';
      this.orderSalesChannelNameFilter = '';

      this.getOrders();
  }
}
