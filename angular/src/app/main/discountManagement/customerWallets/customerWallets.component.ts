import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerWalletsServiceProxy, CustomerWalletDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditCustomerWalletModalComponent } from './create-or-edit-customerWallet-modal.component';

import { ViewCustomerWalletModalComponent } from './view-customerWallet-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './customerWallets.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class CustomerWalletsComponent extends AppComponentBase {
    @ViewChild('createOrEditCustomerWalletModal', { static: true })
    createOrEditCustomerWalletModal: CreateOrEditCustomerWalletModalComponent;
    @ViewChild('viewCustomerWalletModal', { static: true }) viewCustomerWalletModal: ViewCustomerWalletModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxWalletOpeningDateFilter: DateTime;
    minWalletOpeningDateFilter: DateTime;
    maxBalanceDateFilter: DateTime;
    minBalanceDateFilter: DateTime;
    maxBalanceAmountFilter: number;
    maxBalanceAmountFilterEmpty: number;
    minBalanceAmountFilter: number;
    minBalanceAmountFilterEmpty: number;
    contactFullNameFilter = '';
    userNameFilter = '';
    currencyNameFilter = '';

    constructor(
        injector: Injector,
        private _customerWalletsServiceProxy: CustomerWalletsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getCustomerWallets(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._customerWalletsServiceProxy
            .getAll(
                this.filterText,
                this.maxWalletOpeningDateFilter === undefined
                    ? this.maxWalletOpeningDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxWalletOpeningDateFilter),
                this.minWalletOpeningDateFilter === undefined
                    ? this.minWalletOpeningDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minWalletOpeningDateFilter),
                this.maxBalanceDateFilter === undefined
                    ? this.maxBalanceDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxBalanceDateFilter),
                this.minBalanceDateFilter === undefined
                    ? this.minBalanceDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minBalanceDateFilter),
                this.maxBalanceAmountFilter == null ? this.maxBalanceAmountFilterEmpty : this.maxBalanceAmountFilter,
                this.minBalanceAmountFilter == null ? this.minBalanceAmountFilterEmpty : this.minBalanceAmountFilter,
                this.contactFullNameFilter,
                this.userNameFilter,
                this.currencyNameFilter,
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

    createCustomerWallet(): void {
        this.createOrEditCustomerWalletModal.show();
    }

    deleteCustomerWallet(customerWallet: CustomerWalletDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._customerWalletsServiceProxy.delete(customerWallet.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._customerWalletsServiceProxy
            .getCustomerWalletsToExcel(
                this.filterText,
                this.maxWalletOpeningDateFilter === undefined
                    ? this.maxWalletOpeningDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxWalletOpeningDateFilter),
                this.minWalletOpeningDateFilter === undefined
                    ? this.minWalletOpeningDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minWalletOpeningDateFilter),
                this.maxBalanceDateFilter === undefined
                    ? this.maxBalanceDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxBalanceDateFilter),
                this.minBalanceDateFilter === undefined
                    ? this.minBalanceDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minBalanceDateFilter),
                this.maxBalanceAmountFilter == null ? this.maxBalanceAmountFilterEmpty : this.maxBalanceAmountFilter,
                this.minBalanceAmountFilter == null ? this.minBalanceAmountFilterEmpty : this.minBalanceAmountFilter,
                this.contactFullNameFilter,
                this.userNameFilter,
                this.currencyNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxWalletOpeningDateFilter = undefined;
        this.minWalletOpeningDateFilter = undefined;
        this.maxBalanceDateFilter = undefined;
        this.minBalanceDateFilter = undefined;
        this.maxBalanceAmountFilter = this.maxBalanceAmountFilterEmpty;
        this.minBalanceAmountFilter = this.maxBalanceAmountFilterEmpty;
        this.contactFullNameFilter = '';
        this.userNameFilter = '';
        this.currencyNameFilter = '';

        this.getCustomerWallets();
    }
}
