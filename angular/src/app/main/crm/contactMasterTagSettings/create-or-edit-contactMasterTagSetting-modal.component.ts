import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ContactMasterTagSettingsServiceProxy,
    CreateOrEditContactMasterTagSettingDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContactMasterTagSettingMasterTagLookupTableModalComponent } from './contactMasterTagSetting-masterTag-lookup-table-modal.component';
import { ContactMasterTagSettingMasterTagCategoryLookupTableModalComponent } from './contactMasterTagSetting-masterTagCategory-lookup-table-modal.component';

@Component({
    selector: 'createOrEditContactMasterTagSettingModal',
    templateUrl: './create-or-edit-contactMasterTagSetting-modal.component.html',
})
export class CreateOrEditContactMasterTagSettingModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contactMasterTagSettingMasterTagLookupTableModal', { static: true })
    contactMasterTagSettingMasterTagLookupTableModal: ContactMasterTagSettingMasterTagLookupTableModalComponent;
    @ViewChild('contactMasterTagSettingMasterTagCategoryLookupTableModal', { static: true })
    contactMasterTagSettingMasterTagCategoryLookupTableModal: ContactMasterTagSettingMasterTagCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contactMasterTagSetting: CreateOrEditContactMasterTagSettingDto = new CreateOrEditContactMasterTagSettingDto();

    masterTagName = '';
    masterTagCategoryName = '';

    constructor(
        injector: Injector,
        private _contactMasterTagSettingsServiceProxy: ContactMasterTagSettingsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(contactMasterTagSettingId?: number): void {
        if (!contactMasterTagSettingId) {
            this.contactMasterTagSetting = new CreateOrEditContactMasterTagSettingDto();
            this.contactMasterTagSetting.id = contactMasterTagSettingId;
            this.masterTagName = '';
            this.masterTagCategoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._contactMasterTagSettingsServiceProxy
                .getContactMasterTagSettingForEdit(contactMasterTagSettingId)
                .subscribe((result) => {
                    this.contactMasterTagSetting = result.contactMasterTagSetting;

                    this.masterTagName = result.masterTagName;
                    this.masterTagCategoryName = result.masterTagCategoryName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._contactMasterTagSettingsServiceProxy
            .createOrEdit(this.contactMasterTagSetting)
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
        this.contactMasterTagSettingMasterTagLookupTableModal.id = this.contactMasterTagSetting.contactTypeId;
        this.contactMasterTagSettingMasterTagLookupTableModal.displayName = this.masterTagName;
        this.contactMasterTagSettingMasterTagLookupTableModal.show();
    }
    openSelectMasterTagCategoryModal() {
        this.contactMasterTagSettingMasterTagCategoryLookupTableModal.id =
            this.contactMasterTagSetting.masterTagCategoryId;
        this.contactMasterTagSettingMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.contactMasterTagSettingMasterTagCategoryLookupTableModal.show();
    }

    setContactTypeIdNull() {
        this.contactMasterTagSetting.contactTypeId = null;
        this.masterTagName = '';
    }
    setMasterTagCategoryIdNull() {
        this.contactMasterTagSetting.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }

    getNewContactTypeId() {
        this.contactMasterTagSetting.contactTypeId = this.contactMasterTagSettingMasterTagLookupTableModal.id;
        this.masterTagName = this.contactMasterTagSettingMasterTagLookupTableModal.displayName;
    }
    getNewMasterTagCategoryId() {
        this.contactMasterTagSetting.masterTagCategoryId =
            this.contactMasterTagSettingMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.contactMasterTagSettingMasterTagCategoryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
