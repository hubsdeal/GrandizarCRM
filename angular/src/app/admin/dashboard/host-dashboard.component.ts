import { Component, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DashboardCustomizationConst } from '@app/shared/common/customizable-dashboard/DashboardCustomizationConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { TaskEventsServiceProxy, TaskWorkItemsServiceProxy } from '@shared/service-proxies/service-proxies';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTime } from 'luxon';
// import { AccordionModule } from 'primeng/accordion';
@Component({
    templateUrl: './host-dashboard.component.html',
    styleUrls: ['./host-dashboard.component.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class HostDashboardComponent extends AppComponentBase {
    dashboardName = DashboardCustomizationConst.dashboardNames.defaultHostDashboard;
    // @ViewChild('p-accordion', { static: true }) paginator: AccordionModule;
    workItems: any;
    taskEventList: any;
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
    openOrClosedFilter = 0;
    maxCompletionPercentageFilter: number;
    maxCompletionPercentageFilterEmpty: number;
    minCompletionPercentageFilter: number;
    minCompletionPercentageFilterEmpty: number;
    taskEventNameFilter = '';
    employeeNameFilter = '';
    employeeId: number;
    showCalendarView: boolean;
    showListView: boolean;
    constructor(injector: Injector,
        private _appSessionService: AppSessionService,
        private _taskWorkItemsServiceProxy: TaskWorkItemsServiceProxy,
        private _dateTimeService: DateTimeService,
        private _taskEventsServiceProxy: TaskEventsServiceProxy,
    ) {
        super(injector);
        if (this._appSessionService.userId != null) {
            this.employeeId = this._appSessionService.userId;
        }
        this.getTaskWorkItems();
        this.getTaskEvents();
    }

    onCalenderView() {
        this.showCalendarView = !this.showCalendarView;
        this.showListView = !this.showListView;
      }
    
      onListView() {
        this.showListView = !this.showListView;
        this.showCalendarView = !this.showCalendarView;
      }
    
    getTaskWorkItems() {
        // if (this.primengTableHelper.shouldResetPaging(event)) {
        //   this.paginator.changePage(0);
        //   return;
        // }

        // this.primengTableHelper.showLoadingIndicator();

        this._taskWorkItemsServiceProxy.getAllByTaskEventId(
            //this.taskEventId,
            undefined,
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
            this.employeeId,
            '',
            0,
            50
            // this.primengTableHelper.getSorting(this.dataTable),
            // this.primengTableHelper.getSkipCount(this.paginator, event),
            // this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            //   this.primengTableHelper.totalRecordsCount = result.totalCount;
            //   this.primengTableHelper.records = result.items;
            //   this.totalCount.emit(this.primengTableHelper.totalRecordsCount);
            //this.isReload.emit(true);
            //this.primengTableHelper.hideLoadingIndicator();
            this.workItems = result.items

        });
    }
    getTaskEvents() {


        this._taskEventsServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                undefined,
                19,
                undefined,
                undefined,
                undefined,
                this.startTimeFilter,
                this.endTimeFilter,
                0,
                undefined,
                this.maxEndDateFilter === undefined
                    ? this.maxEndDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
                this.minEndDateFilter === undefined
                    ? this.minEndDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
                undefined,
                undefined,
                undefined,
                this.employeeId,
                '',
                0,
                100
            )
            .subscribe((result) => {
                this.taskEventList = result.taskEvents;
            });
    }
}
