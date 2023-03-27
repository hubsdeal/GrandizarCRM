import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { LeadTasksServiceProxy, CreateOrEditLeadTaskDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { LeadTaskLeadLookupTableModalComponent } from './leadTask-lead-lookup-table-modal.component';
import { LeadTaskTaskEventLookupTableModalComponent } from './leadTask-taskEvent-lookup-table-modal.component';

@Component({
    selector: 'createOrEditLeadTaskModal',
    templateUrl: './create-or-edit-leadTask-modal.component.html',
})
export class CreateOrEditLeadTaskModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('leadTaskLeadLookupTableModal', { static: true })
    leadTaskLeadLookupTableModal: LeadTaskLeadLookupTableModalComponent;
    @ViewChild('leadTaskTaskEventLookupTableModal', { static: true })
    leadTaskTaskEventLookupTableModal: LeadTaskTaskEventLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    leadTask: CreateOrEditLeadTaskDto = new CreateOrEditLeadTaskDto();

    leadTitle = '';
    taskEventName = '';

    constructor(
        injector: Injector,
        private _leadTasksServiceProxy: LeadTasksServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(leadTaskId?: number): void {
        if (!leadTaskId) {
            this.leadTask = new CreateOrEditLeadTaskDto();
            this.leadTask.id = leadTaskId;
            this.leadTitle = '';
            this.taskEventName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._leadTasksServiceProxy.getLeadTaskForEdit(leadTaskId).subscribe((result) => {
                this.leadTask = result.leadTask;

                this.leadTitle = result.leadTitle;
                this.taskEventName = result.taskEventName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._leadTasksServiceProxy
            .createOrEdit(this.leadTask)
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

    openSelectLeadModal() {
        this.leadTaskLeadLookupTableModal.id = this.leadTask.leadId;
        this.leadTaskLeadLookupTableModal.displayName = this.leadTitle;
        this.leadTaskLeadLookupTableModal.show();
    }
    openSelectTaskEventModal() {
        this.leadTaskTaskEventLookupTableModal.id = this.leadTask.taskEventId;
        this.leadTaskTaskEventLookupTableModal.displayName = this.taskEventName;
        this.leadTaskTaskEventLookupTableModal.show();
    }

    setLeadIdNull() {
        this.leadTask.leadId = null;
        this.leadTitle = '';
    }
    setTaskEventIdNull() {
        this.leadTask.taskEventId = null;
        this.taskEventName = '';
    }

    getNewLeadId() {
        this.leadTask.leadId = this.leadTaskLeadLookupTableModal.id;
        this.leadTitle = this.leadTaskLeadLookupTableModal.displayName;
    }
    getNewTaskEventId() {
        this.leadTask.taskEventId = this.leadTaskTaskEventLookupTableModal.id;
        this.taskEventName = this.leadTaskTaskEventLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
