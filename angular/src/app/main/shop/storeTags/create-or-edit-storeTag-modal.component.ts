import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreTagsServiceProxy, CreateOrEditStoreTagDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreTagStoreLookupTableModalComponent } from './storeTag-store-lookup-table-modal.component';
import { StoreTagMasterTagCategoryLookupTableModalComponent } from './storeTag-masterTagCategory-lookup-table-modal.component';
import { StoreTagMasterTagLookupTableModalComponent } from './storeTag-masterTag-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreTagModal',
    templateUrl: './create-or-edit-storeTag-modal.component.html',
})
export class CreateOrEditStoreTagModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeTagStoreLookupTableModal', { static: true })
    storeTagStoreLookupTableModal: StoreTagStoreLookupTableModalComponent;
    @ViewChild('storeTagMasterTagCategoryLookupTableModal', { static: true })
    storeTagMasterTagCategoryLookupTableModal: StoreTagMasterTagCategoryLookupTableModalComponent;
    @ViewChild('storeTagMasterTagLookupTableModal', { static: true })
    storeTagMasterTagLookupTableModal: StoreTagMasterTagLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeTag: CreateOrEditStoreTagDto = new CreateOrEditStoreTagDto();

    storeName = '';
    masterTagCategoryName = '';
    masterTagName = '';

    constructor(
        injector: Injector,
        private _storeTagsServiceProxy: StoreTagsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeTagId?: number): void {
        if (!storeTagId) {
            this.storeTag = new CreateOrEditStoreTagDto();
            this.storeTag.id = storeTagId;
            this.storeName = '';
            this.masterTagCategoryName = '';
            this.masterTagName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeTagsServiceProxy.getStoreTagForEdit(storeTagId).subscribe((result) => {
                this.storeTag = result.storeTag;

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

        this._storeTagsServiceProxy
            .createOrEdit(this.storeTag)
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
        this.storeTagStoreLookupTableModal.id = this.storeTag.storeId;
        this.storeTagStoreLookupTableModal.displayName = this.storeName;
        this.storeTagStoreLookupTableModal.show();
    }
    openSelectMasterTagCategoryModal() {
        this.storeTagMasterTagCategoryLookupTableModal.id = this.storeTag.masterTagCategoryId;
        this.storeTagMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.storeTagMasterTagCategoryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.storeTagMasterTagLookupTableModal.id = this.storeTag.masterTagId;
        this.storeTagMasterTagLookupTableModal.displayName = this.masterTagName;
        this.storeTagMasterTagLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeTag.storeId = null;
        this.storeName = '';
    }
    setMasterTagCategoryIdNull() {
        this.storeTag.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }
    setMasterTagIdNull() {
        this.storeTag.masterTagId = null;
        this.masterTagName = '';
    }

    getNewStoreId() {
        this.storeTag.storeId = this.storeTagStoreLookupTableModal.id;
        this.storeName = this.storeTagStoreLookupTableModal.displayName;
    }
    getNewMasterTagCategoryId() {
        this.storeTag.masterTagCategoryId = this.storeTagMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.storeTagMasterTagCategoryLookupTableModal.displayName;
    }
    getNewMasterTagId() {
        this.storeTag.masterTagId = this.storeTagMasterTagLookupTableModal.id;
        this.masterTagName = this.storeTagMasterTagLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
