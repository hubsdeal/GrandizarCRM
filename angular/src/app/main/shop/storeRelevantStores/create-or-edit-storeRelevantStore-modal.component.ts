import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreRelevantStoresServiceProxy,
    CreateOrEditStoreRelevantStoreDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreRelevantStoreStoreLookupTableModalComponent } from './storeRelevantStore-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreRelevantStoreModal',
    templateUrl: './create-or-edit-storeRelevantStore-modal.component.html',
})
export class CreateOrEditStoreRelevantStoreModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeRelevantStoreStoreLookupTableModal', { static: true })
    storeRelevantStoreStoreLookupTableModal: StoreRelevantStoreStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeRelevantStore: CreateOrEditStoreRelevantStoreDto = new CreateOrEditStoreRelevantStoreDto();

    storeName = '';

    constructor(
        injector: Injector,
        private _storeRelevantStoresServiceProxy: StoreRelevantStoresServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeRelevantStoreId?: number): void {
        if (!storeRelevantStoreId) {
            this.storeRelevantStore = new CreateOrEditStoreRelevantStoreDto();
            this.storeRelevantStore.id = storeRelevantStoreId;
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeRelevantStoresServiceProxy
                .getStoreRelevantStoreForEdit(storeRelevantStoreId)
                .subscribe((result) => {
                    this.storeRelevantStore = result.storeRelevantStore;

                    this.storeName = result.storeName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._storeRelevantStoresServiceProxy
            .createOrEdit(this.storeRelevantStore)
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
        this.storeRelevantStoreStoreLookupTableModal.id = this.storeRelevantStore.primaryStoreId;
        this.storeRelevantStoreStoreLookupTableModal.displayName = this.storeName;
        this.storeRelevantStoreStoreLookupTableModal.show();
    }

    setPrimaryStoreIdNull() {
        this.storeRelevantStore.primaryStoreId = null;
        this.storeName = '';
    }

    getNewPrimaryStoreId() {
        this.storeRelevantStore.primaryStoreId = this.storeRelevantStoreStoreLookupTableModal.id;
        this.storeName = this.storeRelevantStoreStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
