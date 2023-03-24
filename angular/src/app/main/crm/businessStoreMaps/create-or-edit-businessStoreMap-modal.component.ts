import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    BusinessStoreMapsServiceProxy,
    CreateOrEditBusinessStoreMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BusinessStoreMapBusinessLookupTableModalComponent } from './businessStoreMap-business-lookup-table-modal.component';
import { BusinessStoreMapStoreLookupTableModalComponent } from './businessStoreMap-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditBusinessStoreMapModal',
    templateUrl: './create-or-edit-businessStoreMap-modal.component.html',
})
export class CreateOrEditBusinessStoreMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('businessStoreMapBusinessLookupTableModal', { static: true })
    businessStoreMapBusinessLookupTableModal: BusinessStoreMapBusinessLookupTableModalComponent;
    @ViewChild('businessStoreMapStoreLookupTableModal', { static: true })
    businessStoreMapStoreLookupTableModal: BusinessStoreMapStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    businessStoreMap: CreateOrEditBusinessStoreMapDto = new CreateOrEditBusinessStoreMapDto();

    businessName = '';
    storeName = '';

    constructor(
        injector: Injector,
        private _businessStoreMapsServiceProxy: BusinessStoreMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(businessStoreMapId?: number): void {
        if (!businessStoreMapId) {
            this.businessStoreMap = new CreateOrEditBusinessStoreMapDto();
            this.businessStoreMap.id = businessStoreMapId;
            this.businessName = '';
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._businessStoreMapsServiceProxy.getBusinessStoreMapForEdit(businessStoreMapId).subscribe((result) => {
                this.businessStoreMap = result.businessStoreMap;

                this.businessName = result.businessName;
                this.storeName = result.storeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._businessStoreMapsServiceProxy
            .createOrEdit(this.businessStoreMap)
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

    openSelectBusinessModal() {
        this.businessStoreMapBusinessLookupTableModal.id = this.businessStoreMap.businessId;
        this.businessStoreMapBusinessLookupTableModal.displayName = this.businessName;
        this.businessStoreMapBusinessLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.businessStoreMapStoreLookupTableModal.id = this.businessStoreMap.storeId;
        this.businessStoreMapStoreLookupTableModal.displayName = this.storeName;
        this.businessStoreMapStoreLookupTableModal.show();
    }

    setBusinessIdNull() {
        this.businessStoreMap.businessId = null;
        this.businessName = '';
    }
    setStoreIdNull() {
        this.businessStoreMap.storeId = null;
        this.storeName = '';
    }

    getNewBusinessId() {
        this.businessStoreMap.businessId = this.businessStoreMapBusinessLookupTableModal.id;
        this.businessName = this.businessStoreMapBusinessLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.businessStoreMap.storeId = this.businessStoreMapStoreLookupTableModal.id;
        this.storeName = this.businessStoreMapStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
