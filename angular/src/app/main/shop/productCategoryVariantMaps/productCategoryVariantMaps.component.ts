import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    ProductCategoryVariantMapsServiceProxy,
    ProductCategoryVariantMapDto,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductCategoryVariantMapModalComponent } from './create-or-edit-productCategoryVariantMap-modal.component';

import { ViewProductCategoryVariantMapModalComponent } from './view-productCategoryVariantMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector:'appProductCategoryAndVariantCategoryMap',
    templateUrl: './productCategoryVariantMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductCategoryVariantMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditProductCategoryVariantMapModal', { static: true })
    createOrEditProductCategoryVariantMapModal: CreateOrEditProductCategoryVariantMapModalComponent;
    @ViewChild('viewProductCategoryVariantMapModal', { static: true })
    viewProductCategoryVariantMapModal: ViewProductCategoryVariantMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    productCategoryNameFilter = '';
    productVariantCategoryNameFilter = '';
    @Input() productCategoryIdFilter:number;
    constructor(
        injector: Injector,
        private _productCategoryVariantMapsServiceProxy: ProductCategoryVariantMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductCategoryVariantMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productCategoryVariantMapsServiceProxy
            .getAll(
                this.productCategoryIdFilter,
                this.filterText,
                this.productCategoryNameFilter,
                this.productVariantCategoryNameFilter,
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

    createProductCategoryVariantMap(): void {
        this.createOrEditProductCategoryVariantMapModal.productCategoryIdFilter=this.productCategoryIdFilter;
        this.createOrEditProductCategoryVariantMapModal.show();
    }
   

    deleteProductCategoryVariantMap(productCategoryVariantMap: ProductCategoryVariantMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productCategoryVariantMapsServiceProxy.delete(productCategoryVariantMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productCategoryVariantMapsServiceProxy
            .getProductCategoryVariantMapsToExcel(
                this.filterText,
                this.productCategoryNameFilter,
                this.productVariantCategoryNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.productCategoryNameFilter = '';
        this.productVariantCategoryNameFilter = '';

        this.getProductCategoryVariantMaps();
    }
    getTotalcount(total){
        return '| Total : ' + total;
    }
}
