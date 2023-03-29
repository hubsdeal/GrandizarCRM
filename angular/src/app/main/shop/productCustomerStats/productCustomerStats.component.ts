import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductCustomerStatsServiceProxy, ProductCustomerStatDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductCustomerStatModalComponent } from './create-or-edit-productCustomerStat-modal.component';

import { ViewProductCustomerStatModalComponent } from './view-productCustomerStat-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './productCustomerStats.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductCustomerStatsComponent extends AppComponentBase {
    @ViewChild('createOrEditProductCustomerStatModal', { static: true })
    createOrEditProductCustomerStatModal: CreateOrEditProductCustomerStatModalComponent;
    @ViewChild('viewProductCustomerStatModal', { static: true })
    viewProductCustomerStatModal: ViewProductCustomerStatModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    clickedFilter = -1;
    wishedOrFavoriteFilter = -1;
    purchasedFilter = -1;
    maxPurchasedQuantityFilter: number;
    maxPurchasedQuantityFilterEmpty: number;
    minPurchasedQuantityFilter: number;
    minPurchasedQuantityFilterEmpty: number;
    sharedFilter = -1;
    pageLinkFilter = '';
    appOrWebFilter = -1;
    quitFromLinkFilter = '';
    productNameFilter = '';
    contactFullNameFilter = '';
    storeNameFilter = '';
    hubNameFilter = '';
    socialMediaNameFilter = '';

    constructor(
        injector: Injector,
        private _productCustomerStatsServiceProxy: ProductCustomerStatsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductCustomerStats(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productCustomerStatsServiceProxy
            .getAll(
                this.filterText,
                this.clickedFilter,
                this.wishedOrFavoriteFilter,
                this.purchasedFilter,
                this.maxPurchasedQuantityFilter == null
                    ? this.maxPurchasedQuantityFilterEmpty
                    : this.maxPurchasedQuantityFilter,
                this.minPurchasedQuantityFilter == null
                    ? this.minPurchasedQuantityFilterEmpty
                    : this.minPurchasedQuantityFilter,
                this.sharedFilter,
                this.pageLinkFilter,
                this.appOrWebFilter,
                this.quitFromLinkFilter,
                this.productNameFilter,
                this.contactFullNameFilter,
                this.storeNameFilter,
                this.hubNameFilter,
                this.socialMediaNameFilter,
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

    createProductCustomerStat(): void {
        this.createOrEditProductCustomerStatModal.show();
    }

    deleteProductCustomerStat(productCustomerStat: ProductCustomerStatDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productCustomerStatsServiceProxy.delete(productCustomerStat.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productCustomerStatsServiceProxy
            .getProductCustomerStatsToExcel(
                this.filterText,
                this.clickedFilter,
                this.wishedOrFavoriteFilter,
                this.purchasedFilter,
                this.maxPurchasedQuantityFilter == null
                    ? this.maxPurchasedQuantityFilterEmpty
                    : this.maxPurchasedQuantityFilter,
                this.minPurchasedQuantityFilter == null
                    ? this.minPurchasedQuantityFilterEmpty
                    : this.minPurchasedQuantityFilter,
                this.sharedFilter,
                this.pageLinkFilter,
                this.appOrWebFilter,
                this.quitFromLinkFilter,
                this.productNameFilter,
                this.contactFullNameFilter,
                this.storeNameFilter,
                this.hubNameFilter,
                this.socialMediaNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.clickedFilter = -1;
        this.wishedOrFavoriteFilter = -1;
        this.purchasedFilter = -1;
        this.maxPurchasedQuantityFilter = this.maxPurchasedQuantityFilterEmpty;
        this.minPurchasedQuantityFilter = this.maxPurchasedQuantityFilterEmpty;
        this.sharedFilter = -1;
        this.pageLinkFilter = '';
        this.appOrWebFilter = -1;
        this.quitFromLinkFilter = '';
        this.productNameFilter = '';
        this.contactFullNameFilter = '';
        this.storeNameFilter = '';
        this.hubNameFilter = '';
        this.socialMediaNameFilter = '';

        this.getProductCustomerStats();
    }
}
