import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreBankAccountsServiceProxy, StoreBankAccountDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreBankAccountModalComponent } from './create-or-edit-storeBankAccount-modal.component';

import { ViewStoreBankAccountModalComponent } from './view-storeBankAccount-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeBankAccounts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreBankAccountsComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreBankAccountModal', { static: true })
    createOrEditStoreBankAccountModal: CreateOrEditStoreBankAccountModalComponent;
    @ViewChild('viewStoreBankAccountModal', { static: true })
    viewStoreBankAccountModal: ViewStoreBankAccountModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    accountNameFilter = '';
    accountNoFilter = '';
    bankNameFilter = '';
    routingNoFilter = '';
    bankAddressFilter = '';
    storeNameFilter = '';

    constructor(
        injector: Injector,
        private _storeBankAccountsServiceProxy: StoreBankAccountsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreBankAccounts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeBankAccountsServiceProxy
            .getAll(
                this.filterText,
                this.accountNameFilter,
                this.accountNoFilter,
                this.bankNameFilter,
                this.routingNoFilter,
                this.bankAddressFilter,
                this.storeNameFilter,
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

    createStoreBankAccount(): void {
        this.createOrEditStoreBankAccountModal.show();
    }

    deleteStoreBankAccount(storeBankAccount: StoreBankAccountDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeBankAccountsServiceProxy.delete(storeBankAccount.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeBankAccountsServiceProxy
            .getStoreBankAccountsToExcel(
                this.filterText,
                this.accountNameFilter,
                this.accountNoFilter,
                this.bankNameFilter,
                this.routingNoFilter,
                this.bankAddressFilter,
                this.storeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.accountNameFilter = '';
        this.accountNoFilter = '';
        this.bankNameFilter = '';
        this.routingNoFilter = '';
        this.bankAddressFilter = '';
        this.storeNameFilter = '';

        this.getStoreBankAccounts();
    }
}
