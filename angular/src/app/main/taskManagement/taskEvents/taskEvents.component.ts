import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskEventsServiceProxy, TaskEventDto, TaskEventTaskStatusLookupTableDto, TaskTeamEmployeeLookupTableDto, TaskTeamsServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTaskEventModalComponent } from './create-or-edit-taskEvent-modal.component';

import { ViewTaskEventModalComponent } from './view-taskEvent-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './taskEvents.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class TaskEventsComponent extends AppComponentBase {
    @ViewChild('createOrEditTaskEventModal', { static: true })
    createOrEditTaskEventModal: CreateOrEditTaskEventModalComponent;
    @ViewChild('viewTaskEventModal', { static: true }) viewTaskEventModal: ViewTaskEventModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    descriptionFilter = '';
    statusFilter = -1;
    priorityFilter = -1;
    maxEventDateFilter: DateTime;
    minEventDateFilter: DateTime;
    startTimeFilter = '';
    endTimeFilter = '';
    templateFilter = -1;
    actualTimeFilter = '';
    maxEndDateFilter: DateTime;
    minEndDateFilter: DateTime;
    estimatedTimeFilter = '';
    hourAndMinutesFilter = '';
    taskStatusNameFilter = '';

    employeeList: TaskTeamEmployeeLookupTableDto[] = [];
    selectedEmployees: any;
    selectedEmployeesId:number
    //=[{id:1,displayName:"Team 1"},{id:2,displayName:"Team 2"},{id:3,displayName:"Team 3"}]

    selectedTag:any;
    allTags:any[]
    //=[{id:1,displayName:"Tag 1"},{id:2,displayName:"Tag 2"},{id:3,displayName:"Tag 3"}]

    value: number = 50;
    allTaskStatuss: TaskEventTaskStatusLookupTableDto[];
    selectedAll: boolean = false;
    constructor(
        injector: Injector,
        private _taskEventsServiceProxy: TaskEventsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _taskTeamsServiceProxy: TaskTeamsServiceProxy,
    ) {
        super(injector);
        this._taskEventsServiceProxy.getAllTaskStatusForTableDropdown().subscribe((result) => {
            this.allTaskStatuss = result;
        });
        this._taskTeamsServiceProxy.getAllEmployeeForLookupTable('','',0,1000).subscribe(result => {
            this.employeeList = result.items;
        });
        
    }

    getTaskEvents(event?: LazyLoadEvent) {
       
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._taskEventsServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.descriptionFilter,
                this.statusFilter,
                this.priorityFilter,
                this.maxEventDateFilter === undefined
                    ? this.maxEventDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEventDateFilter),
                this.minEventDateFilter === undefined
                    ? this.minEventDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEventDateFilter),
                this.startTimeFilter,
                this.endTimeFilter,
                this.templateFilter,
                this.actualTimeFilter,
                this.maxEndDateFilter === undefined
                    ? this.maxEndDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
                this.minEndDateFilter === undefined
                    ? this.minEndDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
                this.estimatedTimeFilter,
                this.hourAndMinutesFilter,
                this.taskStatusNameFilter,
                this.selectedEmployeesId,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.taskEvents;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createTaskEvent(): void {
        this.createOrEditTaskEventModal.show();
    }

    deleteTaskEvent(taskEvent: TaskEventDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._taskEventsServiceProxy.delete(taskEvent.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._taskEventsServiceProxy
            .getTaskEventsToExcel(
                this.filterText,
                this.nameFilter,
                this.descriptionFilter,
                this.statusFilter,
                this.priorityFilter,
                this.maxEventDateFilter === undefined
                    ? this.maxEventDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEventDateFilter),
                this.minEventDateFilter === undefined
                    ? this.minEventDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEventDateFilter),
                this.startTimeFilter,
                this.endTimeFilter,
                this.templateFilter,
                this.actualTimeFilter,
                this.maxEndDateFilter === undefined
                    ? this.maxEndDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
                this.minEndDateFilter === undefined
                    ? this.minEndDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
                this.estimatedTimeFilter,
                this.hourAndMinutesFilter,
                this.taskStatusNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.descriptionFilter = '';
        this.statusFilter = -1;
        this.priorityFilter = -1;
        this.maxEventDateFilter = undefined;
        this.minEventDateFilter = undefined;
        this.startTimeFilter = '';
        this.endTimeFilter = '';
        this.templateFilter = -1;
        this.actualTimeFilter = '';
        this.maxEndDateFilter = undefined;
        this.minEndDateFilter = undefined;
        this.estimatedTimeFilter = '';
        this.hourAndMinutesFilter = '';
        this.taskStatusNameFilter = '';

        this.getTaskEvents();
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
    onEmployeeSelect(event: any) {
        if (event) {
            this.selectedEmployeesId = event.value.id;
            this.getTaskEvents();
        }

    }
}
