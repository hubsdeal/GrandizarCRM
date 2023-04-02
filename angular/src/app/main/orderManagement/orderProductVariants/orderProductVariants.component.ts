import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderProductVariantsServiceProxy, OrderProductVariantDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditOrderProductVariantModalComponent } from './create-or-edit-orderProductVariant-modal.component';

import { ViewOrderProductVariantModalComponent } from './view-orderProductVariant-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './orderProductVariants.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class OrderProductVariantsComponent extends AppComponentBase {
    @ViewChild('createOrEditOrderProductVariantModal', { static: true })
    createOrEditOrderProductVariantModal: CreateOrEditOrderProductVariantModalComponent;
    @ViewChild('viewOrderProductVariantModal', { static: true })
    viewOrderProductVariantModal: ViewOrderProductVariantModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxPriceFilter: number;
    maxPriceFilterEmpty: number;
    minPriceFilter: number;
    minPriceFilterEmpty: number;
    maxOrderProductInfoIdFilter: number;
    maxOrderProductInfoIdFilterEmpty: number;
    minOrderProductInfoIdFilter: number;
    minOrderProductInfoIdFilterEmpty: number;
    productVariantCategoryNameFilter = '';
    productVariantNameFilter = '';

    constructor(
        injector: Injector,
        private _orderProductVariantsServiceProxy: OrderProductVariantsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getOrderProductVariants(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._orderProductVariantsServiceProxy
            .getAll(
                this.filterText,
                this.maxPriceFilter == null ? this.maxPriceFilterEmpty : this.maxPriceFilter,
                this.minPriceFilter == null ? this.minPriceFilterEmpty : this.minPriceFilter,
                this.maxOrderProductInfoIdFilter == null
                    ? this.maxOrderProductInfoIdFilterEmpty
                    : this.maxOrderProductInfoIdFilter,
                this.minOrderProductInfoIdFilter == null
                    ? this.minOrderProductInfoIdFilterEmpty
                    : this.minOrderProductInfoIdFilter,
                this.productVariantCategoryNameFilter,
                this.productVariantNameFilter,
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

    createOrderProductVariant(): void {
        this.createOrEditOrderProductVariantModal.show();
    }

    deleteOrderProductVariant(orderProductVariant: OrderProductVariantDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._orderProductVariantsServiceProxy.delete(orderProductVariant.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._orderProductVariantsServiceProxy
            .getOrderProductVariantsToExcel(
                this.filterText,
                this.maxPriceFilter == null ? this.maxPriceFilterEmpty : this.maxPriceFilter,
                this.minPriceFilter == null ? this.minPriceFilterEmpty : this.minPriceFilter,
                this.maxOrderProductInfoIdFilter == null
                    ? this.maxOrderProductInfoIdFilterEmpty
                    : this.maxOrderProductInfoIdFilter,
                this.minOrderProductInfoIdFilter == null
                    ? this.minOrderProductInfoIdFilterEmpty
                    : this.minOrderProductInfoIdFilter,
                this.productVariantCategoryNameFilter,
                this.productVariantNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxPriceFilter = this.maxPriceFilterEmpty;
        this.minPriceFilter = this.maxPriceFilterEmpty;
        this.maxOrderProductInfoIdFilter = this.maxOrderProductInfoIdFilterEmpty;
        this.minOrderProductInfoIdFilter = this.maxOrderProductInfoIdFilterEmpty;
        this.productVariantCategoryNameFilter = '';
        this.productVariantNameFilter = '';

        this.getOrderProductVariants();
    }
}
