import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreTaskMapsServiceProxy, CreateOrEditStoreTaskMapDto, CreateOrEditTaskEventDto, TaskEventTaskStatusLookupTableDto, TaskTeamEmployeeLookupTableDto, TaskTeamsServiceProxy, TaskEventsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreTaskMapStoreLookupTableModalComponent } from './storeTaskMap-store-lookup-table-modal.component';
import { StoreTaskMapTaskEventLookupTableModalComponent } from './storeTaskMap-taskEvent-lookup-table-modal.component';
import { SelectItem } from 'primeng/api';
import * as moment from 'moment';
import { TaskEventsLookupTableModalComponent } from '../taskEvents/task-events-lookup-table-modal/task-events-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreTaskMapModal',
    templateUrl: './create-or-edit-storeTaskMap-modal.component.html',
})
export class CreateOrEditStoreTaskMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeTaskMapStoreLookupTableModal', { static: true })
    storeTaskMapStoreLookupTableModal: StoreTaskMapStoreLookupTableModalComponent;
    @ViewChild('storeTaskMapTaskEventLookupTableModal', { static: true })
    storeTaskMapTaskEventLookupTableModal: StoreTaskMapTaskEventLookupTableModalComponent;

    @ViewChild('taskEventLookupTableModal', { static: true })
    taskEventLookupTableModal: TaskEventsLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeTaskMap: CreateOrEditStoreTaskMapDto = new CreateOrEditStoreTaskMapDto();

    storeName = '';
    storeId: number;

    taskEvent: CreateOrEditTaskEventDto = new CreateOrEditTaskEventDto();

    taskStatusName = '';
    isFromTaskLibrary: boolean = false
    allTaskStatuss: TaskEventTaskStatusLookupTableDto[];

    taskStatusOptions: SelectItem[];
    priorityOptions: SelectItem[];

    selectedTemplate: any;
    allTemplate: any[] = []
    //[{id:1,displayName:"template 1"},{id:2,displayName:"template 2"},{id:3,displayName:"template 3"}]

    selectedTeam: any;
    allTeams: any[];
    //=[{id:1,displayName:"Team 1"},{id:2,displayName:"Team 2"},{id:3,displayName:"Team 3"}]

    selectedTag: any;
    allTags: any[] = [{ id: 1, displayName: "Tag 1" }, { id: 2, displayName: "Tag 2" }, { id: 3, displayName: "Tag 3" }]

    employeeList: TaskTeamEmployeeLookupTableDto[] = [];
    selectedEmployees: TaskTeamEmployeeLookupTableDto[] = [];

    taskEventId: number;
    taskEventName: string;
    constructor(
        injector: Injector,
        private _storeTaskMapsServiceProxy: StoreTaskMapsServiceProxy,
        private _taskEventsServiceProxy: TaskEventsServiceProxy,
        private _taskTeamsServiceProxy: TaskTeamsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeTaskMapId?: number): void {
        if (!storeTaskMapId) {
            this.storeTaskMap = new CreateOrEditStoreTaskMapDto();
            this.storeTaskMap.id = storeTaskMapId;
            this.storeName = '';
            this.taskEventName = '';
            this.taskEvent.eventDate = this._dateTimeService.getStartOfDay();
            this.taskEvent.endDate = this._dateTimeService.getStartOfDay();
            this.taskEvent.endTime = moment().add(30, 'minutes').format('hh:mm A');
            this.taskEvent.startTime = moment().format('hh:mm A');
            this.taskEvent.completionPercentage = 0;
            //this.taskEvent.status = false;
            this.taskEvent.priority = false;
            this.active = true;
            this.taskEvent.template = this.isFromTaskLibrary;
            this.active = true;
            this.modal.show();
        } else {
            this._storeTaskMapsServiceProxy.getStoreTaskMapForEdit(storeTaskMapId).subscribe((result) => {
                this.storeTaskMap = result.storeTaskMap;
                this.storeId = result.storeTaskMap.storeId;
                this.storeName = result.storeName;
                this.taskEventName = result.taskEventName;

                this.active = true;
                this.modal.show();
            });
        }
        this._taskTeamsServiceProxy.getAllEmployeeForLookupTable('', '', 0, 1000).subscribe(result => {
            this.employeeList = result.items;
        });
        this.taskStatusOptions = [{ label: 'Completed', value: true }, { label: 'Open', value: false }];
        this.priorityOptions = [{ label: 'High', value: true }, { label: 'Low', value: false }];
        this.getTempleteList()
    }

    save(): void {
        // this.saving = true;
        // this.storeTaskMap.storeId = this.storeId;
        // this._storeTaskMapsServiceProxy
        //     .createOrEdit(this.storeTaskMap)
        //     .pipe(
        //         finalize(() => {
        //             this.saving = false;
        //         })
        //     )
        //     .subscribe(() => {
        //         this.notify.info(this.l('SavedSuccessfully'));
        //         this.close();
        //         this.modalSave.emit(null);
        //     });
        this.saving = true;


        this._taskEventsServiceProxy.createOrEdit(this.taskEvent)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe((r) => {
                this.storeTaskMap.storeId = this.storeId;
                this.storeTaskMap.taskEventId = r;
                this._storeTaskMapsServiceProxy.createOrEdit(this.storeTaskMap)
                    .pipe(finalize(() => { this.saving = false; }))
                    .subscribe(() => {
                        this.notify.info(this.l('SavedSuccessfully'));
                        this.close();
                        this.modalSave.emit(null);
                    });
            });
    }

    openSelectStoreModal() {
        this.storeTaskMapStoreLookupTableModal.id = this.storeTaskMap.storeId;
        this.storeTaskMapStoreLookupTableModal.displayName = this.storeName;
        this.storeTaskMapStoreLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeTaskMap.storeId = null;
        this.storeName = '';
    }


    getNewStoreId() {
        this.storeTaskMap.storeId = this.storeTaskMapStoreLookupTableModal.id;
        this.storeName = this.storeTaskMapStoreLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void { }


    startTimeValue(value: any) {
        this.taskEvent.startTime = value;
    }

    endTimeValue(value: any) {
        this.taskEvent.endTime = value;
    }
    getTempleteList() {
        this._taskEventsServiceProxy.getAllTaskTemplateForDropdown().subscribe(result => {
            this.allTemplate = result;
        });
    }
    getTeamList() {
        this._taskTeamsServiceProxy.getTaskTeamForDropdown().subscribe(result => {
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
            let index = this.taskEvent.teams ? this.taskEvent.teams.findIndex(x => x.id == event.itemValue.id) : -1;
            if (index < 0) {
                this.taskEvent.teams = event.value;
            } else if (index >= 0 && this.taskEvent.id) {
                // this._taskTeamsServiceProxy.deleteByTask(this.taskEvent.id,event.itemValue.id).subscribe(result=>{
                //     this.taskEvent.teams.splice(index, 1);
                // });
            }
        }

        // console.log(event);
        // if (event.value.length > 0) {

        // }
    }
    saveAsLibararyEvent() {
        this.taskEvent.template = !this.taskEvent.template
    }
    getcompletionPercentage(event) {
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
