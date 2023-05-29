import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskTeamsServiceProxy, TaskTeamDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTaskTeamModalComponent } from './create-or-edit-taskTeam-modal.component';

import { ViewTaskTeamModalComponent } from './view-taskTeam-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './taskTeams.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class TaskTeamsComponent extends AppComponentBase {
    @ViewChild('createOrEditTaskTeamModal', { static: true })
    createOrEditTaskTeamModal: CreateOrEditTaskTeamModalComponent;
    @ViewChild('viewTaskTeamModal', { static: true }) viewTaskTeamModal: ViewTaskTeamModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxStartDateFilter: DateTime;
    minStartDateFilter: DateTime;
    startTimeFilter = '';
    endTimeFilter = '';
    hourMinutesFilter = '';
    maxEndDateFilter: DateTime;
    minEndDateFilter: DateTime;
    isPrimaryFilter = -1;
    estimatedHourFilter = '';
    subTaskTitleFilter = '';
    taskEventNameFilter = '';
    employeeNameFilter = '';
    contactFullNameFilter = '';

    constructor(
        injector: Injector,
        private _taskTeamsServiceProxy: TaskTeamsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getTaskTeams(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._taskTeamsServiceProxy
            .getAll(
                this.filterText,
                this.maxStartDateFilter === undefined
                    ? this.maxStartDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
                this.minStartDateFilter === undefined
                    ? this.minStartDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
                this.startTimeFilter,
                this.endTimeFilter,
                this.hourMinutesFilter,
                this.maxEndDateFilter === undefined
                    ? this.maxEndDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
                this.minEndDateFilter === undefined
                    ? this.minEndDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
                this.isPrimaryFilter,
                this.estimatedHourFilter,
                this.subTaskTitleFilter,
                this.taskEventNameFilter,
                this.employeeNameFilter,
                this.contactFullNameFilter,
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

    createTaskTeam(): void {
        this.createOrEditTaskTeamModal.show();
    }

    deleteTaskTeam(taskTeam: TaskTeamDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._taskTeamsServiceProxy.delete(taskTeam.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._taskTeamsServiceProxy
            .getTaskTeamsToExcel(
                this.filterText,
                this.maxStartDateFilter === undefined
                    ? this.maxStartDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
                this.minStartDateFilter === undefined
                    ? this.minStartDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
                this.startTimeFilter,
                this.endTimeFilter,
                this.hourMinutesFilter,
                this.maxEndDateFilter === undefined
                    ? this.maxEndDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
                this.minEndDateFilter === undefined
                    ? this.minEndDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
                this.isPrimaryFilter,
                this.estimatedHourFilter,
                this.subTaskTitleFilter,
                this.taskEventNameFilter,
                this.employeeNameFilter,
                this.contactFullNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxStartDateFilter = undefined;
        this.minStartDateFilter = undefined;
        this.startTimeFilter = '';
        this.endTimeFilter = '';
        this.hourMinutesFilter = '';
        this.maxEndDateFilter = undefined;
        this.minEndDateFilter = undefined;
        this.isPrimaryFilter = -1;
        this.estimatedHourFilter = '';
        this.subTaskTitleFilter = '';
        this.taskEventNameFilter = '';
        this.employeeNameFilter = '';
        this.contactFullNameFilter = '';

        this.getTaskTeams();
    }
}
