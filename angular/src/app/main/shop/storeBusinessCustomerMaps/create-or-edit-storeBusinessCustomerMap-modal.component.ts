import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreBusinessCustomerMapsServiceProxy,
    CreateOrEditStoreBusinessCustomerMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreBusinessCustomerMapStoreLookupTableModalComponent } from './storeBusinessCustomerMap-store-lookup-table-modal.component';
import { StoreBusinessCustomerMapBusinessLookupTableModalComponent } from './storeBusinessCustomerMap-business-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreBusinessCustomerMapModal',
    templateUrl: './create-or-edit-storeBusinessCustomerMap-modal.component.html',
})
export class CreateOrEditStoreBusinessCustomerMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeBusinessCustomerMapStoreLookupTableModal', { static: true })
    storeBusinessCustomerMapStoreLookupTableModal: StoreBusinessCustomerMapStoreLookupTableModalComponent;
    @ViewChild('storeBusinessCustomerMapBusinessLookupTableModal', { static: true })
    storeBusinessCustomerMapBusinessLookupTableModal: StoreBusinessCustomerMapBusinessLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeBusinessCustomerMap: CreateOrEditStoreBusinessCustomerMapDto = new CreateOrEditStoreBusinessCustomerMapDto();

    storeName = '';
    businessName = '';

    constructor(
        injector: Injector,
        private _storeBusinessCustomerMapsServiceProxy: StoreBusinessCustomerMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeBusinessCustomerMapId?: number): void {
        if (!storeBusinessCustomerMapId) {
            this.storeBusinessCustomerMap = new CreateOrEditStoreBusinessCustomerMapDto();
            this.storeBusinessCustomerMap.id = storeBusinessCustomerMapId;
            this.storeName = '';
            this.businessName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeBusinessCustomerMapsServiceProxy
                .getStoreBusinessCustomerMapForEdit(storeBusinessCustomerMapId)
                .subscribe((result) => {
                    this.storeBusinessCustomerMap = result.storeBusinessCustomerMap;

                    this.storeName = result.storeName;
                    this.businessName = result.businessName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._storeBusinessCustomerMapsServiceProxy
            .createOrEdit(this.storeBusinessCustomerMap)
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
        this.storeBusinessCustomerMapStoreLookupTableModal.id = this.storeBusinessCustomerMap.storeId;
        this.storeBusinessCustomerMapStoreLookupTableModal.displayName = this.storeName;
        this.storeBusinessCustomerMapStoreLookupTableModal.show();
    }
    openSelectBusinessModal() {
        this.storeBusinessCustomerMapBusinessLookupTableModal.id = this.storeBusinessCustomerMap.businessId;
        this.storeBusinessCustomerMapBusinessLookupTableModal.displayName = this.businessName;
        this.storeBusinessCustomerMapBusinessLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeBusinessCustomerMap.storeId = null;
        this.storeName = '';
    }
    setBusinessIdNull() {
        this.storeBusinessCustomerMap.businessId = null;
        this.businessName = '';
    }

    getNewStoreId() {
        this.storeBusinessCustomerMap.storeId = this.storeBusinessCustomerMapStoreLookupTableModal.id;
        this.storeName = this.storeBusinessCustomerMapStoreLookupTableModal.displayName;
    }
    getNewBusinessId() {
        this.storeBusinessCustomerMap.businessId = this.storeBusinessCustomerMapBusinessLookupTableModal.id;
        this.businessName = this.storeBusinessCustomerMapBusinessLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
