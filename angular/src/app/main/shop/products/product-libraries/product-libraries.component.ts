import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductsServiceProxy, ProductDto, ProductCategoriesServiceProxy } from '@shared/service-proxies/service-proxies';
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
    styleUrls: ['./product-libraries.component.scss'],
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
    productCategoryidFilter: number;

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


    constructor(
        injector: Injector,
        private _productsServiceProxy: ProductsServiceProxy,
        private _productCategoriesServiceProxy: ProductCategoriesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _router: Router
    ) {
        super(injector);
        this.loadAllDropdown();
    }

    getProducts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();
        this._productCategoriesServiceProxy.getAllProductCategoryForTemplateView(
            this.filterText,
            this.productCategoryidFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event),
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

    // createBulkProductCategoryAssign(): void {
    //     this.selectedInput = this.primengTableHelper.records.filter(x => x.selected == true).map(x => x.id);
    //     this.appBulkProductAssignToCategoryModal.selectedInput = this.selectedInput;
    //     this.appBulkProductAssignToCategoryModal.show();
    // }

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
                            this.notify.success(this.l('SuccessfullyDeleted'));
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

    // onAddToStore() {
    //     this.selectedInput = this.primengTableHelper.records.filter(x => x.selected == true).map(x => x.id);
    //     this.createOrEditStoreProductMapModal.selectedInput = this.selectedInput;
    //     this.createOrEditStoreProductMapModal.bulkProductTemplateAssignToStore = true;
    //     this.createOrEditStoreProductMapModal.show();
    // }

    onCategoryClick(id: number) {
        this.productCategoryId = this.productCategoryId == id ? 0 : id;
    }

    onProductCategoryChange(event: any) {
        if (event.value) {
            this.productCategoryidFilter = event.value.id
        } else {
            this.productCategoryidFilter = null;
        }
    }

}

