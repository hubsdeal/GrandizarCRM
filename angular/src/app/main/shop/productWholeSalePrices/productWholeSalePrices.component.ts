import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductWholeSalePricesServiceProxy, ProductWholeSalePriceDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductWholeSalePriceModalComponent } from './create-or-edit-productWholeSalePrice-modal.component';

import { ViewProductWholeSalePriceModalComponent } from './view-productWholeSalePrice-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './productWholeSalePrices.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductWholeSalePricesComponent extends AppComponentBase {
    @ViewChild('createOrEditProductWholeSalePriceModal', { static: true })
    createOrEditProductWholeSalePriceModal: CreateOrEditProductWholeSalePriceModalComponent;
    @ViewChild('viewProductWholeSalePriceModal', { static: true })
    viewProductWholeSalePriceModal: ViewProductWholeSalePriceModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxPriceFilter: number;
    maxPriceFilterEmpty: number;
    minPriceFilter: number;
    minPriceFilterEmpty: number;
    maxExactQuantityFilter: number;
    maxExactQuantityFilterEmpty: number;
    minExactQuantityFilter: number;
    minExactQuantityFilterEmpty: number;
    packageInfoFilter = '';
    maxPackageQuantityFilter: number;
    maxPackageQuantityFilterEmpty: number;
    minPackageQuantityFilter: number;
    minPackageQuantityFilterEmpty: number;
    wholeSaleSkuIdFilter = '';
    productNameFilter = '';
    productWholeSaleQuantityTypeNameFilter = '';
    measurementUnitNameFilter = '';
    currencyNameFilter = '';

    constructor(
        injector: Injector,
        private _productWholeSalePricesServiceProxy: ProductWholeSalePricesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductWholeSalePrices(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productWholeSalePricesServiceProxy
            .getAll(
                this.filterText,
                this.maxPriceFilter == null ? this.maxPriceFilterEmpty : this.maxPriceFilter,
                this.minPriceFilter == null ? this.minPriceFilterEmpty : this.minPriceFilter,
                this.maxExactQuantityFilter == null ? this.maxExactQuantityFilterEmpty : this.maxExactQuantityFilter,
                this.minExactQuantityFilter == null ? this.minExactQuantityFilterEmpty : this.minExactQuantityFilter,
                this.packageInfoFilter,
                this.maxPackageQuantityFilter == null
                    ? this.maxPackageQuantityFilterEmpty
                    : this.maxPackageQuantityFilter,
                this.minPackageQuantityFilter == null
                    ? this.minPackageQuantityFilterEmpty
                    : this.minPackageQuantityFilter,
                this.wholeSaleSkuIdFilter,
                this.productNameFilter,
                this.productWholeSaleQuantityTypeNameFilter,
                this.measurementUnitNameFilter,
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

    createProductWholeSalePrice(): void {
        this.createOrEditProductWholeSalePriceModal.show();
    }

    deleteProductWholeSalePrice(productWholeSalePrice: ProductWholeSalePriceDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productWholeSalePricesServiceProxy.delete(productWholeSalePrice.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productWholeSalePricesServiceProxy
            .getProductWholeSalePricesToExcel(
                this.filterText,
                this.maxPriceFilter == null ? this.maxPriceFilterEmpty : this.maxPriceFilter,
                this.minPriceFilter == null ? this.minPriceFilterEmpty : this.minPriceFilter,
                this.maxExactQuantityFilter == null ? this.maxExactQuantityFilterEmpty : this.maxExactQuantityFilter,
                this.minExactQuantityFilter == null ? this.minExactQuantityFilterEmpty : this.minExactQuantityFilter,
                this.packageInfoFilter,
                this.maxPackageQuantityFilter == null
                    ? this.maxPackageQuantityFilterEmpty
                    : this.maxPackageQuantityFilter,
                this.minPackageQuantityFilter == null
                    ? this.minPackageQuantityFilterEmpty
                    : this.minPackageQuantityFilter,
                this.wholeSaleSkuIdFilter,
                this.productNameFilter,
                this.productWholeSaleQuantityTypeNameFilter,
                this.measurementUnitNameFilter,
                this.currencyNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxPriceFilter = this.maxPriceFilterEmpty;
        this.minPriceFilter = this.maxPriceFilterEmpty;
        this.maxExactQuantityFilter = this.maxExactQuantityFilterEmpty;
        this.minExactQuantityFilter = this.maxExactQuantityFilterEmpty;
        this.packageInfoFilter = '';
        this.maxPackageQuantityFilter = this.maxPackageQuantityFilterEmpty;
        this.minPackageQuantityFilter = this.maxPackageQuantityFilterEmpty;
        this.wholeSaleSkuIdFilter = '';
        this.productNameFilter = '';
        this.productWholeSaleQuantityTypeNameFilter = '';
        this.measurementUnitNameFilter = '';
        this.currencyNameFilter = '';

        this.getProductWholeSalePrices();
    }
}
