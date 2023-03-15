import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MasterTagCategoriesServiceProxy, MasterTagCategoryDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditMasterTagCategoryModalComponent } from './create-or-edit-masterTagCategory-modal.component';

import { ViewMasterTagCategoryModalComponent } from './view-masterTagCategory-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './masterTagCategories.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class MasterTagCategoriesComponent extends AppComponentBase {
    @ViewChild('createOrEditMasterTagCategoryModal', { static: true })
    createOrEditMasterTagCategoryModal: CreateOrEditMasterTagCategoryModalComponent;
    @ViewChild('viewMasterTagCategoryModal', { static: true })
    viewMasterTagCategoryModal: ViewMasterTagCategoryModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    descriptionFilter = '';
    mediaLibraryNameFilter = '';

    constructor(
        injector: Injector,
        private _masterTagCategoriesServiceProxy: MasterTagCategoriesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getMasterTagCategories(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._masterTagCategoriesServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.descriptionFilter,
                this.mediaLibraryNameFilter,
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

    createMasterTagCategory(): void {
        this.createOrEditMasterTagCategoryModal.show();
    }

    deleteMasterTagCategory(masterTagCategory: MasterTagCategoryDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._masterTagCategoriesServiceProxy.delete(masterTagCategory.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._masterTagCategoriesServiceProxy
            .getMasterTagCategoriesToExcel(
                this.filterText,
                this.nameFilter,
                this.descriptionFilter,
                this.mediaLibraryNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.descriptionFilter = '';
        this.mediaLibraryNameFilter = '';

        this.getMasterTagCategories();
    }
}
