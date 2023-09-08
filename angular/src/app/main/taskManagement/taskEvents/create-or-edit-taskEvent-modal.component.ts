﻿import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    TaskEventsServiceProxy,
    CreateOrEditTaskEventDto,
    TaskEventTaskStatusLookupTableDto,
    TaskTeamEmployeeLookupTableDto,
    TaskTeamsServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { SelectItem } from 'primeng/api';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { TaskEventsLookupTableModalComponent } from './task-events-lookup-table-modal/task-events-lookup-table-modal.component';


@Component({
    selector: 'createOrEditTaskEventModal',
    templateUrl: './create-or-edit-taskEvent-modal.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class CreateOrEditTaskEventModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    @ViewChild('taskEventLookupTableModal', { static: true })
    taskEventLookupTableModal: TaskEventsLookupTableModalComponent;

    active = false;
    saving = false;

    taskEvent: CreateOrEditTaskEventDto = new CreateOrEditTaskEventDto();

    taskStatusName = '';
    isFromTaskLibrary:boolean=false
    allTaskStatuss: TaskEventTaskStatusLookupTableDto[];

    taskStatusOptions: SelectItem[];
    priorityOptions: SelectItem[];

    selectedTemplate:any;
    allTemplate:any[]=[]
    //[{id:1,displayName:"template 1"},{id:2,displayName:"template 2"},{id:3,displayName:"template 3"}]

    selectedTeam:any;
    allTeams:any[];
    //=[{id:1,displayName:"Team 1"},{id:2,displayName:"Team 2"},{id:3,displayName:"Team 3"}]

    selectedTag:any;
    allTags:any[]=[{id:1,displayName:"Tag 1"},{id:2,displayName:"Tag 2"},{id:3,displayName:"Tag 3"}]

    employeeList: TaskTeamEmployeeLookupTableDto[] = [];
    selectedEmployees: TaskTeamEmployeeLookupTableDto[] = [];

    taskEventId:number;
    taskEventName:string;

    constructor(
        injector: Injector,
        private _taskEventsServiceProxy: TaskEventsServiceProxy,
        private _taskTeamsServiceProxy: TaskTeamsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(taskEventId?: number): void {
        this._taskTeamsServiceProxy.getAllEmployeeForLookupTable('','',0,1000).subscribe(result => {
            this.employeeList = result.items;
        });
        if (!taskEventId) {
            this.taskEvent = new CreateOrEditTaskEventDto();
            this.taskEvent.id = taskEventId;
            this.taskEvent.eventDate = this._dateTimeService.getStartOfDay();
            this.taskEvent.endDate = this._dateTimeService.getStartOfDay();
            this.taskStatusName = '';
            this.taskEvent.completionPercentage =0;
            //this.taskEvent.status = false;
            this.taskEvent.priority = false;
            this.active = true;
            this.taskEvent.template=this.isFromTaskLibrary;
            this.modal.show();
        } else {
            this._taskEventsServiceProxy.getTaskEventForEdit(taskEventId).subscribe((result) => {
                this.taskEvent = result.taskEvent;
                this.taskEventId = result.taskEvent.dependentTaskEventId;
                this.taskStatusName = result.taskStatusName;
                this.taskEvent.priority = result.taskEvent.priority;
                this.active = true;
                this.modal.show();
            });
        }
        this._taskEventsServiceProxy.getAllTaskStatusForTableDropdown().subscribe((result) => {
            this.allTaskStatuss = result;
        });
        this.taskStatusOptions = [{ label: 'Completed', value: true }, { label: 'Open', value: false }];
        this.priorityOptions = [{ label: 'High', value: true }, { label: 'Low', value: false }];
        this.getTempleteList()
    }

    save(): void {
        this.saving = true;
        this.taskEvent.dependentTaskEventId = this.taskEventId;
        this._taskEventsServiceProxy
            .createOrEdit(this.taskEvent)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

   
    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {

        //this.allTemplate
    }

    startTimeValue(value: any) {
        this.taskEvent.startTime = value;
    }

    endTimeValue(value: any) {
        this.taskEvent.endTime = value;
    }
    getTempleteList() {
        this._taskEventsServiceProxy.getAllTaskTemplateForDropdown().subscribe(result=>{
            this.allTemplate = result;
       });
    }
    getTeamList() {
        this._taskTeamsServiceProxy.getTaskTeamForDropdown().subscribe(result=>{
            this.allTeams = result;
       });
    }
    onTemplateSelect(event) {
        // console.log(event);
        // debugger
        this._taskEventsServiceProxy.getTaskEventForEdit(event.value.id).subscribe(result => {
            this.taskEvent = result.taskEvent;
            this.taskEvent.template = false;
            this.taskEvent.id = null;
        });
    }
    onEmployeeSelect(event: any) {
        if (event) {
            let index =this.taskEvent.teams?this.taskEvent.teams.findIndex(x => x.id == event.itemValue.id):-1;
            if(index<0){
                this.taskEvent.teams = event.value;
            }else if(index>=0 && this.taskEvent.id){
                // this._taskTeamsServiceProxy.deleteByTask(this.taskEvent.id,event.itemValue.id).subscribe(result=>{
                //     this.taskEvent.teams.splice(index, 1);
                // });
            }
        }

        // console.log(event);
        // if (event.value.length > 0) {

        // }
    }
    saveAsLibararyEvent(){
        this.taskEvent.template = !this.taskEvent.template
    }
    getcompletionPercentage(event){
        console.log(event);
    }

    getNewTaskEventId() {
        this.taskEventId = this.taskEventLookupTableModal.id;
        this.taskEventName = this.taskEventLookupTableModal.displayName;
    }

    openSelectTaskEventModal() {
        this.taskEventLookupTableModal.id = this.taskEventId;
        this.taskEventLookupTableModal.displayName = this.taskEventName;
        this.taskEventLookupTableModal.show();
    }

    setTaskEventIdNull() {
        this.taskEventId = null;
        this.taskEventName = '';
    }
}
