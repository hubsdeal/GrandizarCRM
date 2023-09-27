import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { CustomerWalletTransactionsServiceProxy, CustomerWalletTransactionDto , WalletTransactionType } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditCustomerWalletTransactionModalComponent } from './create-or-edit-customerWalletTransaction-modal.component';

import { ViewCustomerWalletTransactionModalComponent } from './view-customerWalletTransaction-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './customerWalletTransactions.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class CustomerWalletTransactionsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditCustomerWalletTransactionModal', { static: true }) createOrEditCustomerWalletTransactionModal: CreateOrEditCustomerWalletTransactionModalComponent;
    @ViewChild('viewCustomerWalletTransactionModal', { static: true }) viewCustomerWalletTransactionModal: ViewCustomerWalletTransactionModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxBalanceBeforeTransactionFilter : number;
		maxBalanceBeforeTransactionFilterEmpty : number;
		minBalanceBeforeTransactionFilter : number;
		minBalanceBeforeTransactionFilterEmpty : number;
    addOrDeductFilter = -1;
    maxAmountFilter : number;
		maxAmountFilterEmpty : number;
		minAmountFilter : number;
		minAmountFilterEmpty : number;
    maxTransactionDateFilter : DateTime;
		minTransactionDateFilter : DateTime;
    transactionTimeFilter = '';
    maxCurrentBalanceAfterTransactionFilter : number;
		maxCurrentBalanceAfterTransactionFilterEmpty : number;
		minCurrentBalanceAfterTransactionFilter : number;
		minCurrentBalanceAfterTransactionFilterEmpty : number;
    maxCustomerWalletIdFilter : number;
		maxCustomerWalletIdFilterEmpty : number;
		minCustomerWalletIdFilter : number;
		minCustomerWalletIdFilterEmpty : number;
    walletTransactionTypeIdFilter = -1;
        orderInvoiceNumberFilter = '';

    walletTransactionType = WalletTransactionType;





    constructor(
        injector: Injector,
        private _customerWalletTransactionsServiceProxy: CustomerWalletTransactionsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getCustomerWalletTransactions(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._customerWalletTransactionsServiceProxy.getAll(
            this.filterText,
            this.maxBalanceBeforeTransactionFilter == null ? this.maxBalanceBeforeTransactionFilterEmpty: this.maxBalanceBeforeTransactionFilter,
            this.minBalanceBeforeTransactionFilter == null ? this.minBalanceBeforeTransactionFilterEmpty: this.minBalanceBeforeTransactionFilter,
            this.addOrDeductFilter,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty: this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty: this.minAmountFilter,
            this.maxTransactionDateFilter === undefined ? this.maxTransactionDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxTransactionDateFilter),
            this.minTransactionDateFilter === undefined ? this.minTransactionDateFilter : this._dateTimeService.getStartOfDayForDate(this.minTransactionDateFilter),
            this.transactionTimeFilter,
            this.maxCurrentBalanceAfterTransactionFilter == null ? this.maxCurrentBalanceAfterTransactionFilterEmpty: this.maxCurrentBalanceAfterTransactionFilter,
            this.minCurrentBalanceAfterTransactionFilter == null ? this.minCurrentBalanceAfterTransactionFilterEmpty: this.minCurrentBalanceAfterTransactionFilter,
            this.maxCustomerWalletIdFilter == null ? this.maxCustomerWalletIdFilterEmpty: this.maxCustomerWalletIdFilter,
            this.minCustomerWalletIdFilter == null ? this.minCustomerWalletIdFilterEmpty: this.minCustomerWalletIdFilter,
            this.walletTransactionTypeIdFilter,
            this.orderInvoiceNumberFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createCustomerWalletTransaction(): void {
        this.createOrEditCustomerWalletTransactionModal.show();        
    }


    deleteCustomerWalletTransaction(customerWalletTransaction: CustomerWalletTransactionDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._customerWalletTransactionsServiceProxy.delete(customerWalletTransaction.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._customerWalletTransactionsServiceProxy.getCustomerWalletTransactionsToExcel(
        this.filterText,
            this.maxBalanceBeforeTransactionFilter == null ? this.maxBalanceBeforeTransactionFilterEmpty: this.maxBalanceBeforeTransactionFilter,
            this.minBalanceBeforeTransactionFilter == null ? this.minBalanceBeforeTransactionFilterEmpty: this.minBalanceBeforeTransactionFilter,
            this.addOrDeductFilter,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty: this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty: this.minAmountFilter,
            this.maxTransactionDateFilter === undefined ? this.maxTransactionDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxTransactionDateFilter),
            this.minTransactionDateFilter === undefined ? this.minTransactionDateFilter : this._dateTimeService.getStartOfDayForDate(this.minTransactionDateFilter),
            this.transactionTimeFilter,
            this.maxCurrentBalanceAfterTransactionFilter == null ? this.maxCurrentBalanceAfterTransactionFilterEmpty: this.maxCurrentBalanceAfterTransactionFilter,
            this.minCurrentBalanceAfterTransactionFilter == null ? this.minCurrentBalanceAfterTransactionFilterEmpty: this.minCurrentBalanceAfterTransactionFilter,
            this.maxCustomerWalletIdFilter == null ? this.maxCustomerWalletIdFilterEmpty: this.maxCustomerWalletIdFilter,
            this.minCustomerWalletIdFilter == null ? this.minCustomerWalletIdFilterEmpty: this.minCustomerWalletIdFilter,
            this.walletTransactionTypeIdFilter,
            this.orderInvoiceNumberFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.maxBalanceBeforeTransactionFilter = this.maxBalanceBeforeTransactionFilterEmpty;
		this.minBalanceBeforeTransactionFilter = this.maxBalanceBeforeTransactionFilterEmpty;
    this.addOrDeductFilter = -1;
    this.maxAmountFilter = this.maxAmountFilterEmpty;
		this.minAmountFilter = this.maxAmountFilterEmpty;
    this.maxTransactionDateFilter = undefined;
		this.minTransactionDateFilter = undefined;
    this.transactionTimeFilter = '';
    this.maxCurrentBalanceAfterTransactionFilter = this.maxCurrentBalanceAfterTransactionFilterEmpty;
		this.minCurrentBalanceAfterTransactionFilter = this.maxCurrentBalanceAfterTransactionFilterEmpty;
    this.maxCustomerWalletIdFilter = this.maxCustomerWalletIdFilterEmpty;
		this.minCustomerWalletIdFilter = this.maxCustomerWalletIdFilterEmpty;
    this.walletTransactionTypeIdFilter = -1;
		this.orderInvoiceNumberFilter = '';
					
        this.getCustomerWalletTransactions();
    }
}
