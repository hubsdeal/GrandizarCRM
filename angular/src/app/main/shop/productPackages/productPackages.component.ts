import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductPackagesServiceProxy, ProductPackageDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductPackageModalComponent } from './create-or-edit-productPackage-modal.component';

import { ViewProductPackageModalComponent } from './view-productPackage-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './productPackages.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductPackagesComponent extends AppComponentBase {
    @ViewChild('createOrEditProductPackageModal', { static: true })
    createOrEditProductPackageModal: CreateOrEditProductPackageModalComponent;
    @ViewChild('viewProductPackageModal', { static: true }) viewProductPackageModal: ViewProductPackageModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxPackageProductIdFilter: number;
    maxPackageProductIdFilterEmpty: number;
    minPackageProductIdFilter: number;
    minPackageProductIdFilterEmpty: number;
    maxDisplaySequenceFilter: number;
    maxDisplaySequenceFilterEmpty: number;
    minDisplaySequenceFilter: number;
    minDisplaySequenceFilterEmpty: number;
    maxPriceFilter: number;
    maxPriceFilterEmpty: number;
    minPriceFilter: number;
    minPriceFilterEmpty: number;
    maxQuantityFilter: number;
    maxQuantityFilterEmpty: number;
    minQuantityFilter: number;
    minQuantityFilterEmpty: number;
    productNameFilter = '';
    mediaLibraryNameFilter = '';

    constructor(
        injector: Injector,
        private _productPackagesServiceProxy: ProductPackagesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductPackages(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productPackagesServiceProxy
            .getAll(
                this.filterText,
                this.maxPackageProductIdFilter == null
                    ? this.maxPackageProductIdFilterEmpty
                    : this.maxPackageProductIdFilter,
                this.minPackageProductIdFilter == null
                    ? this.minPackageProductIdFilterEmpty
                    : this.minPackageProductIdFilter,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.maxPriceFilter == null ? this.maxPriceFilterEmpty : this.maxPriceFilter,
                this.minPriceFilter == null ? this.minPriceFilterEmpty : this.minPriceFilter,
                this.maxQuantityFilter == null ? this.maxQuantityFilterEmpty : this.maxQuantityFilter,
                this.minQuantityFilter == null ? this.minQuantityFilterEmpty : this.minQuantityFilter,
                this.productNameFilter,
                this.mediaLibraryNameFilter,
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

    createProductPackage(): void {
        this.createOrEditProductPackageModal.show();
    }

    deleteProductPackage(productPackage: ProductPackageDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productPackagesServiceProxy.delete(productPackage.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productPackagesServiceProxy
            .getProductPackagesToExcel(
                this.filterText,
                this.maxPackageProductIdFilter == null
                    ? this.maxPackageProductIdFilterEmpty
                    : this.maxPackageProductIdFilter,
                this.minPackageProductIdFilter == null
                    ? this.minPackageProductIdFilterEmpty
                    : this.minPackageProductIdFilter,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.maxPriceFilter == null ? this.maxPriceFilterEmpty : this.maxPriceFilter,
                this.minPriceFilter == null ? this.minPriceFilterEmpty : this.minPriceFilter,
                this.maxQuantityFilter == null ? this.maxQuantityFilterEmpty : this.maxQuantityFilter,
                this.minQuantityFilter == null ? this.minQuantityFilterEmpty : this.minQuantityFilter,
                this.productNameFilter,
                this.mediaLibraryNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxPackageProductIdFilter = this.maxPackageProductIdFilterEmpty;
        this.minPackageProductIdFilter = this.maxPackageProductIdFilterEmpty;
        this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.maxPriceFilter = this.maxPriceFilterEmpty;
        this.minPriceFilter = this.maxPriceFilterEmpty;
        this.maxQuantityFilter = this.maxQuantityFilterEmpty;
        this.minQuantityFilter = this.maxQuantityFilterEmpty;
        this.productNameFilter = '';
        this.mediaLibraryNameFilter = '';

        this.getProductPackages();
    }
}
