import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    BusinessMasterTagSettingsServiceProxy,
    CreateOrEditBusinessMasterTagSettingDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BusinessMasterTagSettingMasterTagCategoryLookupTableModalComponent } from './businessMasterTagSetting-masterTagCategory-lookup-table-modal.component';
import { BusinessMasterTagSettingMasterTagLookupTableModalComponent } from './businessMasterTagSetting-masterTag-lookup-table-modal.component';

@Component({
    selector: 'createOrEditBusinessMasterTagSettingModal',
    templateUrl: './create-or-edit-businessMasterTagSetting-modal.component.html',
})
export class CreateOrEditBusinessMasterTagSettingModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('businessMasterTagSettingMasterTagCategoryLookupTableModal', { static: true })
    businessMasterTagSettingMasterTagCategoryLookupTableModal: BusinessMasterTagSettingMasterTagCategoryLookupTableModalComponent;
    @ViewChild('businessMasterTagSettingMasterTagLookupTableModal', { static: true })
    businessMasterTagSettingMasterTagLookupTableModal: BusinessMasterTagSettingMasterTagLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    businessMasterTagSetting: CreateOrEditBusinessMasterTagSettingDto = new CreateOrEditBusinessMasterTagSettingDto();

    masterTagCategoryName = '';
    masterTagName = '';

    constructor(
        injector: Injector,
        private _businessMasterTagSettingsServiceProxy: BusinessMasterTagSettingsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(businessMasterTagSettingId?: number): void {
        if (!businessMasterTagSettingId) {
            this.businessMasterTagSetting = new CreateOrEditBusinessMasterTagSettingDto();
            this.businessMasterTagSetting.id = businessMasterTagSettingId;
            this.masterTagCategoryName = '';
            this.masterTagName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._businessMasterTagSettingsServiceProxy
                .getBusinessMasterTagSettingForEdit(businessMasterTagSettingId)
                .subscribe((result) => {
                    this.businessMasterTagSetting = result.businessMasterTagSetting;

                    this.masterTagCategoryName = result.masterTagCategoryName;
                    this.masterTagName = result.masterTagName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._businessMasterTagSettingsServiceProxy
            .createOrEdit(this.businessMasterTagSetting)
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

    openSelectMasterTagCategoryModal() {
        this.businessMasterTagSettingMasterTagCategoryLookupTableModal.id =
            this.businessMasterTagSetting.masterTagCategoryId;
        this.businessMasterTagSettingMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.businessMasterTagSettingMasterTagCategoryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.businessMasterTagSettingMasterTagLookupTableModal.id = this.businessMasterTagSetting.businessTypeId;
        this.businessMasterTagSettingMasterTagLookupTableModal.displayName = this.masterTagName;
        this.businessMasterTagSettingMasterTagLookupTableModal.show();
    }

    setMasterTagCategoryIdNull() {
        this.businessMasterTagSetting.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }
    setBusinessTypeIdNull() {
        this.businessMasterTagSetting.businessTypeId = null;
        this.masterTagName = '';
    }

    getNewMasterTagCategoryId() {
        this.businessMasterTagSetting.masterTagCategoryId =
            this.businessMasterTagSettingMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.businessMasterTagSettingMasterTagCategoryLookupTableModal.displayName;
    }
    getNewBusinessTypeId() {
        this.businessMasterTagSetting.businessTypeId = this.businessMasterTagSettingMasterTagLookupTableModal.id;
        this.masterTagName = this.businessMasterTagSettingMasterTagLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
