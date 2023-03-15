import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MasterTagsServiceProxy, MasterTagDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditMasterTagModalComponent } from './create-or-edit-masterTag-modal.component';

import { ViewMasterTagModalComponent } from './view-masterTag-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './masterTags.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class MasterTagsComponent extends AppComponentBase {
    @ViewChild('createOrEditMasterTagModal', { static: true })
    createOrEditMasterTagModal: CreateOrEditMasterTagModalComponent;
    @ViewChild('viewMasterTagModal', { static: true }) viewMasterTagModal: ViewMasterTagModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    descriptionFilter = '';
    synonymsFilter = '';
    maxDisplaySequenceFilter: number;
    maxDisplaySequenceFilterEmpty: number;
    minDisplaySequenceFilter: number;
    minDisplaySequenceFilterEmpty: number;
    masterTagCategoryNameFilter = '';
    mediaLibraryNameFilter = '';

    constructor(
        injector: Injector,
        private _masterTagsServiceProxy: MasterTagsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getMasterTags(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._masterTagsServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.descriptionFilter,
                this.synonymsFilter,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.masterTagCategoryNameFilter,
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

    createMasterTag(): void {
        this.createOrEditMasterTagModal.show();
    }

    deleteMasterTag(masterTag: MasterTagDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._masterTagsServiceProxy.delete(masterTag.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._masterTagsServiceProxy
            .getMasterTagsToExcel(
                this.filterText,
                this.nameFilter,
                this.descriptionFilter,
                this.synonymsFilter,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.masterTagCategoryNameFilter,
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
        this.synonymsFilter = '';
        this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.masterTagCategoryNameFilter = '';
        this.mediaLibraryNameFilter = '';

        this.getMasterTags();
    }
}
