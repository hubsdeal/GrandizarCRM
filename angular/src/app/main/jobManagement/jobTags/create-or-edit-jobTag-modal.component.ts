import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { JobTagsServiceProxy, CreateOrEditJobTagDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { JobTagJobLookupTableModalComponent } from './jobTag-job-lookup-table-modal.component';
import { JobTagMasterTagCategoryLookupTableModalComponent } from './jobTag-masterTagCategory-lookup-table-modal.component';
import { JobTagMasterTagLookupTableModalComponent } from './jobTag-masterTag-lookup-table-modal.component';

@Component({
    selector: 'createOrEditJobTagModal',
    templateUrl: './create-or-edit-jobTag-modal.component.html',
})
export class CreateOrEditJobTagModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('jobTagJobLookupTableModal', { static: true })
    jobTagJobLookupTableModal: JobTagJobLookupTableModalComponent;
    @ViewChild('jobTagMasterTagCategoryLookupTableModal', { static: true })
    jobTagMasterTagCategoryLookupTableModal: JobTagMasterTagCategoryLookupTableModalComponent;
    @ViewChild('jobTagMasterTagLookupTableModal', { static: true })
    jobTagMasterTagLookupTableModal: JobTagMasterTagLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobTag: CreateOrEditJobTagDto = new CreateOrEditJobTagDto();

    jobTitle = '';
    masterTagCategoryName = '';
    masterTagName = '';

    constructor(
        injector: Injector,
        private _jobTagsServiceProxy: JobTagsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(jobTagId?: number): void {
        if (!jobTagId) {
            this.jobTag = new CreateOrEditJobTagDto();
            this.jobTag.id = jobTagId;
            this.jobTitle = '';
            this.masterTagCategoryName = '';
            this.masterTagName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._jobTagsServiceProxy.getJobTagForEdit(jobTagId).subscribe((result) => {
                this.jobTag = result.jobTag;

                this.jobTitle = result.jobTitle;
                this.masterTagCategoryName = result.masterTagCategoryName;
                this.masterTagName = result.masterTagName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._jobTagsServiceProxy
            .createOrEdit(this.jobTag)
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

    openSelectJobModal() {
        this.jobTagJobLookupTableModal.id = this.jobTag.jobId;
        this.jobTagJobLookupTableModal.displayName = this.jobTitle;
        this.jobTagJobLookupTableModal.show();
    }
    openSelectMasterTagCategoryModal() {
        this.jobTagMasterTagCategoryLookupTableModal.id = this.jobTag.masterTagCategoryId;
        this.jobTagMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.jobTagMasterTagCategoryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.jobTagMasterTagLookupTableModal.id = this.jobTag.masterTagId;
        this.jobTagMasterTagLookupTableModal.displayName = this.masterTagName;
        this.jobTagMasterTagLookupTableModal.show();
    }

    setJobIdNull() {
        this.jobTag.jobId = null;
        this.jobTitle = '';
    }
    setMasterTagCategoryIdNull() {
        this.jobTag.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }
    setMasterTagIdNull() {
        this.jobTag.masterTagId = null;
        this.masterTagName = '';
    }

    getNewJobId() {
        this.jobTag.jobId = this.jobTagJobLookupTableModal.id;
        this.jobTitle = this.jobTagJobLookupTableModal.displayName;
    }
    getNewMasterTagCategoryId() {
        this.jobTag.masterTagCategoryId = this.jobTagMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.jobTagMasterTagCategoryLookupTableModal.displayName;
    }
    getNewMasterTagId() {
        this.jobTag.masterTagId = this.jobTagMasterTagLookupTableModal.id;
        this.masterTagName = this.jobTagMasterTagLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
