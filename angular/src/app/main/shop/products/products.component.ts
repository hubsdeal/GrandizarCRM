import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductsServiceProxy, ProductDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductModalComponent } from './create-or-edit-product-modal.component';

import { ViewProductModalComponent } from './view-product-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './products.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductsComponent extends AppComponentBase {
    @ViewChild('createOrEditProductModal', { static: true })
    createOrEditProductModal: CreateOrEditProductModalComponent;
    @ViewChild('viewProductModal', { static: true }) viewProductModal: ViewProductModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    shortDescriptionFilter = '';
    descriptionFilter = '';
    skuFilter = '';
    urlFilter = '';
    seoTitleFilter = '';
    metaKeywordsFilter = '';
    metaDescriptionFilter = '';
    maxRegularPriceFilter: number;
    maxRegularPriceFilterEmpty: number;
    minRegularPriceFilter: number;
    minRegularPriceFilterEmpty: number;
    maxSalePriceFilter: number;
    maxSalePriceFilterEmpty: number;
    minSalePriceFilter: number;
    minSalePriceFilterEmpty: number;
    maxPriceDiscountPercentageFilter: number;
    maxPriceDiscountPercentageFilterEmpty: number;
    minPriceDiscountPercentageFilter: number;
    minPriceDiscountPercentageFilterEmpty: number;
    callForPriceFilter = -1;
    maxUnitPriceFilter: number;
    maxUnitPriceFilterEmpty: number;
    minUnitPriceFilter: number;
    minUnitPriceFilterEmpty: number;
    maxMeasurementAmountFilter: number;
    maxMeasurementAmountFilterEmpty: number;
    minMeasurementAmountFilter: number;
    minMeasurementAmountFilterEmpty: number;
    isTaxExemptFilter = -1;
    maxStockQuantityFilter: number;
    maxStockQuantityFilterEmpty: number;
    minStockQuantityFilter: number;
    minStockQuantityFilterEmpty: number;
    isDisplayStockQuantityFilter = -1;
    isPublishedFilter = -1;
    isPackageProductFilter = -1;
    internalNotesFilter = '';
    isTemplateFilter = -1;
    maxPriceDiscountAmountFilter: number;
    maxPriceDiscountAmountFilterEmpty: number;
    minPriceDiscountAmountFilter: number;
    minPriceDiscountAmountFilterEmpty: number;
    isServiceFilter = -1;
    isWholeSaleProductFilter = -1;
    productManufacturerSkuFilter = '';
    byOrderOnlyFilter = -1;
    maxScoreFilter: number;
    maxScoreFilterEmpty: number;
    minScoreFilter: number;
    minScoreFilterEmpty: number;
    productCategoryNameFilter = '';
    mediaLibraryNameFilter = '';
    measurementUnitNameFilter = '';
    currencyNameFilter = '';
    ratingLikeNameFilter = '';

    constructor(
        injector: Injector,
        private _productsServiceProxy: ProductsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProducts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productsServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.shortDescriptionFilter,
                this.descriptionFilter,
                this.skuFilter,
                this.urlFilter,
                this.seoTitleFilter,
                this.metaKeywordsFilter,
                this.metaDescriptionFilter,
                this.maxRegularPriceFilter == null ? this.maxRegularPriceFilterEmpty : this.maxRegularPriceFilter,
                this.minRegularPriceFilter == null ? this.minRegularPriceFilterEmpty : this.minRegularPriceFilter,
                this.maxSalePriceFilter == null ? this.maxSalePriceFilterEmpty : this.maxSalePriceFilter,
                this.minSalePriceFilter == null ? this.minSalePriceFilterEmpty : this.minSalePriceFilter,
                this.maxPriceDiscountPercentageFilter == null
                    ? this.maxPriceDiscountPercentageFilterEmpty
                    : this.maxPriceDiscountPercentageFilter,
                this.minPriceDiscountPercentageFilter == null
                    ? this.minPriceDiscountPercentageFilterEmpty
                    : this.minPriceDiscountPercentageFilter,
                this.callForPriceFilter,
                this.maxUnitPriceFilter == null ? this.maxUnitPriceFilterEmpty : this.maxUnitPriceFilter,
                this.minUnitPriceFilter == null ? this.minUnitPriceFilterEmpty : this.minUnitPriceFilter,
                this.maxMeasurementAmountFilter == null
                    ? this.maxMeasurementAmountFilterEmpty
                    : this.maxMeasurementAmountFilter,
                this.minMeasurementAmountFilter == null
                    ? this.minMeasurementAmountFilterEmpty
                    : this.minMeasurementAmountFilter,
                this.isTaxExemptFilter,
                this.maxStockQuantityFilter == null ? this.maxStockQuantityFilterEmpty : this.maxStockQuantityFilter,
                this.minStockQuantityFilter == null ? this.minStockQuantityFilterEmpty : this.minStockQuantityFilter,
                this.isDisplayStockQuantityFilter,
                this.isPublishedFilter,
                this.isPackageProductFilter,
                this.internalNotesFilter,
                this.isTemplateFilter,
                this.maxPriceDiscountAmountFilter == null
                    ? this.maxPriceDiscountAmountFilterEmpty
                    : this.maxPriceDiscountAmountFilter,
                this.minPriceDiscountAmountFilter == null
                    ? this.minPriceDiscountAmountFilterEmpty
                    : this.minPriceDiscountAmountFilter,
                this.isServiceFilter,
                this.isWholeSaleProductFilter,
                this.productManufacturerSkuFilter,
                this.byOrderOnlyFilter,
                this.maxScoreFilter == null ? this.maxScoreFilterEmpty : this.maxScoreFilter,
                this.minScoreFilter == null ? this.minScoreFilterEmpty : this.minScoreFilter,
                this.productCategoryNameFilter,
                this.mediaLibraryNameFilter,
                this.measurementUnitNameFilter,
                this.currencyNameFilter,
                this.ratingLikeNameFilter,
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

    createProduct(): void {
        this.createOrEditProductModal.show();
    }

    deleteProduct(product: ProductDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productsServiceProxy.delete(product.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productsServiceProxy
            .getProductsToExcel(
                this.filterText,
                this.nameFilter,
                this.shortDescriptionFilter,
                this.descriptionFilter,
                this.skuFilter,
                this.urlFilter,
                this.seoTitleFilter,
                this.metaKeywordsFilter,
                this.metaDescriptionFilter,
                this.maxRegularPriceFilter == null ? this.maxRegularPriceFilterEmpty : this.maxRegularPriceFilter,
                this.minRegularPriceFilter == null ? this.minRegularPriceFilterEmpty : this.minRegularPriceFilter,
                this.maxSalePriceFilter == null ? this.maxSalePriceFilterEmpty : this.maxSalePriceFilter,
                this.minSalePriceFilter == null ? this.minSalePriceFilterEmpty : this.minSalePriceFilter,
                this.maxPriceDiscountPercentageFilter == null
                    ? this.maxPriceDiscountPercentageFilterEmpty
                    : this.maxPriceDiscountPercentageFilter,
                this.minPriceDiscountPercentageFilter == null
                    ? this.minPriceDiscountPercentageFilterEmpty
                    : this.minPriceDiscountPercentageFilter,
                this.callForPriceFilter,
                this.maxUnitPriceFilter == null ? this.maxUnitPriceFilterEmpty : this.maxUnitPriceFilter,
                this.minUnitPriceFilter == null ? this.minUnitPriceFilterEmpty : this.minUnitPriceFilter,
                this.maxMeasurementAmountFilter == null
                    ? this.maxMeasurementAmountFilterEmpty
                    : this.maxMeasurementAmountFilter,
                this.minMeasurementAmountFilter == null
                    ? this.minMeasurementAmountFilterEmpty
                    : this.minMeasurementAmountFilter,
                this.isTaxExemptFilter,
                this.maxStockQuantityFilter == null ? this.maxStockQuantityFilterEmpty : this.maxStockQuantityFilter,
                this.minStockQuantityFilter == null ? this.minStockQuantityFilterEmpty : this.minStockQuantityFilter,
                this.isDisplayStockQuantityFilter,
                this.isPublishedFilter,
                this.isPackageProductFilter,
                this.internalNotesFilter,
                this.isTemplateFilter,
                this.maxPriceDiscountAmountFilter == null
                    ? this.maxPriceDiscountAmountFilterEmpty
                    : this.maxPriceDiscountAmountFilter,
                this.minPriceDiscountAmountFilter == null
                    ? this.minPriceDiscountAmountFilterEmpty
                    : this.minPriceDiscountAmountFilter,
                this.isServiceFilter,
                this.isWholeSaleProductFilter,
                this.productManufacturerSkuFilter,
                this.byOrderOnlyFilter,
                this.maxScoreFilter == null ? this.maxScoreFilterEmpty : this.maxScoreFilter,
                this.minScoreFilter == null ? this.minScoreFilterEmpty : this.minScoreFilter,
                this.productCategoryNameFilter,
                this.mediaLibraryNameFilter,
                this.measurementUnitNameFilter,
                this.currencyNameFilter,
                this.ratingLikeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.shortDescriptionFilter = '';
        this.descriptionFilter = '';
        this.skuFilter = '';
        this.urlFilter = '';
        this.seoTitleFilter = '';
        this.metaKeywordsFilter = '';
        this.metaDescriptionFilter = '';
        this.maxRegularPriceFilter = this.maxRegularPriceFilterEmpty;
        this.minRegularPriceFilter = this.maxRegularPriceFilterEmpty;
        this.maxSalePriceFilter = this.maxSalePriceFilterEmpty;
        this.minSalePriceFilter = this.maxSalePriceFilterEmpty;
        this.maxPriceDiscountPercentageFilter = this.maxPriceDiscountPercentageFilterEmpty;
        this.minPriceDiscountPercentageFilter = this.maxPriceDiscountPercentageFilterEmpty;
        this.callForPriceFilter = -1;
        this.maxUnitPriceFilter = this.maxUnitPriceFilterEmpty;
        this.minUnitPriceFilter = this.maxUnitPriceFilterEmpty;
        this.maxMeasurementAmountFilter = this.maxMeasurementAmountFilterEmpty;
        this.minMeasurementAmountFilter = this.maxMeasurementAmountFilterEmpty;
        this.isTaxExemptFilter = -1;
        this.maxStockQuantityFilter = this.maxStockQuantityFilterEmpty;
        this.minStockQuantityFilter = this.maxStockQuantityFilterEmpty;
        this.isDisplayStockQuantityFilter = -1;
        this.isPublishedFilter = -1;
        this.isPackageProductFilter = -1;
        this.internalNotesFilter = '';
        this.isTemplateFilter = -1;
        this.maxPriceDiscountAmountFilter = this.maxPriceDiscountAmountFilterEmpty;
        this.minPriceDiscountAmountFilter = this.maxPriceDiscountAmountFilterEmpty;
        this.isServiceFilter = -1;
        this.isWholeSaleProductFilter = -1;
        this.productManufacturerSkuFilter = '';
        this.byOrderOnlyFilter = -1;
        this.maxScoreFilter = this.maxScoreFilterEmpty;
        this.minScoreFilter = this.maxScoreFilterEmpty;
        this.productCategoryNameFilter = '';
        this.mediaLibraryNameFilter = '';
        this.measurementUnitNameFilter = '';
        this.currencyNameFilter = '';
        this.ratingLikeNameFilter = '';

        this.getProducts();
    }
}
