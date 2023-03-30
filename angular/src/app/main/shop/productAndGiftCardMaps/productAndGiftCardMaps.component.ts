import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductAndGiftCardMapsServiceProxy, ProductAndGiftCardMapDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductAndGiftCardMapModalComponent } from './create-or-edit-productAndGiftCardMap-modal.component';

import { ViewProductAndGiftCardMapModalComponent } from './view-productAndGiftCardMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './productAndGiftCardMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductAndGiftCardMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditProductAndGiftCardMapModal', { static: true })
    createOrEditProductAndGiftCardMapModal: CreateOrEditProductAndGiftCardMapModalComponent;
    @ViewChild('viewProductAndGiftCardMapModal', { static: true })
    viewProductAndGiftCardMapModal: ViewProductAndGiftCardMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxPurchaseAmountFilter: number;
    maxPurchaseAmountFilterEmpty: number;
    minPurchaseAmountFilter: number;
    minPurchaseAmountFilterEmpty: number;
    maxGiftAmountFilter: number;
    maxGiftAmountFilterEmpty: number;
    minGiftAmountFilter: number;
    minGiftAmountFilterEmpty: number;
    productNameFilter = '';
    currencyNameFilter = '';

    constructor(
        injector: Injector,
        private _productAndGiftCardMapsServiceProxy: ProductAndGiftCardMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductAndGiftCardMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productAndGiftCardMapsServiceProxy
            .getAll(
                this.filterText,
                this.maxPurchaseAmountFilter == null ? this.maxPurchaseAmountFilterEmpty : this.maxPurchaseAmountFilter,
                this.minPurchaseAmountFilter == null ? this.minPurchaseAmountFilterEmpty : this.minPurchaseAmountFilter,
                this.maxGiftAmountFilter == null ? this.maxGiftAmountFilterEmpty : this.maxGiftAmountFilter,
                this.minGiftAmountFilter == null ? this.minGiftAmountFilterEmpty : this.minGiftAmountFilter,
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

    createProductAndGiftCardMap(): void {
        this.createOrEditProductAndGiftCardMapModal.show();
    }

    deleteProductAndGiftCardMap(productAndGiftCardMap: ProductAndGiftCardMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productAndGiftCardMapsServiceProxy.delete(productAndGiftCardMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productAndGiftCardMapsServiceProxy
            .getProductAndGiftCardMapsToExcel(
                this.filterText,
                this.maxPurchaseAmountFilter == null ? this.maxPurchaseAmountFilterEmpty : this.maxPurchaseAmountFilter,
                this.minPurchaseAmountFilter == null ? this.minPurchaseAmountFilterEmpty : this.minPurchaseAmountFilter,
                this.maxGiftAmountFilter == null ? this.maxGiftAmountFilterEmpty : this.maxGiftAmountFilter,
                this.minGiftAmountFilter == null ? this.minGiftAmountFilterEmpty : this.minGiftAmountFilter,
                this.productNameFilter,
                this.currencyNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxPurchaseAmountFilter = this.maxPurchaseAmountFilterEmpty;
        this.minPurchaseAmountFilter = this.maxPurchaseAmountFilterEmpty;
        this.maxGiftAmountFilter = this.maxGiftAmountFilterEmpty;
        this.minGiftAmountFilter = this.maxGiftAmountFilterEmpty;
        this.productNameFilter = '';
        this.currencyNameFilter = '';

        this.getProductAndGiftCardMaps();
    }
}
