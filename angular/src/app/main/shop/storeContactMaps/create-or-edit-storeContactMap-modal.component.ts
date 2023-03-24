import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreContactMapsServiceProxy, CreateOrEditStoreContactMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreContactMapStoreLookupTableModalComponent } from './storeContactMap-store-lookup-table-modal.component';
import { StoreContactMapContactLookupTableModalComponent } from './storeContactMap-contact-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreContactMapModal',
    templateUrl: './create-or-edit-storeContactMap-modal.component.html',
})
export class CreateOrEditStoreContactMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeContactMapStoreLookupTableModal', { static: true })
    storeContactMapStoreLookupTableModal: StoreContactMapStoreLookupTableModalComponent;
    @ViewChild('storeContactMapContactLookupTableModal', { static: true })
    storeContactMapContactLookupTableModal: StoreContactMapContactLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeContactMap: CreateOrEditStoreContactMapDto = new CreateOrEditStoreContactMapDto();

    storeName = '';
    contactFullName = '';

    constructor(
        injector: Injector,
        private _storeContactMapsServiceProxy: StoreContactMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeContactMapId?: number): void {
        if (!storeContactMapId) {
            this.storeContactMap = new CreateOrEditStoreContactMapDto();
            this.storeContactMap.id = storeContactMapId;
            this.storeName = '';
            this.contactFullName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeContactMapsServiceProxy.getStoreContactMapForEdit(storeContactMapId).subscribe((result) => {
                this.storeContactMap = result.storeContactMap;

                this.storeName = result.storeName;
                this.contactFullName = result.contactFullName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._storeContactMapsServiceProxy
            .createOrEdit(this.storeContactMap)
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
        this.storeContactMapStoreLookupTableModal.id = this.storeContactMap.storeId;
        this.storeContactMapStoreLookupTableModal.displayName = this.storeName;
        this.storeContactMapStoreLookupTableModal.show();
    }
    openSelectContactModal() {
        this.storeContactMapContactLookupTableModal.id = this.storeContactMap.contactId;
        this.storeContactMapContactLookupTableModal.displayName = this.contactFullName;
        this.storeContactMapContactLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeContactMap.storeId = null;
        this.storeName = '';
    }
    setContactIdNull() {
        this.storeContactMap.contactId = null;
        this.contactFullName = '';
    }

    getNewStoreId() {
        this.storeContactMap.storeId = this.storeContactMapStoreLookupTableModal.id;
        this.storeName = this.storeContactMapStoreLookupTableModal.displayName;
    }
    getNewContactId() {
        this.storeContactMap.contactId = this.storeContactMapContactLookupTableModal.id;
        this.contactFullName = this.storeContactMapContactLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
