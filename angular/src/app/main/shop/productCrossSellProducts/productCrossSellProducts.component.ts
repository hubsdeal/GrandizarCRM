import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    ProductCrossSellProductsServiceProxy,
    ProductCrossSellProductDto,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductCrossSellProductModalComponent } from './create-or-edit-productCrossSellProduct-modal.component';

import { ViewProductCrossSellProductModalComponent } from './view-productCrossSellProduct-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './productCrossSellProducts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductCrossSellProductsComponent extends AppComponentBase {
    @ViewChild('createOrEditProductCrossSellProductModal', { static: true })
    createOrEditProductCrossSellProductModal: CreateOrEditProductCrossSellProductModalComponent;
    @ViewChild('viewProductCrossSellProductModal', { static: true })
    viewProductCrossSellProductModal: ViewProductCrossSellProductModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxCrossProductIdFilter: number;
    maxCrossProductIdFilterEmpty: number;
    minCrossProductIdFilter: number;
    minCrossProductIdFilterEmpty: number;
    maxCrossSellScoreFilter: number;
    maxCrossSellScoreFilterEmpty: number;
    minCrossSellScoreFilter: number;
    minCrossSellScoreFilterEmpty: number;
    productNameFilter = '';

    constructor(
        injector: Injector,
        private _productCrossSellProductsServiceProxy: ProductCrossSellProductsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductCrossSellProducts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productCrossSellProductsServiceProxy
            .getAll(
                this.filterText,
                this.maxCrossProductIdFilter == null ? this.maxCrossProductIdFilterEmpty : this.maxCrossProductIdFilter,
                this.minCrossProductIdFilter == null ? this.minCrossProductIdFilterEmpty : this.minCrossProductIdFilter,
                this.maxCrossSellScoreFilter == null ? this.maxCrossSellScoreFilterEmpty : this.maxCrossSellScoreFilter,
                this.minCrossSellScoreFilter == null ? this.minCrossSellScoreFilterEmpty : this.minCrossSellScoreFilter,
                this.productNameFilter,
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

    createProductCrossSellProduct(): void {
        this.createOrEditProductCrossSellProductModal.show();
    }

    deleteProductCrossSellProduct(productCrossSellProduct: ProductCrossSellProductDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productCrossSellProductsServiceProxy.delete(productCrossSellProduct.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productCrossSellProductsServiceProxy
            .getProductCrossSellProductsToExcel(
                this.filterText,
                this.maxCrossProductIdFilter == null ? this.maxCrossProductIdFilterEmpty : this.maxCrossProductIdFilter,
                this.minCrossProductIdFilter == null ? this.minCrossProductIdFilterEmpty : this.minCrossProductIdFilter,
                this.maxCrossSellScoreFilter == null ? this.maxCrossSellScoreFilterEmpty : this.maxCrossSellScoreFilter,
                this.minCrossSellScoreFilter == null ? this.minCrossSellScoreFilterEmpty : this.minCrossSellScoreFilter,
                this.productNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxCrossProductIdFilter = this.maxCrossProductIdFilterEmpty;
        this.minCrossProductIdFilter = this.maxCrossProductIdFilterEmpty;
        this.maxCrossSellScoreFilter = this.maxCrossSellScoreFilterEmpty;
        this.minCrossSellScoreFilter = this.maxCrossSellScoreFilterEmpty;
        this.productNameFilter = '';

        this.getProductCrossSellProducts();
    }
}
