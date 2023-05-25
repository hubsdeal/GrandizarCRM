import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    JobMasterTagSettingsServiceProxy,
    CreateOrEditJobMasterTagSettingDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { JobMasterTagSettingMasterTagLookupTableModalComponent } from './jobMasterTagSetting-masterTag-lookup-table-modal.component';
import { JobMasterTagSettingMasterTagCategoryLookupTableModalComponent } from './jobMasterTagSetting-masterTagCategory-lookup-table-modal.component';

@Component({
    selector: 'createOrEditJobMasterTagSettingModal',
    templateUrl: './create-or-edit-jobMasterTagSetting-modal.component.html',
})
export class CreateOrEditJobMasterTagSettingModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('jobMasterTagSettingMasterTagLookupTableModal', { static: true })
    jobMasterTagSettingMasterTagLookupTableModal: JobMasterTagSettingMasterTagLookupTableModalComponent;
    @ViewChild('jobMasterTagSettingMasterTagCategoryLookupTableModal', { static: true })
    jobMasterTagSettingMasterTagCategoryLookupTableModal: JobMasterTagSettingMasterTagCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobMasterTagSetting: CreateOrEditJobMasterTagSettingDto = new CreateOrEditJobMasterTagSettingDto();

    masterTagName = '';
    masterTagCategoryName = '';

    constructor(
        injector: Injector,
        private _jobMasterTagSettingsServiceProxy: JobMasterTagSettingsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(jobMasterTagSettingId?: number): void {
        if (!jobMasterTagSettingId) {
            this.jobMasterTagSetting = new CreateOrEditJobMasterTagSettingDto();
            this.jobMasterTagSetting.id = jobMasterTagSettingId;
            this.masterTagName = '';
            this.masterTagCategoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._jobMasterTagSettingsServiceProxy
                .getJobMasterTagSettingForEdit(jobMasterTagSettingId)
                .subscribe((result) => {
                    this.jobMasterTagSetting = result.jobMasterTagSetting;

                    this.masterTagName = result.masterTagName;
                    this.masterTagCategoryName = result.masterTagCategoryName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._jobMasterTagSettingsServiceProxy
            .createOrEdit(this.jobMasterTagSetting)
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

    openSelectMasterTagModal() {
        this.jobMasterTagSettingMasterTagLookupTableModal.id = this.jobMasterTagSetting.jobCategoryId;
        this.jobMasterTagSettingMasterTagLookupTableModal.displayName = this.masterTagName;
        this.jobMasterTagSettingMasterTagLookupTableModal.show();
    }
    openSelectMasterTagCategoryModal() {
        this.jobMasterTagSettingMasterTagCategoryLookupTableModal.id = this.jobMasterTagSetting.masterTagCategoryId;
        this.jobMasterTagSettingMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.jobMasterTagSettingMasterTagCategoryLookupTableModal.show();
    }

    setJobCategoryIdNull() {
        this.jobMasterTagSetting.jobCategoryId = null;
        this.masterTagName = '';
    }
    setMasterTagCategoryIdNull() {
        this.jobMasterTagSetting.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }

    getNewJobCategoryId() {
        this.jobMasterTagSetting.jobCategoryId = this.jobMasterTagSettingMasterTagLookupTableModal.id;
        this.masterTagName = this.jobMasterTagSettingMasterTagLookupTableModal.displayName;
    }
    getNewMasterTagCategoryId() {
        this.jobMasterTagSetting.masterTagCategoryId = this.jobMasterTagSettingMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.jobMasterTagSettingMasterTagCategoryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
