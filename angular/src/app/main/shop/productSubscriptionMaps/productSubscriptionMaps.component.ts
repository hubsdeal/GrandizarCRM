import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    ProductSubscriptionMapsServiceProxy,
    ProductSubscriptionMapDto,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductSubscriptionMapModalComponent } from './create-or-edit-productSubscriptionMap-modal.component';

import { ViewProductSubscriptionMapModalComponent } from './view-productSubscriptionMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './productSubscriptionMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductSubscriptionMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditProductSubscriptionMapModal', { static: true })
    createOrEditProductSubscriptionMapModal: CreateOrEditProductSubscriptionMapModalComponent;
    @ViewChild('viewProductSubscriptionMapModal', { static: true })
    viewProductSubscriptionMapModal: ViewProductSubscriptionMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDiscountPercentageFilter: number;
    maxDiscountPercentageFilterEmpty: number;
    minDiscountPercentageFilter: number;
    minDiscountPercentageFilterEmpty: number;
    maxDiscountAmountFilter: number;
    maxDiscountAmountFilterEmpty: number;
    minDiscountAmountFilter: number;
    minDiscountAmountFilterEmpty: number;
    maxPriceFilter: number;
    maxPriceFilterEmpty: number;
    minPriceFilter: number;
    minPriceFilterEmpty: number;
    productNameFilter = '';
    subscriptionTypeNameFilter = '';

    constructor(
        injector: Injector,
        private _productSubscriptionMapsServiceProxy: ProductSubscriptionMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductSubscriptionMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productSubscriptionMapsServiceProxy
            .getAll(
                this.filterText,
                this.maxDiscountPercentageFilter == null
                    ? this.maxDiscountPercentageFilterEmpty
                    : this.maxDiscountPercentageFilter,
                this.minDiscountPercentageFilter == null
                    ? this.minDiscountPercentageFilterEmpty
                    : this.minDiscountPercentageFilter,
                this.maxDiscountAmountFilter == null ? this.maxDiscountAmountFilterEmpty : this.maxDiscountAmountFilter,
                this.minDiscountAmountFilter == null ? this.minDiscountAmountFilterEmpty : this.minDiscountAmountFilter,
                this.maxPriceFilter == null ? this.maxPriceFilterEmpty : this.maxPriceFilter,
                this.minPriceFilter == null ? this.minPriceFilterEmpty : this.minPriceFilter,
                this.productNameFilter,
                this.subscriptionTypeNameFilter,
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

    createProductSubscriptionMap(): void {
        this.createOrEditProductSubscriptionMapModal.show();
    }

    deleteProductSubscriptionMap(productSubscriptionMap: ProductSubscriptionMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productSubscriptionMapsServiceProxy.delete(productSubscriptionMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productSubscriptionMapsServiceProxy
            .getProductSubscriptionMapsToExcel(
                this.filterText,
                this.maxDiscountPercentageFilter == null
                    ? this.maxDiscountPercentageFilterEmpty
                    : this.maxDiscountPercentageFilter,
                this.minDiscountPercentageFilter == null
                    ? this.minDiscountPercentageFilterEmpty
                    : this.minDiscountPercentageFilter,
                this.maxDiscountAmountFilter == null ? this.maxDiscountAmountFilterEmpty : this.maxDiscountAmountFilter,
                this.minDiscountAmountFilter == null ? this.minDiscountAmountFilterEmpty : this.minDiscountAmountFilter,
                this.maxPriceFilter == null ? this.maxPriceFilterEmpty : this.maxPriceFilter,
                this.minPriceFilter == null ? this.minPriceFilterEmpty : this.minPriceFilter,
                this.productNameFilter,
                this.subscriptionTypeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxDiscountPercentageFilter = this.maxDiscountPercentageFilterEmpty;
        this.minDiscountPercentageFilter = this.maxDiscountPercentageFilterEmpty;
        this.maxDiscountAmountFilter = this.maxDiscountAmountFilterEmpty;
        this.minDiscountAmountFilter = this.maxDiscountAmountFilterEmpty;
        this.maxPriceFilter = this.maxPriceFilterEmpty;
        this.minPriceFilter = this.maxPriceFilterEmpty;
        this.productNameFilter = '';
        this.subscriptionTypeNameFilter = '';

        this.getProductSubscriptionMaps();
    }
}
