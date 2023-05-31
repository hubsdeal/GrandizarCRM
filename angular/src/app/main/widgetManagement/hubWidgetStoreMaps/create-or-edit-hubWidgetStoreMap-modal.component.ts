import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    HubWidgetStoreMapsServiceProxy,
    CreateOrEditHubWidgetStoreMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubWidgetStoreMapHubWidgetMapLookupTableModalComponent } from './hubWidgetStoreMap-hubWidgetMap-lookup-table-modal.component';
import { HubWidgetStoreMapStoreLookupTableModalComponent } from './hubWidgetStoreMap-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditHubWidgetStoreMapModal',
    templateUrl: './create-or-edit-hubWidgetStoreMap-modal.component.html',
})
export class CreateOrEditHubWidgetStoreMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubWidgetStoreMapHubWidgetMapLookupTableModal', { static: true })
    hubWidgetStoreMapHubWidgetMapLookupTableModal: HubWidgetStoreMapHubWidgetMapLookupTableModalComponent;
    @ViewChild('hubWidgetStoreMapStoreLookupTableModal', { static: true })
    hubWidgetStoreMapStoreLookupTableModal: HubWidgetStoreMapStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubWidgetStoreMap: CreateOrEditHubWidgetStoreMapDto = new CreateOrEditHubWidgetStoreMapDto();

    hubWidgetMapCustomName = '';
    storeName = '';

    constructor(
        injector: Injector,
        private _hubWidgetStoreMapsServiceProxy: HubWidgetStoreMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubWidgetStoreMapId?: number): void {
        if (!hubWidgetStoreMapId) {
            this.hubWidgetStoreMap = new CreateOrEditHubWidgetStoreMapDto();
            this.hubWidgetStoreMap.id = hubWidgetStoreMapId;
            this.hubWidgetMapCustomName = '';
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._hubWidgetStoreMapsServiceProxy
                .getHubWidgetStoreMapForEdit(hubWidgetStoreMapId)
                .subscribe((result) => {
                    this.hubWidgetStoreMap = result.hubWidgetStoreMap;

                    this.hubWidgetMapCustomName = result.hubWidgetMapCustomName;
                    this.storeName = result.storeName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._hubWidgetStoreMapsServiceProxy
            .createOrEdit(this.hubWidgetStoreMap)
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

    openSelectHubWidgetMapModal() {
        this.hubWidgetStoreMapHubWidgetMapLookupTableModal.id = this.hubWidgetStoreMap.hubWidgetMapId;
        this.hubWidgetStoreMapHubWidgetMapLookupTableModal.displayName = this.hubWidgetMapCustomName;
        this.hubWidgetStoreMapHubWidgetMapLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.hubWidgetStoreMapStoreLookupTableModal.id = this.hubWidgetStoreMap.storeId;
        this.hubWidgetStoreMapStoreLookupTableModal.displayName = this.storeName;
        this.hubWidgetStoreMapStoreLookupTableModal.show();
    }

    setHubWidgetMapIdNull() {
        this.hubWidgetStoreMap.hubWidgetMapId = null;
        this.hubWidgetMapCustomName = '';
    }
    setStoreIdNull() {
        this.hubWidgetStoreMap.storeId = null;
        this.storeName = '';
    }

    getNewHubWidgetMapId() {
        this.hubWidgetStoreMap.hubWidgetMapId = this.hubWidgetStoreMapHubWidgetMapLookupTableModal.id;
        this.hubWidgetMapCustomName = this.hubWidgetStoreMapHubWidgetMapLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.hubWidgetStoreMap.storeId = this.hubWidgetStoreMapStoreLookupTableModal.id;
        this.storeName = this.hubWidgetStoreMapStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
