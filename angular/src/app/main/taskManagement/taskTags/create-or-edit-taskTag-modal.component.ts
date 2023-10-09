import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TaskTagsServiceProxy, CreateOrEditTaskTagDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { TaskTagTaskEventLookupTableModalComponent } from './taskTag-taskEvent-lookup-table-modal.component';
import { TaskTagMasterTagCategoryLookupTableModalComponent } from './taskTag-masterTagCategory-lookup-table-modal.component';
import { TaskTagMasterTagLookupTableModalComponent } from './taskTag-masterTag-lookup-table-modal.component';
import { SelectItem } from 'primeng/api';

@Component({
    selector: 'createOrEditTaskTagModal',
    templateUrl: './create-or-edit-taskTag-modal.component.html',
})
export class CreateOrEditTaskTagModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('taskTagTaskEventLookupTableModal', { static: true })
    taskTagTaskEventLookupTableModal: TaskTagTaskEventLookupTableModalComponent;
    @ViewChild('taskTagMasterTagCategoryLookupTableModal', { static: true })
    taskTagMasterTagCategoryLookupTableModal: TaskTagMasterTagCategoryLookupTableModalComponent;
    @ViewChild('taskTagMasterTagLookupTableModal', { static: true })
    taskTagMasterTagLookupTableModal: TaskTagMasterTagLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    taskTag: CreateOrEditTaskTagDto = new CreateOrEditTaskTagDto();

    taskEventName = '';
    masterTagCategoryName = '';
    masterTagName = '';
    masterTags:any;
    masterTagCategory:any;
    verifyOptions: SelectItem[];
    seletedTagCategory:any;
    selctedTag:any;
    constructor(
        injector: Injector,
        private _taskTagsServiceProxy: TaskTagsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(taskTagId?: number): void {
        if (!taskTagId) {
            this.taskTag = new CreateOrEditTaskTagDto();
            this.taskTag.id = taskTagId;
            this.taskEventName = '';
            this.masterTagCategoryName = '';
            this.masterTagName = '';
            this.taskTag.verfied=true;
            this.active = true;
            this.modal.show();
        } else {
            this._taskTagsServiceProxy.getTaskTagForEdit(taskTagId).subscribe((result) => {
                this.taskTag = result.taskTag;

                this.taskEventName = result.taskEventName;
                this.masterTagCategoryName = result.masterTagCategoryName;
                this.masterTagName = result.masterTagName;

                this.active = true;
                this.modal.show();
            });
        }
        this.verifyOptions = [{ label: 'Verified', value: true }, { label: 'Not Verified', value: false }];
    }
    showWithTaskId(taskEcventId?: number): void {

            this.taskTag = new CreateOrEditTaskTagDto();
           // this.taskTag.id = taskTagId;
            this.taskEventName = '';
            this.masterTagCategoryName = '';
            this.masterTagName = '';
            this.taskTag.taskEventId = taskEcventId;
            this.taskTag.verfied=true;
            this.verifyOptions = [{ label: 'Verified', value: true }, { label: 'Not Verified', value: false }];
            this.active = true;
            this.modal.show();
            this.getAllMasterTag(taskEcventId);
            this.getAllMasterTagCategory();
        
    }
    getAllMasterTag(taskEcventId) {


        this._taskTagsServiceProxy
            .getAllMasterTagForLookupTable(
                taskEcventId,
                '',
                '',
                0,
                2000
            )
            .subscribe((result) => {

                this.masterTags = result.items;
            });

            //this._taskTagsServiceProxy.g
    }
    getAllMasterTagCategory() {
        this._taskTagsServiceProxy
            .getAllMasterTagCategoryForLookupTable(
                '',
                '',
                0,
                2000
            )
            .subscribe((result) => {
                //this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.masterTagCategory = result.items;
                //this.primengTableHelper.hideLoadingIndicator();
            });
    }
    save(): void {
        this.saving = true;
        debugger
        this.taskTag.masterTagId = this.selctedTag.id;
        this.taskTag.masterTagCategoryId = this.seletedTagCategory.id;
        this._taskTagsServiceProxy
            .createOrEdit(this.taskTag)
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
        this.taskTagTaskEventLookupTableModal.id = this.taskTag.taskEventId;
        this.taskTagTaskEventLookupTableModal.displayName = this.taskEventName;
        this.taskTagTaskEventLookupTableModal.show();
    }
    openSelectMasterTagCategoryModal() {
        this.taskTagMasterTagCategoryLookupTableModal.id = this.taskTag.masterTagCategoryId;
        this.taskTagMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.taskTagMasterTagCategoryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.taskTagMasterTagLookupTableModal.id = this.taskTag.masterTagId;
        this.taskTagMasterTagLookupTableModal.displayName = this.masterTagName;
        this.taskTagMasterTagLookupTableModal.show();
    }

    setTaskEventIdNull() {
        this.taskTag.taskEventId = null;
        this.taskEventName = '';
    }
    setMasterTagCategoryIdNull() {
        this.taskTag.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }
    setMasterTagIdNull() {
        this.taskTag.masterTagId = null;
        this.masterTagName = '';
    }

    getNewTaskEventId() {
        this.taskTag.taskEventId = this.taskTagTaskEventLookupTableModal.id;
        this.taskEventName = this.taskTagTaskEventLookupTableModal.displayName;
    }
    getNewMasterTagCategoryId() {
        this.taskTag.masterTagCategoryId = this.taskTagMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.taskTagMasterTagCategoryLookupTableModal.displayName;
    }
    getNewMasterTagId() {
        this.taskTag.masterTagId = this.taskTagMasterTagLookupTableModal.id;
        this.masterTagName = this.taskTagMasterTagLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
