import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreWidgetProductMapsServiceProxy,
    CreateOrEditStoreWidgetProductMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreWidgetProductMapStoreWidgetMapLookupTableModalComponent } from './storeWidgetProductMap-storeWidgetMap-lookup-table-modal.component';
import { StoreWidgetProductMapProductLookupTableModalComponent } from './storeWidgetProductMap-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreWidgetProductMapModal',
    templateUrl: './create-or-edit-storeWidgetProductMap-modal.component.html',
})
export class CreateOrEditStoreWidgetProductMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeWidgetProductMapStoreWidgetMapLookupTableModal', { static: true })
    storeWidgetProductMapStoreWidgetMapLookupTableModal: StoreWidgetProductMapStoreWidgetMapLookupTableModalComponent;
    @ViewChild('storeWidgetProductMapProductLookupTableModal', { static: true })
    storeWidgetProductMapProductLookupTableModal: StoreWidgetProductMapProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeWidgetProductMap: CreateOrEditStoreWidgetProductMapDto = new CreateOrEditStoreWidgetProductMapDto();

    storeWidgetMapCustomName = '';
    productName = '';

    constructor(
        injector: Injector,
        private _storeWidgetProductMapsServiceProxy: StoreWidgetProductMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeWidgetProductMapId?: number): void {
        if (!storeWidgetProductMapId) {
            this.storeWidgetProductMap = new CreateOrEditStoreWidgetProductMapDto();
            this.storeWidgetProductMap.id = storeWidgetProductMapId;
            this.storeWidgetMapCustomName = '';
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeWidgetProductMapsServiceProxy
                .getStoreWidgetProductMapForEdit(storeWidgetProductMapId)
                .subscribe((result) => {
                    this.storeWidgetProductMap = result.storeWidgetProductMap;

                    this.storeWidgetMapCustomName = result.storeWidgetMapCustomName;
                    this.productName = result.productName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._storeWidgetProductMapsServiceProxy
            .createOrEdit(this.storeWidgetProductMap)
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

    openSelectStoreWidgetMapModal() {
        this.storeWidgetProductMapStoreWidgetMapLookupTableModal.id = this.storeWidgetProductMap.storeWidgetMapId;
        this.storeWidgetProductMapStoreWidgetMapLookupTableModal.displayName = this.storeWidgetMapCustomName;
        this.storeWidgetProductMapStoreWidgetMapLookupTableModal.show();
    }
    openSelectProductModal() {
        this.storeWidgetProductMapProductLookupTableModal.id = this.storeWidgetProductMap.productId;
        this.storeWidgetProductMapProductLookupTableModal.displayName = this.productName;
        this.storeWidgetProductMapProductLookupTableModal.show();
    }

    setStoreWidgetMapIdNull() {
        this.storeWidgetProductMap.storeWidgetMapId = null;
        this.storeWidgetMapCustomName = '';
    }
    setProductIdNull() {
        this.storeWidgetProductMap.productId = null;
        this.productName = '';
    }

    getNewStoreWidgetMapId() {
        this.storeWidgetProductMap.storeWidgetMapId = this.storeWidgetProductMapStoreWidgetMapLookupTableModal.id;
        this.storeWidgetMapCustomName = this.storeWidgetProductMapStoreWidgetMapLookupTableModal.displayName;
    }
    getNewProductId() {
        this.storeWidgetProductMap.productId = this.storeWidgetProductMapProductLookupTableModal.id;
        this.productName = this.storeWidgetProductMapProductLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
