import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderProductInfosServiceProxy, OrderProductInfoDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditOrderProductInfoModalComponent } from './create-or-edit-orderProductInfo-modal.component';

import { ViewOrderProductInfoModalComponent } from './view-orderProductInfo-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './orderProductInfos.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class OrderProductInfosComponent extends AppComponentBase {
    @ViewChild('createOrEditOrderProductInfoModal', { static: true })
    createOrEditOrderProductInfoModal: CreateOrEditOrderProductInfoModalComponent;
    @ViewChild('viewOrderProductInfoModal', { static: true })
    viewOrderProductInfoModal: ViewOrderProductInfoModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxQuantityFilter: number;
    maxQuantityFilterEmpty: number;
    minQuantityFilter: number;
    minQuantityFilterEmpty: number;
    maxUnitPriceFilter: number;
    maxUnitPriceFilterEmpty: number;
    minUnitPriceFilter: number;
    minUnitPriceFilterEmpty: number;
    maxByProductDiscountAmountFilter: number;
    maxByProductDiscountAmountFilterEmpty: number;
    minByProductDiscountAmountFilter: number;
    minByProductDiscountAmountFilterEmpty: number;
    maxByProductDiscountPercentageFilter: number;
    maxByProductDiscountPercentageFilterEmpty: number;
    minByProductDiscountPercentageFilter: number;
    minByProductDiscountPercentageFilterEmpty: number;
    maxByProductTaxAmountFilter: number;
    maxByProductTaxAmountFilterEmpty: number;
    minByProductTaxAmountFilter: number;
    minByProductTaxAmountFilterEmpty: number;
    maxByProductTotalAmountFilter: number;
    maxByProductTotalAmountFilterEmpty: number;
    minByProductTotalAmountFilter: number;
    minByProductTotalAmountFilterEmpty: number;
    orderInvoiceNumberFilter = '';
    storeNameFilter = '';
    productNameFilter = '';
    measurementUnitNameFilter = '';

    constructor(
        injector: Injector,
        private _orderProductInfosServiceProxy: OrderProductInfosServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getOrderProductInfos(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._orderProductInfosServiceProxy
            .getAll(
                this.filterText,
                this.maxQuantityFilter == null ? this.maxQuantityFilterEmpty : this.maxQuantityFilter,
                this.minQuantityFilter == null ? this.minQuantityFilterEmpty : this.minQuantityFilter,
                this.maxUnitPriceFilter == null ? this.maxUnitPriceFilterEmpty : this.maxUnitPriceFilter,
                this.minUnitPriceFilter == null ? this.minUnitPriceFilterEmpty : this.minUnitPriceFilter,
                this.maxByProductDiscountAmountFilter == null
                    ? this.maxByProductDiscountAmountFilterEmpty
                    : this.maxByProductDiscountAmountFilter,
                this.minByProductDiscountAmountFilter == null
                    ? this.minByProductDiscountAmountFilterEmpty
                    : this.minByProductDiscountAmountFilter,
                this.maxByProductDiscountPercentageFilter == null
                    ? this.maxByProductDiscountPercentageFilterEmpty
                    : this.maxByProductDiscountPercentageFilter,
                this.minByProductDiscountPercentageFilter == null
                    ? this.minByProductDiscountPercentageFilterEmpty
                    : this.minByProductDiscountPercentageFilter,
                this.maxByProductTaxAmountFilter == null
                    ? this.maxByProductTaxAmountFilterEmpty
                    : this.maxByProductTaxAmountFilter,
                this.minByProductTaxAmountFilter == null
                    ? this.minByProductTaxAmountFilterEmpty
                    : this.minByProductTaxAmountFilter,
                this.maxByProductTotalAmountFilter == null
                    ? this.maxByProductTotalAmountFilterEmpty
                    : this.maxByProductTotalAmountFilter,
                this.minByProductTotalAmountFilter == null
                    ? this.minByProductTotalAmountFilterEmpty
                    : this.minByProductTotalAmountFilter,
                this.orderInvoiceNumberFilter,
                this.storeNameFilter,
                this.productNameFilter,
                this.measurementUnitNameFilter,
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

    createOrderProductInfo(): void {
        this.createOrEditOrderProductInfoModal.show();
    }

    deleteOrderProductInfo(orderProductInfo: OrderProductInfoDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._orderProductInfosServiceProxy.delete(orderProductInfo.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._orderProductInfosServiceProxy
            .getOrderProductInfosToExcel(
                this.filterText,
                this.maxQuantityFilter == null ? this.maxQuantityFilterEmpty : this.maxQuantityFilter,
                this.minQuantityFilter == null ? this.minQuantityFilterEmpty : this.minQuantityFilter,
                this.maxUnitPriceFilter == null ? this.maxUnitPriceFilterEmpty : this.maxUnitPriceFilter,
                this.minUnitPriceFilter == null ? this.minUnitPriceFilterEmpty : this.minUnitPriceFilter,
                this.maxByProductDiscountAmountFilter == null
                    ? this.maxByProductDiscountAmountFilterEmpty
                    : this.maxByProductDiscountAmountFilter,
                this.minByProductDiscountAmountFilter == null
                    ? this.minByProductDiscountAmountFilterEmpty
                    : this.minByProductDiscountAmountFilter,
                this.maxByProductDiscountPercentageFilter == null
                    ? this.maxByProductDiscountPercentageFilterEmpty
                    : this.maxByProductDiscountPercentageFilter,
                this.minByProductDiscountPercentageFilter == null
                    ? this.minByProductDiscountPercentageFilterEmpty
                    : this.minByProductDiscountPercentageFilter,
                this.maxByProductTaxAmountFilter == null
                    ? this.maxByProductTaxAmountFilterEmpty
                    : this.maxByProductTaxAmountFilter,
                this.minByProductTaxAmountFilter == null
                    ? this.minByProductTaxAmountFilterEmpty
                    : this.minByProductTaxAmountFilter,
                this.maxByProductTotalAmountFilter == null
                    ? this.maxByProductTotalAmountFilterEmpty
                    : this.maxByProductTotalAmountFilter,
                this.minByProductTotalAmountFilter == null
                    ? this.minByProductTotalAmountFilterEmpty
                    : this.minByProductTotalAmountFilter,
                this.orderInvoiceNumberFilter,
                this.storeNameFilter,
                this.productNameFilter,
                this.measurementUnitNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxQuantityFilter = this.maxQuantityFilterEmpty;
        this.minQuantityFilter = this.maxQuantityFilterEmpty;
        this.maxUnitPriceFilter = this.maxUnitPriceFilterEmpty;
        this.minUnitPriceFilter = this.maxUnitPriceFilterEmpty;
        this.maxByProductDiscountAmountFilter = this.maxByProductDiscountAmountFilterEmpty;
        this.minByProductDiscountAmountFilter = this.maxByProductDiscountAmountFilterEmpty;
        this.maxByProductDiscountPercentageFilter = this.maxByProductDiscountPercentageFilterEmpty;
        this.minByProductDiscountPercentageFilter = this.maxByProductDiscountPercentageFilterEmpty;
        this.maxByProductTaxAmountFilter = this.maxByProductTaxAmountFilterEmpty;
        this.minByProductTaxAmountFilter = this.maxByProductTaxAmountFilterEmpty;
        this.maxByProductTotalAmountFilter = this.maxByProductTotalAmountFilterEmpty;
        this.minByProductTotalAmountFilter = this.maxByProductTotalAmountFilterEmpty;
        this.orderInvoiceNumberFilter = '';
        this.storeNameFilter = '';
        this.productNameFilter = '';
        this.measurementUnitNameFilter = '';

        this.getOrderProductInfos();
    }
}
