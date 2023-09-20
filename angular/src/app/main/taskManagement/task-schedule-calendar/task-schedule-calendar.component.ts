import { Component, OnInit, ViewChild } from '@angular/core';
import { CalendarComponent } from 'ng-fullcalendar';
import { Options } from 'fullcalendar';
import { EventService } from './event.service';
import { TaskEventsServiceProxy } from '@shared/service-proxies/service-proxies';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
  selector: 'app-task-schedule-calendar',
  templateUrl: './task-schedule-calendar.component.html',
  styleUrls: ['./task-schedule-calendar.component.css']
})
export class TaskScheduleCalendarComponent implements OnInit  {
  calendarOptions: Options;
 displayEvent: any;
  @ViewChild(CalendarComponent) ucCalendar: CalendarComponent;
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
  openOrClosedFilter = -1;
  maxCompletionPercentageFilter: number;
  maxCompletionPercentageFilterEmpty: number;
  minCompletionPercentageFilter: number;
  minCompletionPercentageFilterEmpty: number;
  taskEventNameFilter = '';
  employeeNameFilter = '';
  constructor(protected eventService: EventService,
    private _taskEventsServiceProxy: TaskEventsServiceProxy,
    private _dateTimeService: DateTimeService) { }
  ngOnInit() { 
    this.GetTaskEventList();
  }
  GetTaskEventList(){
    this._taskEventsServiceProxy
      .getAll(
        this.filterText,
        this.nameFilter,
        undefined,
        undefined,
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
        1,//this.employeeId,
        '',
        0,
        100
      )
      .subscribe((result) => {
        this.taskEventList = result.taskEvents;
        var data = this.mapData(this.taskEventList);
        console.log(data);
        if(data !=null){
          this.calendarOptions = {
            editable: true,
            eventLimit: false,
            // initialView: 'agendaWeek',
            //allDaySlot: false,
            header: {
              left: 'prev,next today',
              center: 'title',
              right: 'agendaWeek',
            },
            events: data,
            eventRender: (v,el) => {console.log(v, el)}
          };
        }
        
      });
   
  }
  clickButton(model: any) {
    this.displayEvent = model;
  }
  eventClick(model: any) {
    model = {
      event: {
        id: model.event.id,
        start: model.event.start,
        end: model.event.end,
        title: model.event.title,
        // allDay: model.event.allDay,
        // DoctorsCount: model.event.DoctorsCount,
        // TotalAppointments: model.event.TotalAppointments,
        // Booked: model.event.Booked,
        // Canceled: model.event.Canceled
        // other params
      },
      duration: {}
    }
    this.displayEvent = model;
  }
  updateEvent(model: any) {
    model = {
      event: {
        id: model.event.id,
        start: model.event.start,
        end: model.event.end,
        title: model.event.title
        // other params
      },
      duration: {
        _data: model.duration._data
      }
    }
    this.displayEvent = model;
  }
  eventRender(e) {
    const html = `<ul>
      <li>${e.event.title}</li>
    </ul>`;
    e.element.html(html)
  }
  mapData(data: any[]): any[] {
    // const mappedData = data.map(item => {
    //   const yearMonth = new Date().toISOString().slice(0, 7); // Get current year and month
    //   return {
    //     title: item.taskEvent.name,
    //     start: `${yearMonth}-01`
    //     // DoctorsCount: item.DoctorsCount,
    //     // TotalAppointments: item.TotalAppointments,
    //     // Booked: item.Booked,
    //     // Cancelled: item.Cancelled
    //   };
    // });
    const mappedData = data.map(item => {
      const date = new Date(item.taskEvent.eventDate);
      const year = date.getFullYear();
      const month = date.toLocaleString('default', { month: 'short' });
      const day = date.getDate();
      return {
        title: item.taskEvent.name,
        start: `${year}-${month}-${day}`
        // DoctorsCount: item.DoctorsCount,
        // TotalAppointments: item.TotalAppointments,
        // Booked: item.Booked,
        // Cancelled: item.Cancelled
      };
    });
    return mappedData;
  }
}
