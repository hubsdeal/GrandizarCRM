import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ShoppingCartsServiceProxy, ShoppingCartDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditShoppingCartModalComponent } from './create-or-edit-shoppingCart-modal.component';

import { ViewShoppingCartModalComponent } from './view-shoppingCart-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './shoppingCarts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ShoppingCartsComponent extends AppComponentBase {
    @ViewChild('createOrEditShoppingCartModal', { static: true })
    createOrEditShoppingCartModal: CreateOrEditShoppingCartModalComponent;
    @ViewChild('viewShoppingCartModal', { static: true }) viewShoppingCartModal: ViewShoppingCartModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxQuantityFilter: number;
    maxQuantityFilterEmpty: number;
    minQuantityFilter: number;
    minQuantityFilterEmpty: number;
    maxUnitPriceFilter: number;
    maxUnitPriceFilterEmpty: number;
    minUnitPriceFilter: number;
    minUnitPriceFilterEmpty: number;
    maxTotalAmountFilter: number;
    maxTotalAmountFilterEmpty: number;
    minTotalAmountFilter: number;
    minTotalAmountFilterEmpty: number;
    maxUnitTotalPriceFilter: number;
    maxUnitTotalPriceFilterEmpty: number;
    minUnitTotalPriceFilter: number;
    minUnitTotalPriceFilterEmpty: number;
    maxUnitDiscountAmountFilter: number;
    maxUnitDiscountAmountFilterEmpty: number;
    minUnitDiscountAmountFilter: number;
    minUnitDiscountAmountFilterEmpty: number;
    contactFullNameFilter = '';
    orderInvoiceNumberFilter = '';
    storeNameFilter = '';
    productNameFilter = '';
    currencyNameFilter = '';

    constructor(
        injector: Injector,
        private _shoppingCartsServiceProxy: ShoppingCartsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getShoppingCarts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._shoppingCartsServiceProxy
            .getAll(
                this.filterText,
                this.maxQuantityFilter == null ? this.maxQuantityFilterEmpty : this.maxQuantityFilter,
                this.minQuantityFilter == null ? this.minQuantityFilterEmpty : this.minQuantityFilter,
                this.maxUnitPriceFilter == null ? this.maxUnitPriceFilterEmpty : this.maxUnitPriceFilter,
                this.minUnitPriceFilter == null ? this.minUnitPriceFilterEmpty : this.minUnitPriceFilter,
                this.maxTotalAmountFilter == null ? this.maxTotalAmountFilterEmpty : this.maxTotalAmountFilter,
                this.minTotalAmountFilter == null ? this.minTotalAmountFilterEmpty : this.minTotalAmountFilter,
                this.maxUnitTotalPriceFilter == null ? this.maxUnitTotalPriceFilterEmpty : this.maxUnitTotalPriceFilter,
                this.minUnitTotalPriceFilter == null ? this.minUnitTotalPriceFilterEmpty : this.minUnitTotalPriceFilter,
                this.maxUnitDiscountAmountFilter == null
                    ? this.maxUnitDiscountAmountFilterEmpty
                    : this.maxUnitDiscountAmountFilter,
                this.minUnitDiscountAmountFilter == null
                    ? this.minUnitDiscountAmountFilterEmpty
                    : this.minUnitDiscountAmountFilter,
                this.contactFullNameFilter,
                this.orderInvoiceNumberFilter,
                this.storeNameFilter,
                this.productNameFilter,
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

    createShoppingCart(): void {
        this.createOrEditShoppingCartModal.show();
    }

    deleteShoppingCart(shoppingCart: ShoppingCartDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._shoppingCartsServiceProxy.delete(shoppingCart.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._shoppingCartsServiceProxy
            .getShoppingCartsToExcel(
                this.filterText,
                this.maxQuantityFilter == null ? this.maxQuantityFilterEmpty : this.maxQuantityFilter,
                this.minQuantityFilter == null ? this.minQuantityFilterEmpty : this.minQuantityFilter,
                this.maxUnitPriceFilter == null ? this.maxUnitPriceFilterEmpty : this.maxUnitPriceFilter,
                this.minUnitPriceFilter == null ? this.minUnitPriceFilterEmpty : this.minUnitPriceFilter,
                this.maxTotalAmountFilter == null ? this.maxTotalAmountFilterEmpty : this.maxTotalAmountFilter,
                this.minTotalAmountFilter == null ? this.minTotalAmountFilterEmpty : this.minTotalAmountFilter,
                this.maxUnitTotalPriceFilter == null ? this.maxUnitTotalPriceFilterEmpty : this.maxUnitTotalPriceFilter,
                this.minUnitTotalPriceFilter == null ? this.minUnitTotalPriceFilterEmpty : this.minUnitTotalPriceFilter,
                this.maxUnitDiscountAmountFilter == null
                    ? this.maxUnitDiscountAmountFilterEmpty
                    : this.maxUnitDiscountAmountFilter,
                this.minUnitDiscountAmountFilter == null
                    ? this.minUnitDiscountAmountFilterEmpty
                    : this.minUnitDiscountAmountFilter,
                this.contactFullNameFilter,
                this.orderInvoiceNumberFilter,
                this.storeNameFilter,
                this.productNameFilter,
                this.currencyNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxQuantityFilter = this.maxQuantityFilterEmpty;
        this.minQuantityFilter = this.maxQuantityFilterEmpty;
        this.maxUnitPriceFilter = this.maxUnitPriceFilterEmpty;
        this.minUnitPriceFilter = this.maxUnitPriceFilterEmpty;
        this.maxTotalAmountFilter = this.maxTotalAmountFilterEmpty;
        this.minTotalAmountFilter = this.maxTotalAmountFilterEmpty;
        this.maxUnitTotalPriceFilter = this.maxUnitTotalPriceFilterEmpty;
        this.minUnitTotalPriceFilter = this.maxUnitTotalPriceFilterEmpty;
        this.maxUnitDiscountAmountFilter = this.maxUnitDiscountAmountFilterEmpty;
        this.minUnitDiscountAmountFilter = this.maxUnitDiscountAmountFilterEmpty;
        this.contactFullNameFilter = '';
        this.orderInvoiceNumberFilter = '';
        this.storeNameFilter = '';
        this.productNameFilter = '';
        this.currencyNameFilter = '';

        this.getShoppingCarts();
    }
}
