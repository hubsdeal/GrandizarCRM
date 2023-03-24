import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreProductCategoryMapsServiceProxy,
    CreateOrEditStoreProductCategoryMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreProductCategoryMapStoreLookupTableModalComponent } from './storeProductCategoryMap-store-lookup-table-modal.component';
import { StoreProductCategoryMapProductCategoryLookupTableModalComponent } from './storeProductCategoryMap-productCategory-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreProductCategoryMapModal',
    templateUrl: './create-or-edit-storeProductCategoryMap-modal.component.html',
})
export class CreateOrEditStoreProductCategoryMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeProductCategoryMapStoreLookupTableModal', { static: true })
    storeProductCategoryMapStoreLookupTableModal: StoreProductCategoryMapStoreLookupTableModalComponent;
    @ViewChild('storeProductCategoryMapProductCategoryLookupTableModal', { static: true })
    storeProductCategoryMapProductCategoryLookupTableModal: StoreProductCategoryMapProductCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeProductCategoryMap: CreateOrEditStoreProductCategoryMapDto = new CreateOrEditStoreProductCategoryMapDto();

    storeName = '';
    productCategoryName = '';

    constructor(
        injector: Injector,
        private _storeProductCategoryMapsServiceProxy: StoreProductCategoryMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeProductCategoryMapId?: number): void {
        if (!storeProductCategoryMapId) {
            this.storeProductCategoryMap = new CreateOrEditStoreProductCategoryMapDto();
            this.storeProductCategoryMap.id = storeProductCategoryMapId;
            this.storeName = '';
            this.productCategoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeProductCategoryMapsServiceProxy
                .getStoreProductCategoryMapForEdit(storeProductCategoryMapId)
                .subscribe((result) => {
                    this.storeProductCategoryMap = result.storeProductCategoryMap;

                    this.storeName = result.storeName;
                    this.productCategoryName = result.productCategoryName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._storeProductCategoryMapsServiceProxy
            .createOrEdit(this.storeProductCategoryMap)
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
        this.storeProductCategoryMapStoreLookupTableModal.id = this.storeProductCategoryMap.storeId;
        this.storeProductCategoryMapStoreLookupTableModal.displayName = this.storeName;
        this.storeProductCategoryMapStoreLookupTableModal.show();
    }
    openSelectProductCategoryModal() {
        this.storeProductCategoryMapProductCategoryLookupTableModal.id = this.storeProductCategoryMap.productCategoryId;
        this.storeProductCategoryMapProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.storeProductCategoryMapProductCategoryLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeProductCategoryMap.storeId = null;
        this.storeName = '';
    }
    setProductCategoryIdNull() {
        this.storeProductCategoryMap.productCategoryId = null;
        this.productCategoryName = '';
    }

    getNewStoreId() {
        this.storeProductCategoryMap.storeId = this.storeProductCategoryMapStoreLookupTableModal.id;
        this.storeName = this.storeProductCategoryMapStoreLookupTableModal.displayName;
    }
    getNewProductCategoryId() {
        this.storeProductCategoryMap.productCategoryId = this.storeProductCategoryMapProductCategoryLookupTableModal.id;
        this.productCategoryName = this.storeProductCategoryMapProductCategoryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
