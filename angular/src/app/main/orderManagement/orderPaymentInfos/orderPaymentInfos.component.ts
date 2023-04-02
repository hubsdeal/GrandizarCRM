import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderPaymentInfosServiceProxy, OrderPaymentInfoDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditOrderPaymentInfoModalComponent } from './create-or-edit-orderPaymentInfo-modal.component';

import { ViewOrderPaymentInfoModalComponent } from './view-orderPaymentInfo-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './orderPaymentInfos.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class OrderPaymentInfosComponent extends AppComponentBase {
    @ViewChild('createOrEditOrderPaymentInfoModal', { static: true })
    createOrEditOrderPaymentInfoModal: CreateOrEditOrderPaymentInfoModalComponent;
    @ViewChild('viewOrderPaymentInfoModal', { static: true })
    viewOrderPaymentInfoModal: ViewOrderPaymentInfoModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    paymentSplitFilter = -1;
    maxDueAmountFilter: number;
    maxDueAmountFilterEmpty: number;
    minDueAmountFilter: number;
    minDueAmountFilterEmpty: number;
    maxPaySplitAmountFilter: number;
    maxPaySplitAmountFilterEmpty: number;
    minPaySplitAmountFilter: number;
    minPaySplitAmountFilterEmpty: number;
    billingAddressFilter = '';
    billingCityFilter = '';
    billingStateFilter = '';
    billingZipCodeFilter = '';
    saveCreditCardNumberFilter = -1;
    maskedCreditCardNumberFilter = '';
    cardNameFilter = '';
    cardNumberFilter = '';
    cardCvvFilter = '';
    cardExpirationMonthFilter = '';
    cardExpirationYearFilter = '';
    authorizationTransactionNumberFilter = '';
    authorizationTransactionCodeFilter = '';
    authrorizationTransactionResultFilter = '';
    customerIpAddressFilter = '';
    customerDeviceInfoFilter = '';
    maxPaidDateFilter: DateTime;
    minPaidDateFilter: DateTime;
    paidTimeFilter = '';
    orderInvoiceNumberFilter = '';
    currencyNameFilter = '';
    paymentTypeNameFilter = '';

    constructor(
        injector: Injector,
        private _orderPaymentInfosServiceProxy: OrderPaymentInfosServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getOrderPaymentInfos(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._orderPaymentInfosServiceProxy
            .getAll(
                this.filterText,
                this.paymentSplitFilter,
                this.maxDueAmountFilter == null ? this.maxDueAmountFilterEmpty : this.maxDueAmountFilter,
                this.minDueAmountFilter == null ? this.minDueAmountFilterEmpty : this.minDueAmountFilter,
                this.maxPaySplitAmountFilter == null ? this.maxPaySplitAmountFilterEmpty : this.maxPaySplitAmountFilter,
                this.minPaySplitAmountFilter == null ? this.minPaySplitAmountFilterEmpty : this.minPaySplitAmountFilter,
                this.billingAddressFilter,
                this.billingCityFilter,
                this.billingStateFilter,
                this.billingZipCodeFilter,
                this.saveCreditCardNumberFilter,
                this.maskedCreditCardNumberFilter,
                this.cardNameFilter,
                this.cardNumberFilter,
                this.cardCvvFilter,
                this.cardExpirationMonthFilter,
                this.cardExpirationYearFilter,
                this.authorizationTransactionNumberFilter,
                this.authorizationTransactionCodeFilter,
                this.authrorizationTransactionResultFilter,
                this.customerIpAddressFilter,
                this.customerDeviceInfoFilter,
                this.maxPaidDateFilter === undefined
                    ? this.maxPaidDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxPaidDateFilter),
                this.minPaidDateFilter === undefined
                    ? this.minPaidDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minPaidDateFilter),
                this.paidTimeFilter,
                this.orderInvoiceNumberFilter,
                this.currencyNameFilter,
                this.paymentTypeNameFilter,
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

    createOrderPaymentInfo(): void {
        this.createOrEditOrderPaymentInfoModal.show();
    }

    deleteOrderPaymentInfo(orderPaymentInfo: OrderPaymentInfoDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._orderPaymentInfosServiceProxy.delete(orderPaymentInfo.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._orderPaymentInfosServiceProxy
            .getOrderPaymentInfosToExcel(
                this.filterText,
                this.paymentSplitFilter,
                this.maxDueAmountFilter == null ? this.maxDueAmountFilterEmpty : this.maxDueAmountFilter,
                this.minDueAmountFilter == null ? this.minDueAmountFilterEmpty : this.minDueAmountFilter,
                this.maxPaySplitAmountFilter == null ? this.maxPaySplitAmountFilterEmpty : this.maxPaySplitAmountFilter,
                this.minPaySplitAmountFilter == null ? this.minPaySplitAmountFilterEmpty : this.minPaySplitAmountFilter,
                this.billingAddressFilter,
                this.billingCityFilter,
                this.billingStateFilter,
                this.billingZipCodeFilter,
                this.saveCreditCardNumberFilter,
                this.maskedCreditCardNumberFilter,
                this.cardNameFilter,
                this.cardNumberFilter,
                this.cardCvvFilter,
                this.cardExpirationMonthFilter,
                this.cardExpirationYearFilter,
                this.authorizationTransactionNumberFilter,
                this.authorizationTransactionCodeFilter,
                this.authrorizationTransactionResultFilter,
                this.customerIpAddressFilter,
                this.customerDeviceInfoFilter,
                this.maxPaidDateFilter === undefined
                    ? this.maxPaidDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxPaidDateFilter),
                this.minPaidDateFilter === undefined
                    ? this.minPaidDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minPaidDateFilter),
                this.paidTimeFilter,
                this.orderInvoiceNumberFilter,
                this.currencyNameFilter,
                this.paymentTypeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.paymentSplitFilter = -1;
        this.maxDueAmountFilter = this.maxDueAmountFilterEmpty;
        this.minDueAmountFilter = this.maxDueAmountFilterEmpty;
        this.maxPaySplitAmountFilter = this.maxPaySplitAmountFilterEmpty;
        this.minPaySplitAmountFilter = this.maxPaySplitAmountFilterEmpty;
        this.billingAddressFilter = '';
        this.billingCityFilter = '';
        this.billingStateFilter = '';
        this.billingZipCodeFilter = '';
        this.saveCreditCardNumberFilter = -1;
        this.maskedCreditCardNumberFilter = '';
        this.cardNameFilter = '';
        this.cardNumberFilter = '';
        this.cardCvvFilter = '';
        this.cardExpirationMonthFilter = '';
        this.cardExpirationYearFilter = '';
        this.authorizationTransactionNumberFilter = '';
        this.authorizationTransactionCodeFilter = '';
        this.authrorizationTransactionResultFilter = '';
        this.customerIpAddressFilter = '';
        this.customerDeviceInfoFilter = '';
        this.maxPaidDateFilter = undefined;
        this.minPaidDateFilter = undefined;
        this.paidTimeFilter = '';
        this.orderInvoiceNumberFilter = '';
        this.currencyNameFilter = '';
        this.paymentTypeNameFilter = '';

        this.getOrderPaymentInfos();
    }
}
