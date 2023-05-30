import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreWidgetMapsServiceProxy, CreateOrEditStoreWidgetMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreWidgetMapMasterWidgetLookupTableModalComponent } from './storeWidgetMap-masterWidget-lookup-table-modal.component';
import { StoreWidgetMapStoreLookupTableModalComponent } from './storeWidgetMap-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreWidgetMapModal',
    templateUrl: './create-or-edit-storeWidgetMap-modal.component.html',
})
export class CreateOrEditStoreWidgetMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeWidgetMapMasterWidgetLookupTableModal', { static: true })
    storeWidgetMapMasterWidgetLookupTableModal: StoreWidgetMapMasterWidgetLookupTableModalComponent;
    @ViewChild('storeWidgetMapStoreLookupTableModal', { static: true })
    storeWidgetMapStoreLookupTableModal: StoreWidgetMapStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeWidgetMap: CreateOrEditStoreWidgetMapDto = new CreateOrEditStoreWidgetMapDto();

    masterWidgetName = '';
    storeName = '';

    constructor(
        injector: Injector,
        private _storeWidgetMapsServiceProxy: StoreWidgetMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeWidgetMapId?: number): void {
        if (!storeWidgetMapId) {
            this.storeWidgetMap = new CreateOrEditStoreWidgetMapDto();
            this.storeWidgetMap.id = storeWidgetMapId;
            this.masterWidgetName = '';
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeWidgetMapsServiceProxy.getStoreWidgetMapForEdit(storeWidgetMapId).subscribe((result) => {
                this.storeWidgetMap = result.storeWidgetMap;

                this.masterWidgetName = result.masterWidgetName;
                this.storeName = result.storeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._storeWidgetMapsServiceProxy
            .createOrEdit(this.storeWidgetMap)
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

    openSelectMasterWidgetModal() {
        this.storeWidgetMapMasterWidgetLookupTableModal.id = this.storeWidgetMap.masterWidgetId;
        this.storeWidgetMapMasterWidgetLookupTableModal.displayName = this.masterWidgetName;
        this.storeWidgetMapMasterWidgetLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.storeWidgetMapStoreLookupTableModal.id = this.storeWidgetMap.storeId;
        this.storeWidgetMapStoreLookupTableModal.displayName = this.storeName;
        this.storeWidgetMapStoreLookupTableModal.show();
    }

    setMasterWidgetIdNull() {
        this.storeWidgetMap.masterWidgetId = null;
        this.masterWidgetName = '';
    }
    setStoreIdNull() {
        this.storeWidgetMap.storeId = null;
        this.storeName = '';
    }

    getNewMasterWidgetId() {
        this.storeWidgetMap.masterWidgetId = this.storeWidgetMapMasterWidgetLookupTableModal.id;
        this.masterWidgetName = this.storeWidgetMapMasterWidgetLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.storeWidgetMap.storeId = this.storeWidgetMapStoreLookupTableModal.id;
        this.storeName = this.storeWidgetMapStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
