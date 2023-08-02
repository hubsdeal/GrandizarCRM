import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HubProductsServiceProxy, HubProductDto, HubProductCategoriesServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditHubProductModalComponent } from './create-or-edit-hubProduct-modal.component';

import { ViewHubProductModalComponent } from './view-hubProduct-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'app-hubProducts',
    templateUrl: './hubProducts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class HubProductsComponent extends AppComponentBase {
    @ViewChild('createOrEditHubProductModal', { static: true })
    createOrEditHubProductModal: CreateOrEditHubProductModalComponent;
    @ViewChild('viewHubProductModal', { static: true }) viewHubProductModal: ViewHubProductModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    publishedFilter = -1;
    maxDisplayScoreFilter: number;
    maxDisplayScoreFilterEmpty: number;
    minDisplayScoreFilter: number;
    minDisplayScoreFilterEmpty: number;
    hubNameFilter = '';
    productNameFilter = '';
    @Input() hubId: number;

    selectedAll: boolean = false;
    selectedInput: number[] = [];


    allCategories: any[] = [];
    

    constructor(
        injector: Injector,
        private _hubProductsServiceProxy: HubProductsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _hubProductCategoriesServiceProxy: HubProductCategoriesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getHubProducts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._hubProductsServiceProxy
            .getAllByHubId(
                this.hubId,
                this.filterText,
                this.publishedFilter,
                this.maxDisplayScoreFilter == null ? this.maxDisplayScoreFilterEmpty : this.maxDisplayScoreFilter,
                this.minDisplayScoreFilter == null ? this.minDisplayScoreFilterEmpty : this.minDisplayScoreFilter,
                this.hubNameFilter,
                this.productNameFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.getHubCategories();
                this.primengTableHelper.hideLoadingIndicator();
            });
    }


    getHubCategories() {
        this._hubProductCategoriesServiceProxy.getAllByHubId(
            this.hubId,
            this.filterText,
            this.publishedFilter,
            this.maxDisplayScoreFilter == null ? this.maxDisplayScoreFilterEmpty : this.maxDisplayScoreFilter,
            this.minDisplayScoreFilter == null ? this.minDisplayScoreFilterEmpty : this.minDisplayScoreFilter,
            this.hubNameFilter,
            '',
            '',
            0,
            100000).subscribe(result => {
            this.allCategories = result.items;
        });
    }
    onChangesSelectAll() {
        for (var i = 0; i < this.primengTableHelper.records.length; i++) {
            this.primengTableHelper.records[i].selected = this.selectedAll;
        }
    }

    checkIfAllSelected() {
        this.selectedAll = this.primengTableHelper.records.every(function (item: any) {
            return item.selected == true;
        })
    }

    refreshCheckboxReloadList() {
        this.selectedAll = false;
        for (var i = 0; i < this.primengTableHelper.records.length; i++) {
            this.primengTableHelper.records[i].selected = false;
        }
        this.reloadPage();
    }
    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createHubProduct(): void {
        this.createOrEditHubProductModal.hubId = this.hubId;
        this.createOrEditHubProductModal.show();
    }

    deleteHubProduct(hubProduct: HubProductDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._hubProductsServiceProxy.delete(hubProduct.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._hubProductsServiceProxy
            .getHubProductsToExcel(
                this.filterText,
                this.publishedFilter,
                this.maxDisplayScoreFilter == null ? this.maxDisplayScoreFilterEmpty : this.maxDisplayScoreFilter,
                this.minDisplayScoreFilter == null ? this.minDisplayScoreFilterEmpty : this.minDisplayScoreFilter,
                this.hubNameFilter,
                this.productNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.publishedFilter = -1;
        this.maxDisplayScoreFilter = this.maxDisplayScoreFilterEmpty;
        this.minDisplayScoreFilter = this.maxDisplayScoreFilterEmpty;
        this.hubNameFilter = '';
        this.productNameFilter = '';

        this.getHubProducts();
    }
}
