import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TaskNotesServiceProxy, CreateOrEditTaskNoteDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { TaskNoteTaskEventLookupTableModalComponent } from './taskNote-taskEvent-lookup-table-modal.component';



@Component({
    selector: 'createOrEditTaskNoteModal',
    templateUrl: './create-or-edit-taskNote-modal.component.html'
})
export class CreateOrEditTaskNoteModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('taskNoteTaskEventLookupTableModal', { static: true }) taskNoteTaskEventLookupTableModal: TaskNoteTaskEventLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    taskId:number;
    active = false;
    saving = false;

    taskNote: CreateOrEditTaskNoteDto = new CreateOrEditTaskNoteDto();

    taskEventName = '';



    constructor(
        injector: Injector,
        private _taskNotesServiceProxy: TaskNotesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(taskNoteId?: number): void {
    
        this.taskNote.taskEventId = this.taskId;
        if (!taskNoteId) {
            this.taskNote = new CreateOrEditTaskNoteDto();
            this.taskNote.id = taskNoteId;
            this.taskEventName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._taskNotesServiceProxy.getTaskNoteForEdit(taskNoteId).subscribe(result => {
                this.taskNote = result.taskNote;
                this.taskNote.taskEventId= this.taskId;
                this.taskEventName = result.taskEventName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			this.taskNote.taskEventId = this.taskId;
            this._taskNotesServiceProxy.createOrEdit(this.taskNote)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectTaskEventModal() {
        this.taskNoteTaskEventLookupTableModal.id = this.taskNote.taskEventId;
        this.taskNoteTaskEventLookupTableModal.displayName = this.taskEventName;
        this.taskNoteTaskEventLookupTableModal.show();
    }


    setTaskEventIdNull() {
        this.taskNote.taskEventId = null;
        this.taskEventName = '';
    }


    getNewTaskEventId() {
        this.taskNote.taskEventId = this.taskNoteTaskEventLookupTableModal.id;
        this.taskEventName = this.taskNoteTaskEventLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
