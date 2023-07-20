import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HubProductCategoriesServiceProxy, HubProductCategoryDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditHubProductCategoryModalComponent } from './create-or-edit-hubProductCategory-modal.component';

import { ViewHubProductCategoryModalComponent } from './view-hubProductCategory-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'app-hubProductCategories',
    templateUrl: './hubProductCategories.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class HubProductCategoriesComponent extends AppComponentBase {
    @ViewChild('createOrEditHubProductCategoryModal', { static: true })
    createOrEditHubProductCategoryModal: CreateOrEditHubProductCategoryModalComponent;
    @ViewChild('viewHubProductCategoryModal', { static: true })
    viewHubProductCategoryModal: ViewHubProductCategoryModalComponent;

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
    productCategoryNameFilter = '';

    constructor(
        injector: Injector,
        private _hubProductCategoriesServiceProxy: HubProductCategoriesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getHubProductCategories(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._hubProductCategoriesServiceProxy
            .getAll(
                this.filterText,
                this.publishedFilter,
                this.maxDisplayScoreFilter == null ? this.maxDisplayScoreFilterEmpty : this.maxDisplayScoreFilter,
                this.minDisplayScoreFilter == null ? this.minDisplayScoreFilterEmpty : this.minDisplayScoreFilter,
                this.hubNameFilter,
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

    createHubProductCategory(): void {
        this.createOrEditHubProductCategoryModal.show();
    }

    deleteHubProductCategory(hubProductCategory: HubProductCategoryDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._hubProductCategoriesServiceProxy.delete(hubProductCategory.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._hubProductCategoriesServiceProxy
            .getHubProductCategoriesToExcel(
                this.filterText,
                this.publishedFilter,
                this.maxDisplayScoreFilter == null ? this.maxDisplayScoreFilterEmpty : this.maxDisplayScoreFilter,
                this.minDisplayScoreFilter == null ? this.minDisplayScoreFilterEmpty : this.minDisplayScoreFilter,
                this.hubNameFilter,
                this.productCategoryNameFilter
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
        this.productCategoryNameFilter = '';

        this.getHubProductCategories();
    }
}
