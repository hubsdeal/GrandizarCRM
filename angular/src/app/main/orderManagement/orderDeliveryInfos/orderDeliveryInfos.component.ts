import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderDeliveryInfosServiceProxy, OrderDeliveryInfoDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditOrderDeliveryInfoModalComponent } from './create-or-edit-orderDeliveryInfo-modal.component';

import { ViewOrderDeliveryInfoModalComponent } from './view-orderDeliveryInfo-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './orderDeliveryInfos.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class OrderDeliveryInfosComponent extends AppComponentBase {
    @ViewChild('createOrEditOrderDeliveryInfoModal', { static: true })
    createOrEditOrderDeliveryInfoModal: CreateOrEditOrderDeliveryInfoModalComponent;
    @ViewChild('viewOrderDeliveryInfoModal', { static: true })
    viewOrderDeliveryInfoModal: ViewOrderDeliveryInfoModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    trackingNumberFilter = '';
    maxTotalWeightFilter: number;
    maxTotalWeightFilterEmpty: number;
    minTotalWeightFilter: number;
    minTotalWeightFilterEmpty: number;
    deliveryProviderIdFilter = '';
    maxDispatchDateFilter: DateTime;
    minDispatchDateFilter: DateTime;
    dispatchTimeFilter = '';
    maxDeliverToCustomerDateFilter: DateTime;
    minDeliverToCustomerDateFilter: DateTime;
    deliverToCustomerTimeFilter = '';
    deliveryNotesFilter = '';
    customerAcknowledgedFilter = -1;
    customerSignatureFilter = '';
    maxCateringDateFilter: DateTime;
    minCateringDateFilter: DateTime;
    cateringTimeFilter = '';
    maxDeliveryDateFilter: DateTime;
    minDeliveryDateFilter: DateTime;
    deliveryTimeFilter = '';
    maxDineInDateFilter: DateTime;
    minDineInDateFilter: DateTime;
    dineInTimeFilter = '';
    includedChildrenFilter = -1;
    isAsapFilter = -1;
    isPickupCateringFilter = -1;
    maxNumberOfGuestsFilter: number;
    maxNumberOfGuestsFilterEmpty: number;
    minNumberOfGuestsFilter: number;
    minNumberOfGuestsFilterEmpty: number;
    maxPickupDateFilter: DateTime;
    minPickupDateFilter: DateTime;
    pickupTimeFilter = '';
    employeeNameFilter = '';
    orderInvoiceNumberFilter = '';

    constructor(
        injector: Injector,
        private _orderDeliveryInfosServiceProxy: OrderDeliveryInfosServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getOrderDeliveryInfos(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._orderDeliveryInfosServiceProxy
            .getAll(
                this.filterText,
                this.trackingNumberFilter,
                this.maxTotalWeightFilter == null ? this.maxTotalWeightFilterEmpty : this.maxTotalWeightFilter,
                this.minTotalWeightFilter == null ? this.minTotalWeightFilterEmpty : this.minTotalWeightFilter,
                this.deliveryProviderIdFilter,
                this.maxDispatchDateFilter === undefined
                    ? this.maxDispatchDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxDispatchDateFilter),
                this.minDispatchDateFilter === undefined
                    ? this.minDispatchDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minDispatchDateFilter),
                this.dispatchTimeFilter,
                this.maxDeliverToCustomerDateFilter === undefined
                    ? this.maxDeliverToCustomerDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxDeliverToCustomerDateFilter),
                this.minDeliverToCustomerDateFilter === undefined
                    ? this.minDeliverToCustomerDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minDeliverToCustomerDateFilter),
                this.deliverToCustomerTimeFilter,
                this.deliveryNotesFilter,
                this.customerAcknowledgedFilter,
                this.customerSignatureFilter,
                this.maxCateringDateFilter === undefined
                    ? this.maxCateringDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxCateringDateFilter),
                this.minCateringDateFilter === undefined
                    ? this.minCateringDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minCateringDateFilter),
                this.cateringTimeFilter,
                this.maxDeliveryDateFilter === undefined
                    ? this.maxDeliveryDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxDeliveryDateFilter),
                this.minDeliveryDateFilter === undefined
                    ? this.minDeliveryDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minDeliveryDateFilter),
                this.deliveryTimeFilter,
                this.maxDineInDateFilter === undefined
                    ? this.maxDineInDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxDineInDateFilter),
                this.minDineInDateFilter === undefined
                    ? this.minDineInDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minDineInDateFilter),
                this.dineInTimeFilter,
                this.includedChildrenFilter,
                this.isAsapFilter,
                this.isPickupCateringFilter,
                this.maxNumberOfGuestsFilter == null ? this.maxNumberOfGuestsFilterEmpty : this.maxNumberOfGuestsFilter,
                this.minNumberOfGuestsFilter == null ? this.minNumberOfGuestsFilterEmpty : this.minNumberOfGuestsFilter,
                this.maxPickupDateFilter === undefined
                    ? this.maxPickupDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxPickupDateFilter),
                this.minPickupDateFilter === undefined
                    ? this.minPickupDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minPickupDateFilter),
                this.pickupTimeFilter,
                this.employeeNameFilter,
                this.orderInvoiceNumberFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createOrderDeliveryInfo(): void {
        this.createOrEditOrderDeliveryInfoModal.show();
    }

    deleteOrderDeliveryInfo(orderDeliveryInfo: OrderDeliveryInfoDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._orderDeliveryInfosServiceProxy.delete(orderDeliveryInfo.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._orderDeliveryInfosServiceProxy
            .getOrderDeliveryInfosToExcel(
                this.filterText,
                this.trackingNumberFilter,
                this.maxTotalWeightFilter == null ? this.maxTotalWeightFilterEmpty : this.maxTotalWeightFilter,
                this.minTotalWeightFilter == null ? this.minTotalWeightFilterEmpty : this.minTotalWeightFilter,
                this.deliveryProviderIdFilter,
                this.maxDispatchDateFilter === undefined
                    ? this.maxDispatchDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxDispatchDateFilter),
                this.minDispatchDateFilter === undefined
                    ? this.minDispatchDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minDispatchDateFilter),
                this.dispatchTimeFilter,
                this.maxDeliverToCustomerDateFilter === undefined
                    ? this.maxDeliverToCustomerDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxDeliverToCustomerDateFilter),
                this.minDeliverToCustomerDateFilter === undefined
                    ? this.minDeliverToCustomerDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minDeliverToCustomerDateFilter),
                this.deliverToCustomerTimeFilter,
                this.deliveryNotesFilter,
                this.customerAcknowledgedFilter,
                this.customerSignatureFilter,
                this.maxCateringDateFilter === undefined
                    ? this.maxCateringDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxCateringDateFilter),
                this.minCateringDateFilter === undefined
                    ? this.minCateringDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minCateringDateFilter),
                this.cateringTimeFilter,
                this.maxDeliveryDateFilter === undefined
                    ? this.maxDeliveryDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxDeliveryDateFilter),
                this.minDeliveryDateFilter === undefined
                    ? this.minDeliveryDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minDeliveryDateFilter),
                this.deliveryTimeFilter,
                this.maxDineInDateFilter === undefined
                    ? this.maxDineInDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxDineInDateFilter),
                this.minDineInDateFilter === undefined
                    ? this.minDineInDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minDineInDateFilter),
                this.dineInTimeFilter,
                this.includedChildrenFilter,
                this.isAsapFilter,
                this.isPickupCateringFilter,
                this.maxNumberOfGuestsFilter == null ? this.maxNumberOfGuestsFilterEmpty : this.maxNumberOfGuestsFilter,
                this.minNumberOfGuestsFilter == null ? this.minNumberOfGuestsFilterEmpty : this.minNumberOfGuestsFilter,
                this.maxPickupDateFilter === undefined
                    ? this.maxPickupDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxPickupDateFilter),
                this.minPickupDateFilter === undefined
                    ? this.minPickupDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minPickupDateFilter),
                this.pickupTimeFilter,
                this.employeeNameFilter,
                this.orderInvoiceNumberFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.trackingNumberFilter = '';
        this.maxTotalWeightFilter = this.maxTotalWeightFilterEmpty;
        this.minTotalWeightFilter = this.maxTotalWeightFilterEmpty;
        this.deliveryProviderIdFilter = '';
        this.maxDispatchDateFilter = undefined;
        this.minDispatchDateFilter = undefined;
        this.dispatchTimeFilter = '';
        this.maxDeliverToCustomerDateFilter = undefined;
        this.minDeliverToCustomerDateFilter = undefined;
        this.deliverToCustomerTimeFilter = '';
        this.deliveryNotesFilter = '';
        this.customerAcknowledgedFilter = -1;
        this.customerSignatureFilter = '';
        this.maxCateringDateFilter = undefined;
        this.minCateringDateFilter = undefined;
        this.cateringTimeFilter = '';
        this.maxDeliveryDateFilter = undefined;
        this.minDeliveryDateFilter = undefined;
        this.deliveryTimeFilter = '';
        this.maxDineInDateFilter = undefined;
        this.minDineInDateFilter = undefined;
        this.dineInTimeFilter = '';
        this.includedChildrenFilter = -1;
        this.isAsapFilter = -1;
        this.isPickupCateringFilter = -1;
        this.maxNumberOfGuestsFilter = this.maxNumberOfGuestsFilterEmpty;
        this.minNumberOfGuestsFilter = this.maxNumberOfGuestsFilterEmpty;
        this.maxPickupDateFilter = undefined;
        this.minPickupDateFilter = undefined;
        this.pickupTimeFilter = '';
        this.employeeNameFilter = '';
        this.orderInvoiceNumberFilter = '';

        this.getOrderDeliveryInfos();
    }
}
