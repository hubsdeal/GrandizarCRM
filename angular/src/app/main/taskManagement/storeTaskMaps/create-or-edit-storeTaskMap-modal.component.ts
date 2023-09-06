import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreTaskMapsServiceProxy, CreateOrEditStoreTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreTaskMapStoreLookupTableModalComponent } from './storeTaskMap-store-lookup-table-modal.component';
import { StoreTaskMapTaskEventLookupTableModalComponent } from './storeTaskMap-taskEvent-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreTaskMapModal',
    templateUrl: './create-or-edit-storeTaskMap-modal.component.html',
})
export class CreateOrEditStoreTaskMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeTaskMapStoreLookupTableModal', { static: true })
    storeTaskMapStoreLookupTableModal: StoreTaskMapStoreLookupTableModalComponent;
    @ViewChild('storeTaskMapTaskEventLookupTableModal', { static: true })
    storeTaskMapTaskEventLookupTableModal: StoreTaskMapTaskEventLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeTaskMap: CreateOrEditStoreTaskMapDto = new CreateOrEditStoreTaskMapDto();

    storeName = '';
    taskEventName = '';
    storeId:number;
    constructor(
        injector: Injector,
        private _storeTaskMapsServiceProxy: StoreTaskMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeTaskMapId?: number): void {
        if (!storeTaskMapId) {
            this.storeTaskMap = new CreateOrEditStoreTaskMapDto();
            this.storeTaskMap.id = storeTaskMapId;
            this.storeName = '';
            this.taskEventName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeTaskMapsServiceProxy.getStoreTaskMapForEdit(storeTaskMapId).subscribe((result) => {
                this.storeTaskMap = result.storeTaskMap;
                this.storeId = result.storeTaskMap.storeId;
                this.storeName = result.storeName;
                this.taskEventName = result.taskEventName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this.storeTaskMap.storeId = this.storeId;
        this._storeTaskMapsServiceProxy
            .createOrEdit(this.storeTaskMap)
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
        this.storeTaskMapStoreLookupTableModal.id = this.storeTaskMap.storeId;
        this.storeTaskMapStoreLookupTableModal.displayName = this.storeName;
        this.storeTaskMapStoreLookupTableModal.show();
    }
    openSelectTaskEventModal() {
        this.storeTaskMapTaskEventLookupTableModal.id = this.storeTaskMap.taskEventId;
        this.storeTaskMapTaskEventLookupTableModal.displayName = this.taskEventName;
        this.storeTaskMapTaskEventLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeTaskMap.storeId = null;
        this.storeName = '';
    }
    setTaskEventIdNull() {
        this.storeTaskMap.taskEventId = null;
        this.taskEventName = '';
    }

    getNewStoreId() {
        this.storeTaskMap.storeId = this.storeTaskMapStoreLookupTableModal.id;
        this.storeName = this.storeTaskMapStoreLookupTableModal.displayName;
    }
    getNewTaskEventId() {
        this.storeTaskMap.taskEventId = this.storeTaskMapTaskEventLookupTableModal.id;
        this.taskEventName = this.storeTaskMapTaskEventLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
