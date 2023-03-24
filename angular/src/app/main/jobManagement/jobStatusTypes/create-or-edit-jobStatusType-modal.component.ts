import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { JobStatusTypesServiceProxy, CreateOrEditJobStatusTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditJobStatusTypeModal',
    templateUrl: './create-or-edit-jobStatusType-modal.component.html',
})
export class CreateOrEditJobStatusTypeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobStatusType: CreateOrEditJobStatusTypeDto = new CreateOrEditJobStatusTypeDto();

    constructor(
        injector: Injector,
        private _jobStatusTypesServiceProxy: JobStatusTypesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(jobStatusTypeId?: number): void {
        if (!jobStatusTypeId) {
            this.jobStatusType = new CreateOrEditJobStatusTypeDto();
            this.jobStatusType.id = jobStatusTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._jobStatusTypesServiceProxy.getJobStatusTypeForEdit(jobStatusTypeId).subscribe((result) => {
                this.jobStatusType = result.jobStatusType;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._jobStatusTypesServiceProxy
            .createOrEdit(this.jobStatusType)
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
