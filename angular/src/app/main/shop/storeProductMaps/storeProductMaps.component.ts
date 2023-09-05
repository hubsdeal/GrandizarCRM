import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreProductMapsServiceProxy, StoreProductMapDto, StoreProductCategoryMapsServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreProductMapModalComponent } from './create-or-edit-storeProductMap-modal.component';

import { ViewStoreProductMapModalComponent } from './view-storeProductMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'app-storeProductMaps',
    templateUrl: './storeProductMaps.component.html',
    styleUrls: ['./storeProductMap-product-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreProductMapsComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditStoreProductMapModal', { static: true })
    createOrEditStoreProductMapModal: CreateOrEditStoreProductMapModalComponent;
    @ViewChild('viewStoreProductMapModal', { static: true })
    viewStoreProductMapModal: ViewStoreProductMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    publishedFilter = -1;
    maxDisplaySequenceFilter: number;
    maxDisplaySequenceFilterEmpty: number;
    minDisplaySequenceFilter: number;
    minDisplaySequenceFilterEmpty: number;
    storeNameFilter = '';
    productNameFilter = '';


    selectedAll: boolean = false;
    selectedInput: number[] = [];

    showGridView: boolean;
    showListView: boolean;

    allCategories: any[] = [];

    @Input() storeId: number;
    @Input() productId: number;
    productCategoryIdFilter:number;

    productCategoryOptions: any;

    constructor(
        injector: Injector,
        private _storeProductMapsServiceProxy: StoreProductMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _router:Router,
        private _storeCategoryMapsServiceProxy: StoreProductCategoryMapsServiceProxy
    ) {
        super(injector);
    }

    getStoreProductMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeProductMapsServiceProxy.getAllByProductIdAndStoreId(
            this.productId != null ? this.productId : undefined,
            this.storeId != null ? this.storeId : undefined,
            this.productCategoryIdFilter != null ? this.productCategoryIdFilter : undefined,
            this.filterText,
            this.publishedFilter,
            this.maxDisplaySequenceFilter == null ? this.maxDisplaySequenceFilterEmpty : this.maxDisplaySequenceFilter,
            this.minDisplaySequenceFilter == null ? this.minDisplaySequenceFilterEmpty : this.minDisplaySequenceFilter,
            this.storeNameFilter,
            this.productNameFilter,
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

    createStoreProductMap(): void {
        this.createOrEditStoreProductMapModal.storeId = this.storeId;
        this.createOrEditStoreProductMapModal.show();
    }

    deleteStoreProductMap(storeProductMap: StoreProductMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeProductMapsServiceProxy.delete(storeProductMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeProductMapsServiceProxy
            .getStoreProductMapsToExcel(
                this.filterText,
                this.publishedFilter,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.storeNameFilter,
                this.productNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.publishedFilter = -1;
        this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.storeNameFilter = '';
        this.productNameFilter = '';

        this.getStoreProductMaps();
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

    onGridView() {
        this.showGridView = !this.showGridView;
        this.showListView = !this.showListView;
    }

    onListView() {
        this.showListView = !this.showListView;
        this.showGridView = !this.showGridView;
    }

    ngOnInit() {
        this._storeCategoryMapsServiceProxy.getAllCategoriesByStoreId(this.storeId).subscribe(result => {
            this.allCategories = result.items;
        });
    }

    deletestoreProductCategoryMap(id: number): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._storeCategoryMapsServiceProxy.delete(id)
                        .subscribe(() => {
                            this.ngOnInit();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }
    
  createStoreBulkProduct(): void {
    this._router.navigate(['/app/main/shop/productLibraries/'], { queryParams: { storeId: this.storeId } });
  }
}
