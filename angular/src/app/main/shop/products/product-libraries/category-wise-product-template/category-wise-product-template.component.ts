import { Component, Injector, Input, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditProductModalComponent } from '../../create-or-edit-product-modal.component';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { CreateOrEditStoreProductMapModalComponent } from '@app/main/shop/storeProductMaps/create-or-edit-storeProductMap-modal.component';
import { ProductCategoriesServiceProxy, ProductsServiceProxy, StoresServiceProxy, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { ActivatedRoute, Router } from '@angular/router';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-category-wise-product-template',
  templateUrl: './category-wise-product-template.component.html',
  styleUrls: ['./category-wise-product-template.component.css']
})
export class CategoryWiseProductTemplateComponent extends AppComponentBase {

  @ViewChild('createOrEditProductModal', { static: true }) createOrEditProductModal: CreateOrEditProductModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  // @ViewChild('appBulkProductAssignToCategoryModal', { static: true }) appBulkProductAssignToCategoryModal: BulkProductAssignToCategoryModalComponent;
  @ViewChild('createOrEditStoreProductMapModal', { static: true }) createOrEditStoreProductMapModal: CreateOrEditStoreProductMapModalComponent;
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
  isPackageProductFilter = -1;
  internalNotesFilter = '';
  iconFilter = '';
  productCategoryNameFilter = '';
  mediaLibraryNameFilter = '';
  measurementUnitNameFilter = '';
  currencyNameFilter = '';
  ratingLikeNameFilter = '';

  minPriceFilter: number;
  maxpriceFilter: number;
  productCategoryidFilter: number;
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

  isExpand: boolean = false;
  productCategoryId: number;

  @Input() categoryId: number;

  storeId: number;


  constructor(
    injector: Injector,
    private _productsServiceProxy: ProductsServiceProxy,
    private _productCategoriesServiceProxy: ProductCategoriesServiceProxy,
    private _notifyService: NotifyService,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _fileDownloadService: FileDownloadService,
    private _router: Router,
    private _storesServiceProxy: StoresServiceProxy
  ) {
    super(injector);
    this.loadAllDropdown();
    this._activatedRoute.queryParams.subscribe(params => {
      let storeId = params['storeId'];
      if (storeId != null) {
        this.storeId = parseInt(storeId);
      }
    });
  }

  getProducts(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();
    
    this._productCategoriesServiceProxy.getAllProductsByCategoryForTemplateView(
      this.categoryId,
      this.filterText,
      this.primengTableHelper.getSorting(this.dataTable),
      this.primengTableHelper.getSkipCount(this.paginator, event),
      this.primengTableHelper.getMaxResultCount(this.paginator, event)
    ).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }

  loadAllDropdown(): void {
    this._productsServiceProxy.getAllProductCategoryForTableDropdown().subscribe(result => {
      this.productCategoryOptions = result;
    });

  }

  reloadPage(): void {
    this.paginator.changePage(this.paginator.getPage());
  }

  createProduct(): void {
    this.createOrEditProductModal.isTemplate = true;
    this.createOrEditProductModal.show();
  }

 
  editProduct(id: number) {
    this._router.navigate(['/app/main/shop/products/dashboard', id]);
  }


  deleteProduct(id: number): void {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._productsServiceProxy.delete(id)
            .subscribe(() => {
              this.reloadPage();
              this.notify.success(this.l('Successfully Deleted'));
            });
        }
      }
    );
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

  onAddToStore() {
    this.selectedInput = this.primengTableHelper.records.filter(x => x.selected == true).map(x => x.productId);
    this.createOrEditStoreProductMapModal.selectedInput = this.selectedInput;
    this.createOrEditStoreProductMapModal.bulkProductTemplateAssignToStore = true;
    this.createOrEditStoreProductMapModal.show();
  }

  onStoreAssign(id: number) {
    this.selectedInput = this.primengTableHelper.records.filter(x => x.selected == true).map(x => x.productId);
    this._storesServiceProxy.bulkProductAssignFromProductLibrary(id, this.selectedInput)
      .pipe(finalize(() => { }))
      .subscribe(() => {
        this._router.navigate(['/app/main/shop/stores/dashboard', id]);
        this.notify.info(this.l('SavedSuccessfully'));
      });
  }

}

