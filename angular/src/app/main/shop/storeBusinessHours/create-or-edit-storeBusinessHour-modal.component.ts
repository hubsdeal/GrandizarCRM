import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreBusinessHoursServiceProxy,
    CreateOrEditStoreBusinessHourDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreBusinessHourStoreLookupTableModalComponent } from './storeBusinessHour-store-lookup-table-modal.component';
import { StoreBusinessHourMasterTagCategoryLookupTableModalComponent } from './storeBusinessHour-masterTagCategory-lookup-table-modal.component';
import { StoreBusinessHourMasterTagLookupTableModalComponent } from './storeBusinessHour-masterTag-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreBusinessHourModal',
    templateUrl: './create-or-edit-storeBusinessHour-modal.component.html',
})
export class CreateOrEditStoreBusinessHourModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeBusinessHourStoreLookupTableModal', { static: true })
    storeBusinessHourStoreLookupTableModal: StoreBusinessHourStoreLookupTableModalComponent;
    @ViewChild('storeBusinessHourMasterTagCategoryLookupTableModal', { static: true })
    storeBusinessHourMasterTagCategoryLookupTableModal: StoreBusinessHourMasterTagCategoryLookupTableModalComponent;
    @ViewChild('storeBusinessHourMasterTagLookupTableModal', { static: true })
    storeBusinessHourMasterTagLookupTableModal: StoreBusinessHourMasterTagLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeBusinessHour: CreateOrEditStoreBusinessHourDto = new CreateOrEditStoreBusinessHourDto();

    storeName = '';
    masterTagCategoryName = '';
    masterTagName = '';

    constructor(
        injector: Injector,
        private _storeBusinessHoursServiceProxy: StoreBusinessHoursServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeBusinessHourId?: number): void {
        if (!storeBusinessHourId) {
            this.storeBusinessHour = new CreateOrEditStoreBusinessHourDto();
            this.storeBusinessHour.id = storeBusinessHourId;
            this.storeName = '';
            this.masterTagCategoryName = '';
            this.masterTagName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeBusinessHoursServiceProxy
                .getStoreBusinessHourForEdit(storeBusinessHourId)
                .subscribe((result) => {
                    this.storeBusinessHour = result.storeBusinessHour;

                    this.storeName = result.storeName;
                    this.masterTagCategoryName = result.masterTagCategoryName;
                    this.masterTagName = result.masterTagName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._storeBusinessHoursServiceProxy
            .createOrEdit(this.storeBusinessHour)
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

    openSelectStoreModal() {
        this.storeBusinessHourStoreLookupTableModal.id = this.storeBusinessHour.storeId;
        this.storeBusinessHourStoreLookupTableModal.displayName = this.storeName;
        this.storeBusinessHourStoreLookupTableModal.show();
    }
    openSelectMasterTagCategoryModal() {
        this.storeBusinessHourMasterTagCategoryLookupTableModal.id = this.storeBusinessHour.masterTagCategoryId;
        this.storeBusinessHourMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.storeBusinessHourMasterTagCategoryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.storeBusinessHourMasterTagLookupTableModal.id = this.storeBusinessHour.masterTagId;
        this.storeBusinessHourMasterTagLookupTableModal.displayName = this.masterTagName;
        this.storeBusinessHourMasterTagLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeBusinessHour.storeId = null;
        this.storeName = '';
    }
    setMasterTagCategoryIdNull() {
        this.storeBusinessHour.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }
    setMasterTagIdNull() {
        this.storeBusinessHour.masterTagId = null;
        this.masterTagName = '';
    }

    getNewStoreId() {
        this.storeBusinessHour.storeId = this.storeBusinessHourStoreLookupTableModal.id;
        this.storeName = this.storeBusinessHourStoreLookupTableModal.displayName;
    }
    getNewMasterTagCategoryId() {
        this.storeBusinessHour.masterTagCategoryId = this.storeBusinessHourMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.storeBusinessHourMasterTagCategoryLookupTableModal.displayName;
    }
    getNewMasterTagId() {
        this.storeBusinessHour.masterTagId = this.storeBusinessHourMasterTagLookupTableModal.id;
        this.masterTagName = this.storeBusinessHourMasterTagLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
