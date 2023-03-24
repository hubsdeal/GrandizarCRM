import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    TaskEventsServiceProxy,
    CreateOrEditTaskEventDto,
    TaskEventTaskStatusLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditTaskEventModal',
    templateUrl: './create-or-edit-taskEvent-modal.component.html',
})
export class CreateOrEditTaskEventModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    taskEvent: CreateOrEditTaskEventDto = new CreateOrEditTaskEventDto();

    taskStatusName = '';

    allTaskStatuss: TaskEventTaskStatusLookupTableDto[];

    constructor(
        injector: Injector,
        private _taskEventsServiceProxy: TaskEventsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(taskEventId?: number): void {
        if (!taskEventId) {
            this.taskEvent = new CreateOrEditTaskEventDto();
            this.taskEvent.id = taskEventId;
            this.taskEvent.eventDate = this._dateTimeService.getStartOfDay();
            this.taskEvent.endDate = this._dateTimeService.getStartOfDay();
            this.taskStatusName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._taskEventsServiceProxy.getTaskEventForEdit(taskEventId).subscribe((result) => {
                this.taskEvent = result.taskEvent;

                this.taskStatusName = result.taskStatusName;

                this.active = true;
                this.modal.show();
            });
        }
        this._taskEventsServiceProxy.getAllTaskStatusForTableDropdown().subscribe((result) => {
            this.allTaskStatuss = result;
        });
    }

    save(): void {
        this.saving = true;

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

    ngOnInit(): void {}
}
