import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreProductMapsServiceProxy, CreateOrEditStoreProductMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreProductMapStoreLookupTableModalComponent } from './storeProductMap-store-lookup-table-modal.component';
import { StoreProductMapProductLookupTableModalComponent } from './storeProductMap-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreProductMapModal',
    templateUrl: './create-or-edit-storeProductMap-modal.component.html',
})
export class CreateOrEditStoreProductMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeProductMapStoreLookupTableModal', { static: true })
    storeProductMapStoreLookupTableModal: StoreProductMapStoreLookupTableModalComponent;
    @ViewChild('storeProductMapProductLookupTableModal', { static: true })
    storeProductMapProductLookupTableModal: StoreProductMapProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeProductMap: CreateOrEditStoreProductMapDto = new CreateOrEditStoreProductMapDto();

    storeName = '';
    productName = '';

    constructor(
        injector: Injector,
        private _storeProductMapsServiceProxy: StoreProductMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeProductMapId?: number): void {
        if (!storeProductMapId) {
            this.storeProductMap = new CreateOrEditStoreProductMapDto();
            this.storeProductMap.id = storeProductMapId;
            this.storeName = '';
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeProductMapsServiceProxy.getStoreProductMapForEdit(storeProductMapId).subscribe((result) => {
                this.storeProductMap = result.storeProductMap;

                this.storeName = result.storeName;
                this.productName = result.productName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._storeProductMapsServiceProxy
            .createOrEdit(this.storeProductMap)
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
        this.storeProductMapStoreLookupTableModal.id = this.storeProductMap.storeId;
        this.storeProductMapStoreLookupTableModal.displayName = this.storeName;
        this.storeProductMapStoreLookupTableModal.show();
    }
    openSelectProductModal() {
        this.storeProductMapProductLookupTableModal.id = this.storeProductMap.productId;
        this.storeProductMapProductLookupTableModal.displayName = this.productName;
        this.storeProductMapProductLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeProductMap.storeId = null;
        this.storeName = '';
    }
    setProductIdNull() {
        this.storeProductMap.productId = null;
        this.productName = '';
    }

    getNewStoreId() {
        this.storeProductMap.storeId = this.storeProductMapStoreLookupTableModal.id;
        this.storeName = this.storeProductMapStoreLookupTableModal.displayName;
    }
    getNewProductId() {
        this.storeProductMap.productId = this.storeProductMapProductLookupTableModal.id;
        this.productName = this.storeProductMapProductLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
