import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ContactTagsServiceProxy, ContactTagDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContactTagModalComponent } from './create-or-edit-contactTag-modal.component';

import { ViewContactTagModalComponent } from './view-contactTag-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contactTags.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ContactTagsComponent extends AppComponentBase {
    @ViewChild('createOrEditContactTagModal', { static: true })
    createOrEditContactTagModal: CreateOrEditContactTagModalComponent;
    @ViewChild('viewContactTagModal', { static: true }) viewContactTagModal: ViewContactTagModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    customTagFilter = '';
    tagValueFilter = '';
    verifiedFilter = -1;
    maxSequenceFilter: number;
    maxSequenceFilterEmpty: number;
    minSequenceFilter: number;
    minSequenceFilterEmpty: number;
    contactFullNameFilter = '';
    masterTagCategoryNameFilter = '';
    masterTagNameFilter = '';

    constructor(
        injector: Injector,
        private _contactTagsServiceProxy: ContactTagsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContactTags(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contactTagsServiceProxy
            .getAll(
                this.filterText,
                this.customTagFilter,
                this.tagValueFilter,
                this.verifiedFilter,
                this.maxSequenceFilter == null ? this.maxSequenceFilterEmpty : this.maxSequenceFilter,
                this.minSequenceFilter == null ? this.minSequenceFilterEmpty : this.minSequenceFilter,
                this.contactFullNameFilter,
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

    createContactTag(): void {
        this.createOrEditContactTagModal.show();
    }

    deleteContactTag(contactTag: ContactTagDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._contactTagsServiceProxy.delete(contactTag.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._contactTagsServiceProxy
            .getContactTagsToExcel(
                this.filterText,
                this.customTagFilter,
                this.tagValueFilter,
                this.verifiedFilter,
                this.maxSequenceFilter == null ? this.maxSequenceFilterEmpty : this.maxSequenceFilter,
                this.minSequenceFilter == null ? this.minSequenceFilterEmpty : this.minSequenceFilter,
                this.contactFullNameFilter,
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
        this.verifiedFilter = -1;
        this.maxSequenceFilter = this.maxSequenceFilterEmpty;
        this.minSequenceFilter = this.maxSequenceFilterEmpty;
        this.contactFullNameFilter = '';
        this.masterTagCategoryNameFilter = '';
        this.masterTagNameFilter = '';

        this.getContactTags();
    }
}
