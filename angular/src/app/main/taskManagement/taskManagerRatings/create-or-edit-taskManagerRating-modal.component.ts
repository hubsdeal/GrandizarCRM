import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TaskManagerRatingsServiceProxy, CreateOrEditTaskManagerRatingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { TaskManagerRatingTaskEventLookupTableModalComponent } from './taskManagerRating-taskEvent-lookup-table-modal.component';
import { TaskManagerRatingEmployeeLookupTableModalComponent } from './taskManagerRating-employee-lookup-table-modal.component';
import { TaskManagerRatingTaskTeamLookupTableModalComponent } from './taskManagerRating-taskTeam-lookup-table-modal.component';
import { TaskManagerRatingRatingLikeLookupTableModalComponent } from './taskManagerRating-ratingLike-lookup-table-modal.component';



@Component({
    selector: 'createOrEditTaskManagerRatingModal',
    templateUrl: './create-or-edit-taskManagerRating-modal.component.html'
})
export class CreateOrEditTaskManagerRatingModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('taskManagerRatingTaskEventLookupTableModal', { static: true }) taskManagerRatingTaskEventLookupTableModal: TaskManagerRatingTaskEventLookupTableModalComponent;
    @ViewChild('taskManagerRatingEmployeeLookupTableModal', { static: true }) taskManagerRatingEmployeeLookupTableModal: TaskManagerRatingEmployeeLookupTableModalComponent;
    @ViewChild('taskManagerRatingTaskTeamLookupTableModal', { static: true }) taskManagerRatingTaskTeamLookupTableModal: TaskManagerRatingTaskTeamLookupTableModalComponent;
    @ViewChild('taskManagerRatingRatingLikeLookupTableModal', { static: true }) taskManagerRatingRatingLikeLookupTableModal: TaskManagerRatingRatingLikeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    taskManagerRating: CreateOrEditTaskManagerRatingDto = new CreateOrEditTaskManagerRatingDto();

    taskEventName = '';
    employeeName = '';
    taskTeamStartTime = '';
    ratingLikeName = '';



    constructor(
        injector: Injector,
        private _taskManagerRatingsServiceProxy: TaskManagerRatingsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(taskManagerRatingId?: number): void {
    

        if (!taskManagerRatingId) {
            this.taskManagerRating = new CreateOrEditTaskManagerRatingDto();
            this.taskManagerRating.id = taskManagerRatingId;
            this.taskEventName = '';
            this.employeeName = '';
            this.taskTeamStartTime = '';
            this.ratingLikeName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._taskManagerRatingsServiceProxy.getTaskManagerRatingForEdit(taskManagerRatingId).subscribe(result => {
                this.taskManagerRating = result.taskManagerRating;

                this.taskEventName = result.taskEventName;
                this.employeeName = result.employeeName;
                this.taskTeamStartTime = result.taskTeamStartTime;
                this.ratingLikeName = result.ratingLikeName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._taskManagerRatingsServiceProxy.createOrEdit(this.taskManagerRating)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectTaskEventModal() {
        this.taskManagerRatingTaskEventLookupTableModal.id = this.taskManagerRating.taskEventId;
        this.taskManagerRatingTaskEventLookupTableModal.displayName = this.taskEventName;
        this.taskManagerRatingTaskEventLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.taskManagerRatingEmployeeLookupTableModal.id = this.taskManagerRating.managerEmployeeId;
        this.taskManagerRatingEmployeeLookupTableModal.displayName = this.employeeName;
        this.taskManagerRatingEmployeeLookupTableModal.show();
    }
    openSelectTaskTeamModal() {
        this.taskManagerRatingTaskTeamLookupTableModal.id = this.taskManagerRating.taskTeamId;
        this.taskManagerRatingTaskTeamLookupTableModal.displayName = this.taskTeamStartTime;
        this.taskManagerRatingTaskTeamLookupTableModal.show();
    }
    openSelectRatingLikeModal() {
        this.taskManagerRatingRatingLikeLookupTableModal.id = this.taskManagerRating.ratingLikeId;
        this.taskManagerRatingRatingLikeLookupTableModal.displayName = this.ratingLikeName;
        this.taskManagerRatingRatingLikeLookupTableModal.show();
    }


    setTaskEventIdNull() {
        this.taskManagerRating.taskEventId = null;
        this.taskEventName = '';
    }
    setManagerEmployeeIdNull() {
        this.taskManagerRating.managerEmployeeId = null;
        this.employeeName = '';
    }
    setTaskTeamIdNull() {
        this.taskManagerRating.taskTeamId = null;
        this.taskTeamStartTime = '';
    }
    setRatingLikeIdNull() {
        this.taskManagerRating.ratingLikeId = null;
        this.ratingLikeName = '';
    }


    getNewTaskEventId() {
        this.taskManagerRating.taskEventId = this.taskManagerRatingTaskEventLookupTableModal.id;
        this.taskEventName = this.taskManagerRatingTaskEventLookupTableModal.displayName;
    }
    getNewManagerEmployeeId() {
        this.taskManagerRating.managerEmployeeId = this.taskManagerRatingEmployeeLookupTableModal.id;
        this.employeeName = this.taskManagerRatingEmployeeLookupTableModal.displayName;
    }
    getNewTaskTeamId() {
        this.taskManagerRating.taskTeamId = this.taskManagerRatingTaskTeamLookupTableModal.id;
        this.taskTeamStartTime = this.taskManagerRatingTaskTeamLookupTableModal.displayName;
    }
    getNewRatingLikeId() {
        this.taskManagerRating.ratingLikeId = this.taskManagerRatingRatingLikeLookupTableModal.id;
        this.ratingLikeName = this.taskManagerRatingRatingLikeLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
