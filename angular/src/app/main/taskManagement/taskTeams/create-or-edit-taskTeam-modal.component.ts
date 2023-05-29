import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TaskTeamsServiceProxy, CreateOrEditTaskTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { TaskTeamTaskEventLookupTableModalComponent } from './taskTeam-taskEvent-lookup-table-modal.component';
import { TaskTeamEmployeeLookupTableModalComponent } from './taskTeam-employee-lookup-table-modal.component';
import { TaskTeamContactLookupTableModalComponent } from './taskTeam-contact-lookup-table-modal.component';

@Component({
    selector: 'createOrEditTaskTeamModal',
    templateUrl: './create-or-edit-taskTeam-modal.component.html',
})
export class CreateOrEditTaskTeamModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('taskTeamTaskEventLookupTableModal', { static: true })
    taskTeamTaskEventLookupTableModal: TaskTeamTaskEventLookupTableModalComponent;
    @ViewChild('taskTeamEmployeeLookupTableModal', { static: true })
    taskTeamEmployeeLookupTableModal: TaskTeamEmployeeLookupTableModalComponent;
    @ViewChild('taskTeamContactLookupTableModal', { static: true })
    taskTeamContactLookupTableModal: TaskTeamContactLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    taskTeam: CreateOrEditTaskTeamDto = new CreateOrEditTaskTeamDto();

    taskEventName = '';
    employeeName = '';
    contactFullName = '';

    constructor(
        injector: Injector,
        private _taskTeamsServiceProxy: TaskTeamsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(taskTeamId?: number): void {
        if (!taskTeamId) {
            this.taskTeam = new CreateOrEditTaskTeamDto();
            this.taskTeam.id = taskTeamId;
            this.taskTeam.startDate = this._dateTimeService.getStartOfDay();
            this.taskTeam.endDate = this._dateTimeService.getStartOfDay();
            this.taskEventName = '';
            this.employeeName = '';
            this.contactFullName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._taskTeamsServiceProxy.getTaskTeamForEdit(taskTeamId).subscribe((result) => {
                this.taskTeam = result.taskTeam;

                this.taskEventName = result.taskEventName;
                this.employeeName = result.employeeName;
                this.contactFullName = result.contactFullName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._taskTeamsServiceProxy
            .createOrEdit(this.taskTeam)
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

    openSelectTaskEventModal() {
        this.taskTeamTaskEventLookupTableModal.id = this.taskTeam.taskEventId;
        this.taskTeamTaskEventLookupTableModal.displayName = this.taskEventName;
        this.taskTeamTaskEventLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.taskTeamEmployeeLookupTableModal.id = this.taskTeam.employeeId;
        this.taskTeamEmployeeLookupTableModal.displayName = this.employeeName;
        this.taskTeamEmployeeLookupTableModal.show();
    }
    openSelectContactModal() {
        this.taskTeamContactLookupTableModal.id = this.taskTeam.contactId;
        this.taskTeamContactLookupTableModal.displayName = this.contactFullName;
        this.taskTeamContactLookupTableModal.show();
    }

    setTaskEventIdNull() {
        this.taskTeam.taskEventId = null;
        this.taskEventName = '';
    }
    setEmployeeIdNull() {
        this.taskTeam.employeeId = null;
        this.employeeName = '';
    }
    setContactIdNull() {
        this.taskTeam.contactId = null;
        this.contactFullName = '';
    }

    getNewTaskEventId() {
        this.taskTeam.taskEventId = this.taskTeamTaskEventLookupTableModal.id;
        this.taskEventName = this.taskTeamTaskEventLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.taskTeam.employeeId = this.taskTeamEmployeeLookupTableModal.id;
        this.employeeName = this.taskTeamEmployeeLookupTableModal.displayName;
    }
    getNewContactId() {
        this.taskTeam.contactId = this.taskTeamContactLookupTableModal.id;
        this.contactFullName = this.taskTeamContactLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
