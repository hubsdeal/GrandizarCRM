import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LeadTagsServiceProxy, LeadTagDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditLeadTagModalComponent } from './create-or-edit-leadTag-modal.component';

import { ViewLeadTagModalComponent } from './view-leadTag-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './leadTags.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class LeadTagsComponent extends AppComponentBase {
    @ViewChild('createOrEditLeadTagModal', { static: true })
    createOrEditLeadTagModal: CreateOrEditLeadTagModalComponent;
    @ViewChild('viewLeadTagModal', { static: true }) viewLeadTagModal: ViewLeadTagModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    customTagFilter = '';
    tagValueFilter = '';
    maxDisplaySequenceFilter: number;
    maxDisplaySequenceFilterEmpty: number;
    minDisplaySequenceFilter: number;
    minDisplaySequenceFilterEmpty: number;
    leadTitleFilter = '';
    masterTagCategoryNameFilter = '';
    masterTagNameFilter = '';

    constructor(
        injector: Injector,
        private _leadTagsServiceProxy: LeadTagsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getLeadTags(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._leadTagsServiceProxy
            .getAll(
                this.filterText,
                this.customTagFilter,
                this.tagValueFilter,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.leadTitleFilter,
                this.masterTagCategoryNameFilter,
                this.masterTagNameFilter,
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

    createLeadTag(): void {
        this.createOrEditLeadTagModal.show();
    }

    deleteLeadTag(leadTag: LeadTagDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._leadTagsServiceProxy.delete(leadTag.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._leadTagsServiceProxy
            .getLeadTagsToExcel(
                this.filterText,
                this.customTagFilter,
                this.tagValueFilter,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.leadTitleFilter,
                this.masterTagCategoryNameFilter,
                this.masterTagNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.customTagFilter = '';
        this.tagValueFilter = '';
        this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.leadTitleFilter = '';
        this.masterTagCategoryNameFilter = '';
        this.masterTagNameFilter = '';

        this.getLeadTags();
    }
}
