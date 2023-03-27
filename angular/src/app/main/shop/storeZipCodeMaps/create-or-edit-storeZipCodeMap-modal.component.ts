import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreZipCodeMapsServiceProxy, CreateOrEditStoreZipCodeMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreZipCodeMapStoreLookupTableModalComponent } from './storeZipCodeMap-store-lookup-table-modal.component';
import { StoreZipCodeMapZipCodeLookupTableModalComponent } from './storeZipCodeMap-zipCode-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreZipCodeMapModal',
    templateUrl: './create-or-edit-storeZipCodeMap-modal.component.html',
})
export class CreateOrEditStoreZipCodeMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeZipCodeMapStoreLookupTableModal', { static: true })
    storeZipCodeMapStoreLookupTableModal: StoreZipCodeMapStoreLookupTableModalComponent;
    @ViewChild('storeZipCodeMapZipCodeLookupTableModal', { static: true })
    storeZipCodeMapZipCodeLookupTableModal: StoreZipCodeMapZipCodeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeZipCodeMap: CreateOrEditStoreZipCodeMapDto = new CreateOrEditStoreZipCodeMapDto();

    storeName = '';
    zipCodeName = '';

    constructor(
        injector: Injector,
        private _storeZipCodeMapsServiceProxy: StoreZipCodeMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeZipCodeMapId?: number): void {
        if (!storeZipCodeMapId) {
            this.storeZipCodeMap = new CreateOrEditStoreZipCodeMapDto();
            this.storeZipCodeMap.id = storeZipCodeMapId;
            this.storeName = '';
            this.zipCodeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeZipCodeMapsServiceProxy.getStoreZipCodeMapForEdit(storeZipCodeMapId).subscribe((result) => {
                this.storeZipCodeMap = result.storeZipCodeMap;

                this.storeName = result.storeName;
                this.zipCodeName = result.zipCodeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._storeZipCodeMapsServiceProxy
            .createOrEdit(this.storeZipCodeMap)
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
        this.storeZipCodeMapStoreLookupTableModal.id = this.storeZipCodeMap.storeId;
        this.storeZipCodeMapStoreLookupTableModal.displayName = this.storeName;
        this.storeZipCodeMapStoreLookupTableModal.show();
    }
    openSelectZipCodeModal() {
        this.storeZipCodeMapZipCodeLookupTableModal.id = this.storeZipCodeMap.zipCodeId;
        this.storeZipCodeMapZipCodeLookupTableModal.displayName = this.zipCodeName;
        this.storeZipCodeMapZipCodeLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeZipCodeMap.storeId = null;
        this.storeName = '';
    }
    setZipCodeIdNull() {
        this.storeZipCodeMap.zipCodeId = null;
        this.zipCodeName = '';
    }

    getNewStoreId() {
        this.storeZipCodeMap.storeId = this.storeZipCodeMapStoreLookupTableModal.id;
        this.storeName = this.storeZipCodeMapStoreLookupTableModal.displayName;
    }
    getNewZipCodeId() {
        this.storeZipCodeMap.zipCodeId = this.storeZipCodeMapZipCodeLookupTableModal.id;
        this.zipCodeName = this.storeZipCodeMapZipCodeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
