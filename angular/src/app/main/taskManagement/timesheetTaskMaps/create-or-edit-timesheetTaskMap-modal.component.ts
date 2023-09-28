import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TimesheetTaskMapsServiceProxy, CreateOrEditTimesheetTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { TimesheetTaskMapTimesheetLookupTableModalComponent } from './timesheetTaskMap-timesheet-lookup-table-modal.component';
import { TimesheetTaskMapTaskEventLookupTableModalComponent } from './timesheetTaskMap-taskEvent-lookup-table-modal.component';



@Component({
    selector: 'createOrEditTimesheetTaskMapModal',
    templateUrl: './create-or-edit-timesheetTaskMap-modal.component.html'
})
export class CreateOrEditTimesheetTaskMapModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('timesheetTaskMapTimesheetLookupTableModal', { static: true }) timesheetTaskMapTimesheetLookupTableModal: TimesheetTaskMapTimesheetLookupTableModalComponent;
    @ViewChild('timesheetTaskMapTaskEventLookupTableModal', { static: true }) timesheetTaskMapTaskEventLookupTableModal: TimesheetTaskMapTaskEventLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    timesheetTaskMap: CreateOrEditTimesheetTaskMapDto = new CreateOrEditTimesheetTaskMapDto();

    timesheetStartTime = '';
    taskEventName = '';



    constructor(
        injector: Injector,
        private _timesheetTaskMapsServiceProxy: TimesheetTaskMapsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(timesheetTaskMapId?: number): void {
    

        if (!timesheetTaskMapId) {
            this.timesheetTaskMap = new CreateOrEditTimesheetTaskMapDto();
            this.timesheetTaskMap.id = timesheetTaskMapId;
            this.timesheetStartTime = '';
            this.taskEventName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._timesheetTaskMapsServiceProxy.getTimesheetTaskMapForEdit(timesheetTaskMapId).subscribe(result => {
                this.timesheetTaskMap = result.timesheetTaskMap;

                this.timesheetStartTime = result.timesheetStartTime;
                this.taskEventName = result.taskEventName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._timesheetTaskMapsServiceProxy.createOrEdit(this.timesheetTaskMap)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectTimesheetModal() {
        this.timesheetTaskMapTimesheetLookupTableModal.id = this.timesheetTaskMap.timesheetId;
        this.timesheetTaskMapTimesheetLookupTableModal.displayName = this.timesheetStartTime;
        this.timesheetTaskMapTimesheetLookupTableModal.show();
    }
    openSelectTaskEventModal() {
        this.timesheetTaskMapTaskEventLookupTableModal.id = this.timesheetTaskMap.taskEventId;
        this.timesheetTaskMapTaskEventLookupTableModal.displayName = this.taskEventName;
        this.timesheetTaskMapTaskEventLookupTableModal.show();
    }


    setTimesheetIdNull() {
        this.timesheetTaskMap.timesheetId = null;
        this.timesheetStartTime = '';
    }
    setTaskEventIdNull() {
        this.timesheetTaskMap.taskEventId = null;
        this.taskEventName = '';
    }


    getNewTimesheetId() {
        this.timesheetTaskMap.timesheetId = this.timesheetTaskMapTimesheetLookupTableModal.id;
        this.timesheetStartTime = this.timesheetTaskMapTimesheetLookupTableModal.displayName;
    }
    getNewTaskEventId() {
        this.timesheetTaskMap.taskEventId = this.timesheetTaskMapTaskEventLookupTableModal.id;
        this.taskEventName = this.timesheetTaskMapTaskEventLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
