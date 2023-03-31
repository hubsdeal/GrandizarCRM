import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductReturnInfosServiceProxy, ProductReturnInfoDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductReturnInfoModalComponent } from './create-or-edit-productReturnInfo-modal.component';

import { ViewProductReturnInfoModalComponent } from './view-productReturnInfo-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './productReturnInfos.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductReturnInfosComponent extends AppComponentBase {
    @ViewChild('createOrEditProductReturnInfoModal', { static: true })
    createOrEditProductReturnInfoModal: CreateOrEditProductReturnInfoModalComponent;
    @ViewChild('viewProductReturnInfoModal', { static: true })
    viewProductReturnInfoModal: ViewProductReturnInfoModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    customerNoteFilter = '';
    adminNoteFilter = '';
    productNameFilter = '';
    returnTypeNameFilter = '';
    returnStatusNameFilter = '';

    constructor(
        injector: Injector,
        private _productReturnInfosServiceProxy: ProductReturnInfosServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductReturnInfos(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productReturnInfosServiceProxy
            .getAll(
                this.filterText,
                this.customerNoteFilter,
                this.adminNoteFilter,
                this.productNameFilter,
                this.returnTypeNameFilter,
                this.returnStatusNameFilter,
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

    createProductReturnInfo(): void {
        this.createOrEditProductReturnInfoModal.show();
    }

    deleteProductReturnInfo(productReturnInfo: ProductReturnInfoDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productReturnInfosServiceProxy.delete(productReturnInfo.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productReturnInfosServiceProxy
            .getProductReturnInfosToExcel(
                this.filterText,
                this.customerNoteFilter,
                this.adminNoteFilter,
                this.productNameFilter,
                this.returnTypeNameFilter,
                this.returnStatusNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.customerNoteFilter = '';
        this.adminNoteFilter = '';
        this.productNameFilter = '';
        this.returnTypeNameFilter = '';
        this.returnStatusNameFilter = '';

        this.getProductReturnInfos();
    }
}
