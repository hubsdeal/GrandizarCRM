import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskTagsServiceProxy, TaskTagDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTaskTagModalComponent } from './create-or-edit-taskTag-modal.component';

import { ViewTaskTagModalComponent } from './view-taskTag-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './taskTags.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class TaskTagsComponent extends AppComponentBase {
    @ViewChild('createOrEditTaskTagModal', { static: true })
    createOrEditTaskTagModal: CreateOrEditTaskTagModalComponent;
    @ViewChild('viewTaskTagModal', { static: true }) viewTaskTagModal: ViewTaskTagModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    customTagFilter = '';
    tagValueFilter = '';
    verfiedFilter = -1;
    maxSequenceFilter: number;
    maxSequenceFilterEmpty: number;
    minSequenceFilter: number;
    minSequenceFilterEmpty: number;
    taskEventNameFilter = '';
    masterTagCategoryNameFilter = '';
    masterTagNameFilter = '';

    constructor(
        injector: Injector,
        private _taskTagsServiceProxy: TaskTagsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getTaskTags(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._taskTagsServiceProxy
            .getAll(
                this.filterText,
                this.customTagFilter,
                this.tagValueFilter,
                this.verfiedFilter,
                this.maxSequenceFilter == null ? this.maxSequenceFilterEmpty : this.maxSequenceFilter,
                this.minSequenceFilter == null ? this.minSequenceFilterEmpty : this.minSequenceFilter,
                this.taskEventNameFilter,
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

    createTaskTag(): void {
        this.createOrEditTaskTagModal.show();
    }

    deleteTaskTag(taskTag: TaskTagDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._taskTagsServiceProxy.delete(taskTag.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._taskTagsServiceProxy
            .getTaskTagsToExcel(
                this.filterText,
                this.customTagFilter,
                this.tagValueFilter,
                this.verfiedFilter,
                this.maxSequenceFilter == null ? this.maxSequenceFilterEmpty : this.maxSequenceFilter,
                this.minSequenceFilter == null ? this.minSequenceFilterEmpty : this.minSequenceFilter,
                this.taskEventNameFilter,
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
        this.verfiedFilter = -1;
        this.maxSequenceFilter = this.maxSequenceFilterEmpty;
        this.minSequenceFilter = this.maxSequenceFilterEmpty;
        this.taskEventNameFilter = '';
        this.masterTagCategoryNameFilter = '';
        this.masterTagNameFilter = '';

        this.getTaskTags();
    }
}
