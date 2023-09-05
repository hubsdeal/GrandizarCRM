import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef, Input } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TaskWorkItemsServiceProxy, CreateOrEditTaskWorkItemDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { TaskWorkItemTaskEventLookupTableModalComponent } from './taskWorkItem-taskEvent-lookup-table-modal.component';
import { TaskWorkItemEmployeeLookupTableModalComponent } from './taskWorkItem-employee-lookup-table-modal.component';
import { SelectItem } from 'primeng/api';

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
    taskEventId: number;
    taskWorkItem: CreateOrEditTaskWorkItemDto = new CreateOrEditTaskWorkItemDto();
    OpenOrClosedOptions:any;
    taskEventName = '';
    employeeName = '';
    //taskEventId: number;
    statusOptions: SelectItem[];
    status: string = 'incomplete';
    lineItems: string[] = [];
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
            this.taskEventName = '';
            this.employeeName = '';
            this.taskWorkItem.startDate = this._dateTimeService.getStartOfDay();
            this.taskWorkItem.endDate = this._dateTimeService.getStartOfDay();
            //this.status = 'incomplete';
            this.taskWorkItem.openOrClosed = false;
            this.active = true;
            this.modal.show();
        } else {
            this._taskWorkItemsServiceProxy.getTaskWorkItemForEdit(taskWorkItemId).subscribe(result => {
                this.taskWorkItem = result.taskWorkItem;
                if (this.taskEventId) {
                    this.taskEventId = result.taskWorkItem.taskEventId;
                }
                //this.status = result.taskWorkItem.openOrClosed ? 'completed' : 'incomplete';
                this.taskEventName = result.taskEventName;
                this.employeeName = result.employeeName;

                this.taskWorkItem.openOrClosed=result.taskWorkItem.openOrClosed;
                this.active = true;
                this.modal.show();
            });
        }
        this.OpenOrClosedOptions = [{ label: 'Open', value: false }, { label: 'Closed', value: true }];
    }

    // save(): void {
    //     this.saving = true;
    //     this.taskWorkItem.taskEventId = this.taskEventId;
    //     this._taskWorkItemsServiceProxy
    //         .createOrEdit(this.taskWorkItem)
    //         .pipe(
    //             finalize(() => {
    //                 this.saving = false;
    //             })
    //         )
    //         .subscribe(() => {
    //             this.notify.info(this.l('SavedSuccessfully'));
    //             this.close();
    //             this.modalSave.emit(null);
    //         });
    // }

    save(): void {
        this.saving = true;
        if (this.taskEventId) {
            this.taskWorkItem.taskEventId = this.taskEventId;
        }
        this.taskWorkItem.openOrClosed = this.status === 'completed' ? true : false;
        if (this.lineItems.length > 1) {
            this.message.confirm(
                `You have entered ${this.lineItems.length} items.`,
                this.l('Do you want to save multiple tasks items?'),
                (isConfirmed) => {
                    if (isConfirmed) {
                        for (let i = 0; i < this.lineItems.length; i++) {
                            const value = this.lineItems[i];
                            this.taskWorkItem.name = value;
                            this._taskWorkItemsServiceProxy.createOrEdit(this.taskWorkItem)
                                .pipe(finalize(() => { this.saving = false; }))
                                .subscribe(() => {
                                    this.notify.info(this.l('SavedSuccessfully'));
                                    this.lineItems = [];
                                    this.close();
                                    this.modalSave.emit(null);
                                });
                        }
                    } else {
                        this._taskWorkItemsServiceProxy.createOrEdit(this.taskWorkItem)
                            .pipe(finalize(() => { this.saving = false; }))
                            .subscribe(() => {
                                this.notify.info(this.l('SavedSuccessfully'));
                                this.lineItems = [];
                                this.close();
                                this.modalSave.emit(null);
                            });
                    }
                }
            );
        } else {
            this._taskWorkItemsServiceProxy.createOrEdit(this.taskWorkItem)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.lineItems = [];
                    this.close();
                    this.modalSave.emit(null);
                });
        }


        // if (this.lineItems.length > 0) {
        //     for (let i = 0; i < this.lineItems.length; i++) {
        //         const value = this.lineItems[i];
        //         this.taskWorkItem.name = value;
        //         this._taskWorkItemsServiceProxy.createOrEdit(this.taskWorkItem)
        //         .pipe(finalize(() => { this.saving = false; }))
        //         .subscribe(() => {
        //             this.notify.info(this.l('SavedSuccessfully'));
        //             this.close();
        //             this.modalSave.emit(null);
        //         });
        //     }
        // } else {

        // }
    }

    onInput(event: any) {
        const value = event.target.value;
        this.lineItems = value.split('\n');
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
    startTimeValue(value: any) {
        this.taskWorkItem.startTime = value;
    }

    endTimeValue(value: any) {
        this.taskWorkItem.endTime = value;
    }
}
