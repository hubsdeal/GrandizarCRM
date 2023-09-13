import { Component, OnInit, ViewChild } from '@angular/core';
import { CalendarComponent } from 'ng-fullcalendar';
import { Options } from 'fullcalendar';
import { EventService } from './event.service';

@Component({
  selector: 'app-task-schedule-calendar',
  templateUrl: './task-schedule-calendar.component.html',
  styleUrls: ['./task-schedule-calendar.component.css']
})
export class TaskScheduleCalendarComponent implements OnInit  {
  calendarOptions: Options;
 displayEvent: any;
  @ViewChild(CalendarComponent) ucCalendar: CalendarComponent;
  constructor(protected eventService: EventService) { }
  ngOnInit() { 
    this.eventService.getEvents().subscribe(data => {
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
        allDay: model.event.allDay,
        DoctorsCount: model.event.DoctorsCount,
        TotalAppointments: model.event.TotalAppointments,
        Booked: model.event.Booked,
        Canceled: model.event.Canceled
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
      <li>DoctorsCount: ${e.event.DoctorsCount}</li>
      <li>TotalAppointments: ${e.event.TotalAppointments}</li>
      <li>Booked: ${e.event.Booked}</li>
       <li>Cancelled: ${e.event.Cancelled}</li>
    </ul>`;
    e.element.html(html)
  }
}
