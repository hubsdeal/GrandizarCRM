import { Component, Injector, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CreateOrEditTaskEventModalComponent } from '@app/main/taskManagement/taskEvents/create-or-edit-taskEvent-modal.component';
import { ViewTaskEventModalComponent } from '@app/main/taskManagement/taskEvents/view-taskEvent-modal.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TaskEventsServiceProxy, TokenAuthServiceProxy, TaskEventDto } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { NotifyService } from 'abp-ng2-module';
import { DateTime } from 'luxon';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-admin-dashboard-recent-task',
  templateUrl: './admin-dashboard-recent-task.component.html',
  styleUrls: ['./admin-dashboard-recent-task.component.css'],
  animations: [appModuleAnimation()],
})
export class AdminDashboardRecentTaskComponent  extends AppComponentBase {
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

  selectedTeam:any;
  allTeams:any[]=[{id:1,displayName:"Team 1"},{id:2,displayName:"Team 2"},{id:3,displayName:"Team 3"}]

  selectedTag:any;
  allTags:any[]=[{id:1,displayName:"Tag 1"},{id:2,displayName:"Tag 2"},{id:3,displayName:"Tag 3"}]

  value: number = 50;

  selectedAll: boolean = false;
  
  constructor(
      injector: Injector,
      private _taskEventsServiceProxy: TaskEventsServiceProxy,
      private _notifyService: NotifyService,
      private _tokenAuth: TokenAuthServiceProxy,
      private _activatedRoute: ActivatedRoute,
      private _fileDownloadService: FileDownloadService,
      private _dateTimeService: DateTimeService
  ) {
      super(injector);
  }

  getTaskEvents(event?: LazyLoadEvent) {
      // if (this.primengTableHelper.shouldResetPaging(event)) {
      //     this.paginator.changePage(0);
      //     if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
      //         return;
      //     }
      // }

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
              '',
              0,
              10
          )
          .subscribe((result) => {
              this.primengTableHelper.totalRecordsCount = result.totalCount;
              this.primengTableHelper.records = result.items;
              this.primengTableHelper.hideLoadingIndicator();
          });
  }

  reloadPage(): void {
      //this.paginator.changePage(this.paginator.getPage());
      this.getTaskEvents();
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
}
