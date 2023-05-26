import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreMasterTagSettingsServiceProxy,
    CreateOrEditStoreMasterTagSettingDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreMasterTagSettingStoreTagSettingCategoryLookupTableModalComponent } from './storeMasterTagSetting-storeTagSettingCategory-lookup-table-modal.component';
import { StoreMasterTagSettingMasterTagCategoryLookupTableModalComponent } from './storeMasterTagSetting-masterTagCategory-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreMasterTagSettingModal',
    templateUrl: './create-or-edit-storeMasterTagSetting-modal.component.html',
})
export class CreateOrEditStoreMasterTagSettingModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeMasterTagSettingStoreTagSettingCategoryLookupTableModal', { static: true })
    storeMasterTagSettingStoreTagSettingCategoryLookupTableModal: StoreMasterTagSettingStoreTagSettingCategoryLookupTableModalComponent;
    @ViewChild('storeMasterTagSettingMasterTagCategoryLookupTableModal', { static: true })
    storeMasterTagSettingMasterTagCategoryLookupTableModal: StoreMasterTagSettingMasterTagCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeMasterTagSetting: CreateOrEditStoreMasterTagSettingDto = new CreateOrEditStoreMasterTagSettingDto();

    storeTagSettingCategoryName = '';
    masterTagCategoryName = '';

    selectedStoreTagSettingCategory: any;
    storeTagSettingCategoryOptions: any = []

    selectedTagCategory: any;
    masterTagCategoryOptions: any = []

    constructor(
        injector: Injector,
        private _storeMasterTagSettingsServiceProxy: StoreMasterTagSettingsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeMasterTagSettingId?: number): void {
        if (!storeMasterTagSettingId) {
            this.storeMasterTagSetting = new CreateOrEditStoreMasterTagSettingDto();
            this.storeMasterTagSetting.id = storeMasterTagSettingId;
            this.storeTagSettingCategoryName = '';
            this.masterTagCategoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeMasterTagSettingsServiceProxy
                .getStoreMasterTagSettingForEdit(storeMasterTagSettingId)
                .subscribe((result) => {
                    this.storeMasterTagSetting = result.storeMasterTagSetting;

                    this.storeTagSettingCategoryName = result.storeTagSettingCategoryName;
                    this.masterTagCategoryName = result.masterTagCategoryName;

                    this.active = true;
                    this.modal.show();
                });
        }
        this._storeMasterTagSettingsServiceProxy.getAllStoreTagSettingCategoryForLookupTable('', '', 0, 1000).subscribe(result => {
            this.storeTagSettingCategoryOptions = result.items;
        });
        this._storeMasterTagSettingsServiceProxy.getAllMasterTagCategoryForLookupTable('', '', 0, 1000).subscribe(result => {
            this.masterTagCategoryOptions = result.items;
        })
    }

    save(): void {
        this.saving = true;

        this._storeMasterTagSettingsServiceProxy
            .createOrEdit(this.storeMasterTagSetting)
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

    onStoreTagSettingCategoryClick(event: any) {
        if (event.value != null) {
            this.storeMasterTagSetting.storeTagSettingCategoryId = event.value.id;
        }
    }
    onMmasterTagCategoryClick(event: any) {
        if (event.value != null) {
            this.storeMasterTagSetting.masterTagCategoryId = event.value.id;
        }
    }
    openSelectStoreTagSettingCategoryModal() {
        this.storeMasterTagSettingStoreTagSettingCategoryLookupTableModal.id =
            this.storeMasterTagSetting.storeTagSettingCategoryId;
        this.storeMasterTagSettingStoreTagSettingCategoryLookupTableModal.displayName =
            this.storeTagSettingCategoryName;
        this.storeMasterTagSettingStoreTagSettingCategoryLookupTableModal.show();
    }
    openSelectMasterTagCategoryModal() {
        this.storeMasterTagSettingMasterTagCategoryLookupTableModal.id = this.storeMasterTagSetting.masterTagCategoryId;
        this.storeMasterTagSettingMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.storeMasterTagSettingMasterTagCategoryLookupTableModal.show();
    }

    setStoreTagSettingCategoryIdNull() {
        this.storeMasterTagSetting.storeTagSettingCategoryId = null;
        this.storeTagSettingCategoryName = '';
    }
    setMasterTagCategoryIdNull() {
        this.storeMasterTagSetting.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }

    getNewStoreTagSettingCategoryId() {
        this.storeMasterTagSetting.storeTagSettingCategoryId =
            this.storeMasterTagSettingStoreTagSettingCategoryLookupTableModal.id;
        this.storeTagSettingCategoryName =
            this.storeMasterTagSettingStoreTagSettingCategoryLookupTableModal.displayName;
    }
    getNewMasterTagCategoryId() {
        this.storeMasterTagSetting.masterTagCategoryId = this.storeMasterTagSettingMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.storeMasterTagSettingMasterTagCategoryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void { }
}
