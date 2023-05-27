import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { JobTaskMapsServiceProxy, CreateOrEditJobTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { JobTaskMapJobLookupTableModalComponent } from './jobTaskMap-job-lookup-table-modal.component';
import { JobTaskMapTaskEventLookupTableModalComponent } from './jobTaskMap-taskEvent-lookup-table-modal.component';

@Component({
    selector: 'createOrEditJobTaskMapModal',
    templateUrl: './create-or-edit-jobTaskMap-modal.component.html',
})
export class CreateOrEditJobTaskMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('jobTaskMapJobLookupTableModal', { static: true })
    jobTaskMapJobLookupTableModal: JobTaskMapJobLookupTableModalComponent;
    @ViewChild('jobTaskMapTaskEventLookupTableModal', { static: true })
    jobTaskMapTaskEventLookupTableModal: JobTaskMapTaskEventLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobTaskMap: CreateOrEditJobTaskMapDto = new CreateOrEditJobTaskMapDto();

    jobTitle = '';
    taskEventName = '';

    constructor(
        injector: Injector,
        private _jobTaskMapsServiceProxy: JobTaskMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(jobTaskMapId?: number): void {
        if (!jobTaskMapId) {
            this.jobTaskMap = new CreateOrEditJobTaskMapDto();
            this.jobTaskMap.id = jobTaskMapId;
            this.jobTitle = '';
            this.taskEventName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._jobTaskMapsServiceProxy.getJobTaskMapForEdit(jobTaskMapId).subscribe((result) => {
                this.jobTaskMap = result.jobTaskMap;

                this.jobTitle = result.jobTitle;
                this.taskEventName = result.taskEventName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._jobTaskMapsServiceProxy
            .createOrEdit(this.jobTaskMap)
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
        this.jobTaskMapJobLookupTableModal.id = this.jobTaskMap.jobId;
        this.jobTaskMapJobLookupTableModal.displayName = this.jobTitle;
        this.jobTaskMapJobLookupTableModal.show();
    }
    openSelectTaskEventModal() {
        this.jobTaskMapTaskEventLookupTableModal.id = this.jobTaskMap.taskEventId;
        this.jobTaskMapTaskEventLookupTableModal.displayName = this.taskEventName;
        this.jobTaskMapTaskEventLookupTableModal.show();
    }

    setJobIdNull() {
        this.jobTaskMap.jobId = null;
        this.jobTitle = '';
    }
    setTaskEventIdNull() {
        this.jobTaskMap.taskEventId = null;
        this.taskEventName = '';
    }

    getNewJobId() {
        this.jobTaskMap.jobId = this.jobTaskMapJobLookupTableModal.id;
        this.jobTitle = this.jobTaskMapJobLookupTableModal.displayName;
    }
    getNewTaskEventId() {
        this.jobTaskMap.taskEventId = this.jobTaskMapTaskEventLookupTableModal.id;
        this.taskEventName = this.jobTaskMapTaskEventLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
