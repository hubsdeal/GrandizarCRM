import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductByVariantsServiceProxy, ProductByVariantDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductByVariantModalComponent } from './create-or-edit-productByVariant-modal.component';

import { ViewProductByVariantModalComponent } from './view-productByVariant-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './productByVariants.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductByVariantsComponent extends AppComponentBase {
    @ViewChild('createOrEditProductByVariantModal', { static: true })
    createOrEditProductByVariantModal: CreateOrEditProductByVariantModalComponent;
    @ViewChild('viewProductByVariantModal', { static: true })
    viewProductByVariantModal: ViewProductByVariantModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxPriceFilter: number;
    maxPriceFilterEmpty: number;
    minPriceFilter: number;
    minPriceFilterEmpty: number;
    maxDisplaySequenceFilter: number;
    maxDisplaySequenceFilterEmpty: number;
    minDisplaySequenceFilter: number;
    minDisplaySequenceFilterEmpty: number;
    descriptionFilter = '';
    productNameFilter = '';
    productVariantNameFilter = '';
    productVariantCategoryNameFilter = '';
    mediaLibraryNameFilter = '';

    constructor(
        injector: Injector,
        private _productByVariantsServiceProxy: ProductByVariantsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductByVariants(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productByVariantsServiceProxy
            .getAll(
                this.filterText,
                this.maxPriceFilter == null ? this.maxPriceFilterEmpty : this.maxPriceFilter,
                this.minPriceFilter == null ? this.minPriceFilterEmpty : this.minPriceFilter,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.descriptionFilter,
                this.productNameFilter,
                this.productVariantNameFilter,
                this.productVariantCategoryNameFilter,
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

    createProductByVariant(): void {
        this.createOrEditProductByVariantModal.show();
    }

    deleteProductByVariant(productByVariant: ProductByVariantDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productByVariantsServiceProxy.delete(productByVariant.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productByVariantsServiceProxy
            .getProductByVariantsToExcel(
                this.filterText,
                this.maxPriceFilter == null ? this.maxPriceFilterEmpty : this.maxPriceFilter,
                this.minPriceFilter == null ? this.minPriceFilterEmpty : this.minPriceFilter,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.descriptionFilter,
                this.productNameFilter,
                this.productVariantNameFilter,
                this.productVariantCategoryNameFilter,
                this.mediaLibraryNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxPriceFilter = this.maxPriceFilterEmpty;
        this.minPriceFilter = this.maxPriceFilterEmpty;
        this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.descriptionFilter = '';
        this.productNameFilter = '';
        this.productVariantNameFilter = '';
        this.productVariantCategoryNameFilter = '';
        this.mediaLibraryNameFilter = '';

        this.getProductByVariants();
    }
}
