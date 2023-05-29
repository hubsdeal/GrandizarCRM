import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskWorkItemsServiceProxy, TaskWorkItemDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTaskWorkItemModalComponent } from './create-or-edit-taskWorkItem-modal.component';

import { ViewTaskWorkItemModalComponent } from './view-taskWorkItem-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './taskWorkItems.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class TaskWorkItemsComponent extends AppComponentBase {
    @ViewChild('createOrEditTaskWorkItemModal', { static: true })
    createOrEditTaskWorkItemModal: CreateOrEditTaskWorkItemModalComponent;
    @ViewChild('viewTaskWorkItemModal', { static: true }) viewTaskWorkItemModal: ViewTaskWorkItemModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    estimatedHoursFilter = '';
    actualHoursFilter = '';
    maxStartDateFilter: DateTime;
    minStartDateFilter: DateTime;
    maxEndDateFilter: DateTime;
    minEndDateFilter: DateTime;
    startTimeFilter = '';
    endTimeFilter = '';
    openOrClosedFilter = -1;
    maxCompletionPercentageFilter: number;
    maxCompletionPercentageFilterEmpty: number;
    minCompletionPercentageFilter: number;
    minCompletionPercentageFilterEmpty: number;
    taskEventNameFilter = '';
    employeeNameFilter = '';

    constructor(
        injector: Injector,
        private _taskWorkItemsServiceProxy: TaskWorkItemsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getTaskWorkItems(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._taskWorkItemsServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.estimatedHoursFilter,
                this.actualHoursFilter,
                this.maxStartDateFilter === undefined
                    ? this.maxStartDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
                this.minStartDateFilter === undefined
                    ? this.minStartDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
                this.maxEndDateFilter === undefined
                    ? this.maxEndDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
                this.minEndDateFilter === undefined
                    ? this.minEndDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
                this.startTimeFilter,
                this.endTimeFilter,
                this.openOrClosedFilter,
                this.maxCompletionPercentageFilter == null
                    ? this.maxCompletionPercentageFilterEmpty
                    : this.maxCompletionPercentageFilter,
                this.minCompletionPercentageFilter == null
                    ? this.minCompletionPercentageFilterEmpty
                    : this.minCompletionPercentageFilter,
                this.taskEventNameFilter,
                this.employeeNameFilter,
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

    createTaskWorkItem(): void {
        this.createOrEditTaskWorkItemModal.show();
    }

    deleteTaskWorkItem(taskWorkItem: TaskWorkItemDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._taskWorkItemsServiceProxy.delete(taskWorkItem.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._taskWorkItemsServiceProxy
            .getTaskWorkItemsToExcel(
                this.filterText,
                this.nameFilter,
                this.estimatedHoursFilter,
                this.actualHoursFilter,
                this.maxStartDateFilter === undefined
                    ? this.maxStartDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
                this.minStartDateFilter === undefined
                    ? this.minStartDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
                this.maxEndDateFilter === undefined
                    ? this.maxEndDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
                this.minEndDateFilter === undefined
                    ? this.minEndDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
                this.startTimeFilter,
                this.endTimeFilter,
                this.openOrClosedFilter,
                this.maxCompletionPercentageFilter == null
                    ? this.maxCompletionPercentageFilterEmpty
                    : this.maxCompletionPercentageFilter,
                this.minCompletionPercentageFilter == null
                    ? this.minCompletionPercentageFilterEmpty
                    : this.minCompletionPercentageFilter,
                this.taskEventNameFilter,
                this.employeeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.estimatedHoursFilter = '';
        this.actualHoursFilter = '';
        this.maxStartDateFilter = undefined;
        this.minStartDateFilter = undefined;
        this.maxEndDateFilter = undefined;
        this.minEndDateFilter = undefined;
        this.startTimeFilter = '';
        this.endTimeFilter = '';
        this.openOrClosedFilter = -1;
        this.maxCompletionPercentageFilter = this.maxCompletionPercentageFilterEmpty;
        this.minCompletionPercentageFilter = this.maxCompletionPercentageFilterEmpty;
        this.taskEventNameFilter = '';
        this.employeeNameFilter = '';

        this.getTaskWorkItems();
    }
}
