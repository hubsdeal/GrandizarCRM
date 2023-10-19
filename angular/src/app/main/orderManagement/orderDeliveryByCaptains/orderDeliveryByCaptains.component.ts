import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderDeliveryByCaptainsServiceProxy, OrderDeliveryByCaptainDto, EmployeesServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditOrderDeliveryByCaptainModalComponent } from './create-or-edit-orderDeliveryByCaptain-modal.component';

import { ViewOrderDeliveryByCaptainModalComponent } from './view-orderDeliveryByCaptain-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
  templateUrl: './orderDeliveryByCaptains.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class OrderDeliveryByCaptainsComponent extends AppComponentBase {


  @ViewChild('createOrEditOrderDeliveryByCaptainModal', { static: true }) createOrEditOrderDeliveryByCaptainModal: CreateOrEditOrderDeliveryByCaptainModalComponent;
  @ViewChild('viewOrderDeliveryByCaptainModal', { static: true }) viewOrderDeliveryByCaptainModal: ViewOrderDeliveryByCaptainModalComponent;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  captainSelectionAutoOrManualFilter = -1;
  maxCaptainOrderAcceptedDateTimeFilter: DateTime;
  minCaptainOrderAcceptedDateTimeFilter: DateTime;
  maxCaptainOrderPickedupDateTimeFilter: DateTime;
  minCaptainOrderPickedupDateTimeFilter: DateTime;
  maxOrderDeliveryEstimatedTimeFilter: number;
  maxOrderDeliveryEstimatedTimeFilterEmpty: number;
  minOrderDeliveryEstimatedTimeFilter: number;
  minOrderDeliveryEstimatedTimeFilterEmpty: number;
  maxCaptainOrderDeliveredToCustomerDateTimeFilter: DateTime;
  minCaptainOrderDeliveredToCustomerDateTimeFilter: DateTime;
  maxOrderDeliveryActualTimeFilter: number;
  maxOrderDeliveryActualTimeFilterEmpty: number;
  minOrderDeliveryActualTimeFilter: number;
  minOrderDeliveryActualTimeFilterEmpty: number;
  orderDeliveryRouteDataFilter = '';
  deliveryNotesFilter = '';
  deliveryPointPhotoFilter = '';
  customerSignatureFilter = '';
  maxCaptainRatingByCustomerFilter: number;
  maxCaptainRatingByCustomerFilterEmpty: number;
  minCaptainRatingByCustomerFilter: number;
  minCaptainRatingByCustomerFilterEmpty: number;
  customerNotesFilter = '';
  maxDeliveryStagesFilter: number;
  maxDeliveryStagesFilterEmpty: number;
  minDeliveryStagesFilter: number;
  minDeliveryStagesFilterEmpty: number;
  maxDeliveryCostToDriverFilter: number;
  maxDeliveryCostToDriverFilterEmpty: number;
  minDeliveryCostToDriverFilter: number;
  minDeliveryCostToDriverFilterEmpty: number;
  maxDeliveryCostToCustomerFilter: number;
  maxDeliveryCostToCustomerFilterEmpty: number;
  minDeliveryCostToCustomerFilter: number;
  minDeliveryCostToCustomerFilterEmpty: number;
  orderInvoiceNumberFilter = '';
  storeNameFilter = '';
  contactFullNameFilter = '';
  employeeNameFilter = '';



  nameFilter = '';
  firstNameFilter = '';
  lastNameFilter = '';
  fullAddressFilter = '';
  addressFilter = '';
  zipCodeFilter = '';
  cityFilter = '';
  maxDateOfBirthFilter: DateTime;
  minDateOfBirthFilter: DateTime;
  mobileFilter = '';
  officePhoneFilter = '';
  personalEmailFilter = '';
  businessEmailFilter = '';
  jobTitleFilter = '';
  companyNameFilter = '';
  profileFilter = '';
  maxHireDateFilter: DateTime;
  minHireDateFilter: DateTime;
  facebookFilter = '';
  linkedInFilter = '';
  faxFilter = '';
  profilePictureIdFilter = '';
  currentEmployeeFilter = -1;
  stateNameFilter = '';
  countryNameFilter = '';

  allDeliveryCaptains: any = [];
  totalCaptainCount: number;
  constructor(
    injector: Injector,
    private _orderDeliveryByCaptainsServiceProxy: OrderDeliveryByCaptainsServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService,
    private _employeesServiceProxy: EmployeesServiceProxy,
    private _dateTimeService: DateTimeService
  ) {
    super(injector);
    this.getDeliveryCaptains();
  }

  getOrderDeliveryByCaptains(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      if (this.primengTableHelper.records &&
        this.primengTableHelper.records.length > 0) {
        return;
      }
    }

    this.primengTableHelper.showLoadingIndicator();

    this._orderDeliveryByCaptainsServiceProxy.getAll(
      this.filterText,
      this.captainSelectionAutoOrManualFilter,
      this.maxCaptainOrderAcceptedDateTimeFilter === undefined ? this.maxCaptainOrderAcceptedDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxCaptainOrderAcceptedDateTimeFilter),
      this.minCaptainOrderAcceptedDateTimeFilter === undefined ? this.minCaptainOrderAcceptedDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minCaptainOrderAcceptedDateTimeFilter),
      this.maxCaptainOrderPickedupDateTimeFilter === undefined ? this.maxCaptainOrderPickedupDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxCaptainOrderPickedupDateTimeFilter),
      this.minCaptainOrderPickedupDateTimeFilter === undefined ? this.minCaptainOrderPickedupDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minCaptainOrderPickedupDateTimeFilter),
      this.maxOrderDeliveryEstimatedTimeFilter == null ? this.maxOrderDeliveryEstimatedTimeFilterEmpty : this.maxOrderDeliveryEstimatedTimeFilter,
      this.minOrderDeliveryEstimatedTimeFilter == null ? this.minOrderDeliveryEstimatedTimeFilterEmpty : this.minOrderDeliveryEstimatedTimeFilter,
      this.maxCaptainOrderDeliveredToCustomerDateTimeFilter === undefined ? this.maxCaptainOrderDeliveredToCustomerDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxCaptainOrderDeliveredToCustomerDateTimeFilter),
      this.minCaptainOrderDeliveredToCustomerDateTimeFilter === undefined ? this.minCaptainOrderDeliveredToCustomerDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minCaptainOrderDeliveredToCustomerDateTimeFilter),
      this.maxOrderDeliveryActualTimeFilter == null ? this.maxOrderDeliveryActualTimeFilterEmpty : this.maxOrderDeliveryActualTimeFilter,
      this.minOrderDeliveryActualTimeFilter == null ? this.minOrderDeliveryActualTimeFilterEmpty : this.minOrderDeliveryActualTimeFilter,
      this.orderDeliveryRouteDataFilter,
      this.deliveryNotesFilter,
      this.deliveryPointPhotoFilter,
      this.customerSignatureFilter,
      this.maxCaptainRatingByCustomerFilter == null ? this.maxCaptainRatingByCustomerFilterEmpty : this.maxCaptainRatingByCustomerFilter,
      this.minCaptainRatingByCustomerFilter == null ? this.minCaptainRatingByCustomerFilterEmpty : this.minCaptainRatingByCustomerFilter,
      this.customerNotesFilter,
      this.maxDeliveryStagesFilter == null ? this.maxDeliveryStagesFilterEmpty : this.maxDeliveryStagesFilter,
      this.minDeliveryStagesFilter == null ? this.minDeliveryStagesFilterEmpty : this.minDeliveryStagesFilter,
      this.maxDeliveryCostToDriverFilter == null ? this.maxDeliveryCostToDriverFilterEmpty : this.maxDeliveryCostToDriverFilter,
      this.minDeliveryCostToDriverFilter == null ? this.minDeliveryCostToDriverFilterEmpty : this.minDeliveryCostToDriverFilter,
      this.maxDeliveryCostToCustomerFilter == null ? this.maxDeliveryCostToCustomerFilterEmpty : this.maxDeliveryCostToCustomerFilter,
      this.minDeliveryCostToCustomerFilter == null ? this.minDeliveryCostToCustomerFilterEmpty : this.minDeliveryCostToCustomerFilter,
      this.orderInvoiceNumberFilter,
      this.storeNameFilter,
      this.contactFullNameFilter,
      this.employeeNameFilter,
      this.primengTableHelper.getSorting(this.dataTable),
      this.primengTableHelper.getSkipCount(this.paginator, event),
      this.primengTableHelper.getMaxResultCount(this.paginator, event)
    ).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }


  getDeliveryCaptains() {
    this._employeesServiceProxy
      .getAll(
        undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined,
        undefined,//organizationUnitDisplayNameFilter
        undefined, //contactNameFilter
        -1, //currentEmployee
        undefined, //departmentIdFilter
        '',
        0,
        10000
      )
      .subscribe((result) => {
        this.totalCaptainCount = result.totalCount;
        this.allDeliveryCaptains = result.items;
      });
  }
  reloadPage(): void {
    this.paginator.changePage(this.paginator.getPage());
  }

  createOrderDeliveryByCaptain(): void {
    this.createOrEditOrderDeliveryByCaptainModal.show();
  }


  deleteOrderDeliveryByCaptain(orderDeliveryByCaptain: OrderDeliveryByCaptainDto): void {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._orderDeliveryByCaptainsServiceProxy.delete(orderDeliveryByCaptain.id)
            .subscribe(() => {
              this.reloadPage();
              this.notify.success(this.l('SuccessfullyDeleted'));
            });
        }
      }
    );
  }

  exportToExcel(): void {
    this._orderDeliveryByCaptainsServiceProxy.getOrderDeliveryByCaptainsToExcel(
      this.filterText,
      this.captainSelectionAutoOrManualFilter,
      this.maxCaptainOrderAcceptedDateTimeFilter === undefined ? this.maxCaptainOrderAcceptedDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxCaptainOrderAcceptedDateTimeFilter),
      this.minCaptainOrderAcceptedDateTimeFilter === undefined ? this.minCaptainOrderAcceptedDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minCaptainOrderAcceptedDateTimeFilter),
      this.maxCaptainOrderPickedupDateTimeFilter === undefined ? this.maxCaptainOrderPickedupDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxCaptainOrderPickedupDateTimeFilter),
      this.minCaptainOrderPickedupDateTimeFilter === undefined ? this.minCaptainOrderPickedupDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minCaptainOrderPickedupDateTimeFilter),
      this.maxOrderDeliveryEstimatedTimeFilter == null ? this.maxOrderDeliveryEstimatedTimeFilterEmpty : this.maxOrderDeliveryEstimatedTimeFilter,
      this.minOrderDeliveryEstimatedTimeFilter == null ? this.minOrderDeliveryEstimatedTimeFilterEmpty : this.minOrderDeliveryEstimatedTimeFilter,
      this.maxCaptainOrderDeliveredToCustomerDateTimeFilter === undefined ? this.maxCaptainOrderDeliveredToCustomerDateTimeFilter : this._dateTimeService.getEndOfDayForDate(this.maxCaptainOrderDeliveredToCustomerDateTimeFilter),
      this.minCaptainOrderDeliveredToCustomerDateTimeFilter === undefined ? this.minCaptainOrderDeliveredToCustomerDateTimeFilter : this._dateTimeService.getStartOfDayForDate(this.minCaptainOrderDeliveredToCustomerDateTimeFilter),
      this.maxOrderDeliveryActualTimeFilter == null ? this.maxOrderDeliveryActualTimeFilterEmpty : this.maxOrderDeliveryActualTimeFilter,
      this.minOrderDeliveryActualTimeFilter == null ? this.minOrderDeliveryActualTimeFilterEmpty : this.minOrderDeliveryActualTimeFilter,
      this.orderDeliveryRouteDataFilter,
      this.deliveryNotesFilter,
      this.deliveryPointPhotoFilter,
      this.customerSignatureFilter,
      this.maxCaptainRatingByCustomerFilter == null ? this.maxCaptainRatingByCustomerFilterEmpty : this.maxCaptainRatingByCustomerFilter,
      this.minCaptainRatingByCustomerFilter == null ? this.minCaptainRatingByCustomerFilterEmpty : this.minCaptainRatingByCustomerFilter,
      this.customerNotesFilter,
      this.maxDeliveryStagesFilter == null ? this.maxDeliveryStagesFilterEmpty : this.maxDeliveryStagesFilter,
      this.minDeliveryStagesFilter == null ? this.minDeliveryStagesFilterEmpty : this.minDeliveryStagesFilter,
      this.maxDeliveryCostToDriverFilter == null ? this.maxDeliveryCostToDriverFilterEmpty : this.maxDeliveryCostToDriverFilter,
      this.minDeliveryCostToDriverFilter == null ? this.minDeliveryCostToDriverFilterEmpty : this.minDeliveryCostToDriverFilter,
      this.maxDeliveryCostToCustomerFilter == null ? this.maxDeliveryCostToCustomerFilterEmpty : this.maxDeliveryCostToCustomerFilter,
      this.minDeliveryCostToCustomerFilter == null ? this.minDeliveryCostToCustomerFilterEmpty : this.minDeliveryCostToCustomerFilter,
      this.orderInvoiceNumberFilter,
      this.storeNameFilter,
      this.contactFullNameFilter,
      this.employeeNameFilter,
    )
      .subscribe(result => {
        this._fileDownloadService.downloadTempFile(result);
      });
  }






  resetFilters(): void {
    this.filterText = '';
    this.captainSelectionAutoOrManualFilter = -1;
    this.maxCaptainOrderAcceptedDateTimeFilter = undefined;
    this.minCaptainOrderAcceptedDateTimeFilter = undefined;
    this.maxCaptainOrderPickedupDateTimeFilter = undefined;
    this.minCaptainOrderPickedupDateTimeFilter = undefined;
    this.maxOrderDeliveryEstimatedTimeFilter = this.maxOrderDeliveryEstimatedTimeFilterEmpty;
    this.minOrderDeliveryEstimatedTimeFilter = this.maxOrderDeliveryEstimatedTimeFilterEmpty;
    this.maxCaptainOrderDeliveredToCustomerDateTimeFilter = undefined;
    this.minCaptainOrderDeliveredToCustomerDateTimeFilter = undefined;
    this.maxOrderDeliveryActualTimeFilter = this.maxOrderDeliveryActualTimeFilterEmpty;
    this.minOrderDeliveryActualTimeFilter = this.maxOrderDeliveryActualTimeFilterEmpty;
    this.orderDeliveryRouteDataFilter = '';
    this.deliveryNotesFilter = '';
    this.deliveryPointPhotoFilter = '';
    this.customerSignatureFilter = '';
    this.maxCaptainRatingByCustomerFilter = this.maxCaptainRatingByCustomerFilterEmpty;
    this.minCaptainRatingByCustomerFilter = this.maxCaptainRatingByCustomerFilterEmpty;
    this.customerNotesFilter = '';
    this.maxDeliveryStagesFilter = this.maxDeliveryStagesFilterEmpty;
    this.minDeliveryStagesFilter = this.maxDeliveryStagesFilterEmpty;
    this.maxDeliveryCostToDriverFilter = this.maxDeliveryCostToDriverFilterEmpty;
    this.minDeliveryCostToDriverFilter = this.maxDeliveryCostToDriverFilterEmpty;
    this.maxDeliveryCostToCustomerFilter = this.maxDeliveryCostToCustomerFilterEmpty;
    this.minDeliveryCostToCustomerFilter = this.maxDeliveryCostToCustomerFilterEmpty;
    this.orderInvoiceNumberFilter = '';
    this.storeNameFilter = '';
    this.contactFullNameFilter = '';
    this.employeeNameFilter = '';

    this.getOrderDeliveryByCaptains();
  }
}
