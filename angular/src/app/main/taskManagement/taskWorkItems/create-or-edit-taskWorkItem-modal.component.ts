import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TaskWorkItemsServiceProxy, CreateOrEditTaskWorkItemDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { TaskWorkItemTaskEventLookupTableModalComponent } from './taskWorkItem-taskEvent-lookup-table-modal.component';
import { TaskWorkItemEmployeeLookupTableModalComponent } from './taskWorkItem-employee-lookup-table-modal.component';

@Component({
    selector: 'createOrEditTaskWorkItemModal',
    templateUrl: './create-or-edit-taskWorkItem-modal.component.html',
})
export class CreateOrEditTaskWorkItemModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('taskWorkItemTaskEventLookupTableModal', { static: true })
    taskWorkItemTaskEventLookupTableModal: TaskWorkItemTaskEventLookupTableModalComponent;
    @ViewChild('taskWorkItemEmployeeLookupTableModal', { static: true })
    taskWorkItemEmployeeLookupTableModal: TaskWorkItemEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    taskWorkItem: CreateOrEditTaskWorkItemDto = new CreateOrEditTaskWorkItemDto();

    taskEventName = '';
    employeeName = '';

    constructor(
        injector: Injector,
        private _taskWorkItemsServiceProxy: TaskWorkItemsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(taskWorkItemId?: number): void {
        if (!taskWorkItemId) {
            this.taskWorkItem = new CreateOrEditTaskWorkItemDto();
            this.taskWorkItem.id = taskWorkItemId;
            this.taskWorkItem.startDate = this._dateTimeService.getStartOfDay();
            this.taskWorkItem.endDate = this._dateTimeService.getStartOfDay();
            this.taskEventName = '';
            this.employeeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._taskWorkItemsServiceProxy.getTaskWorkItemForEdit(taskWorkItemId).subscribe((result) => {
                this.taskWorkItem = result.taskWorkItem;

                this.taskEventName = result.taskEventName;
                this.employeeName = result.employeeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._taskWorkItemsServiceProxy
            .createOrEdit(this.taskWorkItem)
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
        this.taskWorkItemTaskEventLookupTableModal.id = this.taskWorkItem.taskEventId;
        this.taskWorkItemTaskEventLookupTableModal.displayName = this.taskEventName;
        this.taskWorkItemTaskEventLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.taskWorkItemEmployeeLookupTableModal.id = this.taskWorkItem.employeeId;
        this.taskWorkItemEmployeeLookupTableModal.displayName = this.employeeName;
        this.taskWorkItemEmployeeLookupTableModal.show();
    }

    setTaskEventIdNull() {
        this.taskWorkItem.taskEventId = null;
        this.taskEventName = '';
    }
    setEmployeeIdNull() {
        this.taskWorkItem.employeeId = null;
        this.employeeName = '';
    }

    getNewTaskEventId() {
        this.taskWorkItem.taskEventId = this.taskWorkItemTaskEventLookupTableModal.id;
        this.taskEventName = this.taskWorkItemTaskEventLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.taskWorkItem.employeeId = this.taskWorkItemEmployeeLookupTableModal.id;
        this.employeeName = this.taskWorkItemEmployeeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
