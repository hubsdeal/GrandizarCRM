import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { HubStoresServiceProxy, CreateOrEditHubStoreDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubStoreHubLookupTableModalComponent } from './hubStore-hub-lookup-table-modal.component';
import { HubStoreStoreLookupTableModalComponent } from './hubStore-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditHubStoreModal',
    templateUrl: './create-or-edit-hubStore-modal.component.html',
})
export class CreateOrEditHubStoreModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubStoreHubLookupTableModal', { static: true })
    hubStoreHubLookupTableModal: HubStoreHubLookupTableModalComponent;
    @ViewChild('hubStoreStoreLookupTableModal', { static: true })
    hubStoreStoreLookupTableModal: HubStoreStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubStore: CreateOrEditHubStoreDto = new CreateOrEditHubStoreDto();

    hubName = '';
    storeName = '';

    constructor(
        injector: Injector,
        private _hubStoresServiceProxy: HubStoresServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubStoreId?: number): void {
        if (!hubStoreId) {
            this.hubStore = new CreateOrEditHubStoreDto();
            this.hubStore.id = hubStoreId;
            this.hubName = '';
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._hubStoresServiceProxy.getHubStoreForEdit(hubStoreId).subscribe((result) => {
                this.hubStore = result.hubStore;

                this.hubName = result.hubName;
                this.storeName = result.storeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._hubStoresServiceProxy
            .createOrEdit(this.hubStore)
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

    openSelectHubModal() {
        this.hubStoreHubLookupTableModal.id = this.hubStore.hubId;
        this.hubStoreHubLookupTableModal.displayName = this.hubName;
        this.hubStoreHubLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.hubStoreStoreLookupTableModal.id = this.hubStore.storeId;
        this.hubStoreStoreLookupTableModal.displayName = this.storeName;
        this.hubStoreStoreLookupTableModal.show();
    }

    setHubIdNull() {
        this.hubStore.hubId = null;
        this.hubName = '';
    }
    setStoreIdNull() {
        this.hubStore.storeId = null;
        this.storeName = '';
    }

    getNewHubId() {
        this.hubStore.hubId = this.hubStoreHubLookupTableModal.id;
        this.hubName = this.hubStoreHubLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.hubStore.storeId = this.hubStoreStoreLookupTableModal.id;
        this.storeName = this.hubStoreStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
