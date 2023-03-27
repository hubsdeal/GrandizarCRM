import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { LeadTagsServiceProxy, CreateOrEditLeadTagDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { LeadTagLeadLookupTableModalComponent } from './leadTag-lead-lookup-table-modal.component';
import { LeadTagMasterTagCategoryLookupTableModalComponent } from './leadTag-masterTagCategory-lookup-table-modal.component';
import { LeadTagMasterTagLookupTableModalComponent } from './leadTag-masterTag-lookup-table-modal.component';

@Component({
    selector: 'createOrEditLeadTagModal',
    templateUrl: './create-or-edit-leadTag-modal.component.html',
})
export class CreateOrEditLeadTagModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('leadTagLeadLookupTableModal', { static: true })
    leadTagLeadLookupTableModal: LeadTagLeadLookupTableModalComponent;
    @ViewChild('leadTagMasterTagCategoryLookupTableModal', { static: true })
    leadTagMasterTagCategoryLookupTableModal: LeadTagMasterTagCategoryLookupTableModalComponent;
    @ViewChild('leadTagMasterTagLookupTableModal', { static: true })
    leadTagMasterTagLookupTableModal: LeadTagMasterTagLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    leadTag: CreateOrEditLeadTagDto = new CreateOrEditLeadTagDto();

    leadTitle = '';
    masterTagCategoryName = '';
    masterTagName = '';

    constructor(
        injector: Injector,
        private _leadTagsServiceProxy: LeadTagsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(leadTagId?: number): void {
        if (!leadTagId) {
            this.leadTag = new CreateOrEditLeadTagDto();
            this.leadTag.id = leadTagId;
            this.leadTitle = '';
            this.masterTagCategoryName = '';
            this.masterTagName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._leadTagsServiceProxy.getLeadTagForEdit(leadTagId).subscribe((result) => {
                this.leadTag = result.leadTag;

                this.leadTitle = result.leadTitle;
                this.masterTagCategoryName = result.masterTagCategoryName;
                this.masterTagName = result.masterTagName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._leadTagsServiceProxy
            .createOrEdit(this.leadTag)
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
        this.leadTagLeadLookupTableModal.id = this.leadTag.leadId;
        this.leadTagLeadLookupTableModal.displayName = this.leadTitle;
        this.leadTagLeadLookupTableModal.show();
    }
    openSelectMasterTagCategoryModal() {
        this.leadTagMasterTagCategoryLookupTableModal.id = this.leadTag.masterTagCategoryId;
        this.leadTagMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.leadTagMasterTagCategoryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.leadTagMasterTagLookupTableModal.id = this.leadTag.masterTagId;
        this.leadTagMasterTagLookupTableModal.displayName = this.masterTagName;
        this.leadTagMasterTagLookupTableModal.show();
    }

    setLeadIdNull() {
        this.leadTag.leadId = null;
        this.leadTitle = '';
    }
    setMasterTagCategoryIdNull() {
        this.leadTag.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }
    setMasterTagIdNull() {
        this.leadTag.masterTagId = null;
        this.masterTagName = '';
    }

    getNewLeadId() {
        this.leadTag.leadId = this.leadTagLeadLookupTableModal.id;
        this.leadTitle = this.leadTagLeadLookupTableModal.displayName;
    }
    getNewMasterTagCategoryId() {
        this.leadTag.masterTagCategoryId = this.leadTagMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.leadTagMasterTagCategoryLookupTableModal.displayName;
    }
    getNewMasterTagId() {
        this.leadTag.masterTagId = this.leadTagMasterTagLookupTableModal.id;
        this.masterTagName = this.leadTagMasterTagLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
