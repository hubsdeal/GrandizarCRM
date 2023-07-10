import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductsServiceProxy, ProductDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';
import { CreateOrEditProductModalComponent } from '../create-or-edit-product-modal.component';
import { ViewProductModalComponent } from '../view-product-modal.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'app-product-libraries',
    templateUrl: './product-libraries.component.html',
    styleUrls: ['./product-libraries.component.css'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductLibrariesComponent extends AppComponentBase {
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
    maxSalesPriceFilter: number;
    maxSalesPriceFilterEmpty: number;
    minSalesPriceFilter: number;
    minSalesPriceFilterEmpty: number;
    maxPriceDiscountPercentageFilter: number;
    maxPriceDiscountPercentageFilterEmpty: number;
    minPriceDiscountPercentageFilter: number;
    minPriceDiscountPercentageFilterEmpty: number;
    callForPriceFilter = -1;
    maxUnitPriceFilter: number;
    maxUnitPriceFilterEmpty: number;
    minUnitPriceFilter: number;
    minUnitPriceFilterEmpty: number;
    maxMeasureAmountFilter: number;
    maxMeasureAmountFilterEmpty: number;
    minMeasureAmountFilter: number;
    minMeasureAmountFilterEmpty: number;
    isTaxExemptFilter = -1;
    maxStockQuantityFilter: number;
    maxStockQuantityFilterEmpty: number;
    minStockQuantityFilter: number;
    minStockQuantityFilterEmpty: number;
    isDisplayStockQuantityFilter = -1;
    isPublishedFilter = -1;
    isTemplateFilter = -1;
    isServiceFilter = -1;
    isPackageProductFilter = -1;
    isWholeSaleProductFilter = -1;
    internalNotesFilter = '';
    iconFilter = '';
    productCategoryNameFilter = '';
    mediaLibraryNameFilter = '';
    measurementUnitNameFilter = '';
    currencyNameFilter = '';
    ratingLikeNameFilter = '';

    productManufacturerSkuFilter: string;
    byOrderOnlyFilter: number;
    maxScoreFilter: number;
    minScoreFilter: number;
    contactFullNameFilter: string;
    storeNameFilter: string;

    minPriceFilter: number;
    maxpriceFilter: number;
    @Input() productCategoryidFilter: number;
    currencyIdFilter: number;
    measurementUnitIdFilter: number;
    ratingLikeIdFilter: number;

    productCategoryOptions: any;
    currencyoptions: any;
    measurementUnitOptions: any;
    ratingLikeOptions: any;

    published: number;
    unpublished: number;

    selectedAll: boolean = false;
    selectedInput: number[] = [];

    productTagNameFilter: string = '';

    myStoreOptions: any;

    myStoreEmployeeId: number;

    storeIdFilter: number;

    @Input() businessIdFilter: number;
    @Input() employeeId: number;

    maxPriceDiscountAmountFilter: number;
    minPriceDiscountAmountFilter: number;

    skipCount: number;
    maxResultCount: number = 10;
    publishedCount: number = 0;
    unpublishedCount: number = 0;

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
        this.getProducts();
    }

    getProducts(event?: LazyLoadEvent) {
        // if (this.primengTableHelper.shouldResetPaging(event)) {
        //     this.paginator.changePage(0);
        //     if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
        //         return;
        //     }
        // }

        this.primengTableHelper.showLoadingIndicator();

        this._productsServiceProxy
            .getAllProductsBySp(
                this.minPriceFilter,
                this.maxpriceFilter,
                this.productCategoryidFilter != null ? this.productCategoryidFilter : undefined,
                this.currencyIdFilter,
                this.measurementUnitIdFilter,
                this.ratingLikeIdFilter,
                1,
                this.employeeId != null ? this.employeeId : undefined,
                this.businessIdFilter != null ? this.businessIdFilter : undefined,
                this.storeIdFilter,
                this.productTagNameFilter,
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
                this.maxSalesPriceFilter == null ? this.maxSalesPriceFilterEmpty : this.maxSalesPriceFilter,
                this.minSalesPriceFilter == null ? this.minSalesPriceFilterEmpty : this.minSalesPriceFilter,
                this.maxPriceDiscountPercentageFilter == null ? this.maxPriceDiscountPercentageFilterEmpty : this.maxPriceDiscountPercentageFilter,
                this.minPriceDiscountPercentageFilter == null ? this.minPriceDiscountPercentageFilterEmpty : this.minPriceDiscountPercentageFilter,
                this.callForPriceFilter,
                this.maxUnitPriceFilter == null ? this.maxUnitPriceFilterEmpty : this.maxUnitPriceFilter,
                this.minUnitPriceFilter == null ? this.minUnitPriceFilterEmpty : this.minUnitPriceFilter,
                this.maxMeasureAmountFilter == null ? this.maxMeasureAmountFilterEmpty : this.maxMeasureAmountFilter,
                this.minMeasureAmountFilter == null ? this.minMeasureAmountFilterEmpty : this.minMeasureAmountFilter,
                this.isTaxExemptFilter,
                this.maxStockQuantityFilter == null ? this.maxStockQuantityFilterEmpty : this.maxStockQuantityFilter,
                this.minStockQuantityFilter == null ? this.minStockQuantityFilterEmpty : this.minStockQuantityFilter,
                this.isDisplayStockQuantityFilter,
                this.isPublishedFilter,
                this.isPackageProductFilter,
                this.internalNotesFilter,
                this.maxPriceDiscountAmountFilter,
                this.minPriceDiscountAmountFilter,
                this.isServiceFilter,
                this.isWholeSaleProductFilter,
                this.productManufacturerSkuFilter,
                this.byOrderOnlyFilter,
                this.maxScoreFilter,
                this.minScoreFilter,
                this.productCategoryNameFilter,
                this.mediaLibraryNameFilter,
                this.measurementUnitNameFilter,
                this.currencyNameFilter,
                this.ratingLikeNameFilter,
                this.contactFullNameFilter,
                this.storeNameFilter,
                '',
                this.skipCount,
                this.maxResultCount
                // this.primengTableHelper.getSorting(this.dataTable),
                // this.primengTableHelper.getSkipCount(this.paginator, event),
                // this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.products;
                this.publishedCount = result.published;
                this.unpublishedCount = result.unPublished;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    paginate(event: any) {
        this.skipCount = event.first;
        this.maxResultCount = event.rows;
        this.getProducts();
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createProduct(): void {
        this.createOrEditProductModal.isTemplate = true
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
        this.maxPriceDiscountPercentageFilter = this.maxPriceDiscountPercentageFilterEmpty;
        this.minPriceDiscountPercentageFilter = this.maxPriceDiscountPercentageFilterEmpty;
        this.callForPriceFilter = -1;
        this.maxUnitPriceFilter = this.maxUnitPriceFilterEmpty;
        this.minUnitPriceFilter = this.maxUnitPriceFilterEmpty;
        this.isTaxExemptFilter = -1;
        this.maxStockQuantityFilter = this.maxStockQuantityFilterEmpty;
        this.minStockQuantityFilter = this.maxStockQuantityFilterEmpty;
        this.isDisplayStockQuantityFilter = -1;
        this.isPublishedFilter = -1;
        this.isPackageProductFilter = -1;
        this.internalNotesFilter = '';
        this.isTemplateFilter = -1;
        this.isServiceFilter = -1;
        this.isWholeSaleProductFilter = -1;
        this.productManufacturerSkuFilter = '';
        this.byOrderOnlyFilter = -1;
        this.productCategoryNameFilter = '';
        this.mediaLibraryNameFilter = '';
        this.measurementUnitNameFilter = '';
        this.currencyNameFilter = '';
        this.ratingLikeNameFilter = '';
        this.contactFullNameFilter = '';
        this.storeNameFilter = '';

        this.getProducts();
    }

    onChangesSelectAll() {
        for (var i = 0; i < this.primengTableHelper.records.length; i++) {
            this.primengTableHelper.records[i].selected = this.selectedAll;
        }
    }

    checkIfAllSelected() {
        this.selectedAll = this.primengTableHelper.records.every(function (item: any) {
            return item.selected == true;
        })
    }

    refreshCheckboxReloadList() {
        this.selectedAll = false;
        for (var i = 0; i < this.primengTableHelper.records.length; i++) {
            this.primengTableHelper.records[i].selected = false;
        }
        this.reloadPage();
    }

}

