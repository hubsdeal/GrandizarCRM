import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductTaskMapsServiceProxy, ProductTaskMapDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductTaskMapModalComponent } from './create-or-edit-productTaskMap-modal.component';

import { ViewProductTaskMapModalComponent } from './view-productTaskMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './productTaskMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductTaskMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditProductTaskMapModal', { static: true })
    createOrEditProductTaskMapModal: CreateOrEditProductTaskMapModalComponent;
    @ViewChild('viewProductTaskMapModal', { static: true }) viewProductTaskMapModal: ViewProductTaskMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    productNameFilter = '';
    taskEventNameFilter = '';
    productCategoryNameFilter = '';

    constructor(
        injector: Injector,
        private _productTaskMapsServiceProxy: ProductTaskMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductTaskMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productTaskMapsServiceProxy
            .getAll(
                this.filterText,
                this.productNameFilter,
                this.taskEventNameFilter,
                this.productCategoryNameFilter,
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

    createProductTaskMap(): void {
        this.createOrEditProductTaskMapModal.show();
    }

    deleteProductTaskMap(productTaskMap: ProductTaskMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productTaskMapsServiceProxy.delete(productTaskMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productTaskMapsServiceProxy
            .getProductTaskMapsToExcel(
                this.filterText,
                this.productNameFilter,
                this.taskEventNameFilter,
                this.productCategoryNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.productNameFilter = '';
        this.taskEventNameFilter = '';
        this.productCategoryNameFilter = '';

        this.getProductTaskMaps();
    }
}
