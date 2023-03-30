import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductFlashSaleProductMapsServiceProxy,
    CreateOrEditProductFlashSaleProductMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductFlashSaleProductMapProductLookupTableModalComponent } from './productFlashSaleProductMap-product-lookup-table-modal.component';
import { ProductFlashSaleProductMapStoreLookupTableModalComponent } from './productFlashSaleProductMap-store-lookup-table-modal.component';
import { ProductFlashSaleProductMapMembershipTypeLookupTableModalComponent } from './productFlashSaleProductMap-membershipType-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductFlashSaleProductMapModal',
    templateUrl: './create-or-edit-productFlashSaleProductMap-modal.component.html',
})
export class CreateOrEditProductFlashSaleProductMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productFlashSaleProductMapProductLookupTableModal', { static: true })
    productFlashSaleProductMapProductLookupTableModal: ProductFlashSaleProductMapProductLookupTableModalComponent;
    @ViewChild('productFlashSaleProductMapStoreLookupTableModal', { static: true })
    productFlashSaleProductMapStoreLookupTableModal: ProductFlashSaleProductMapStoreLookupTableModalComponent;
    @ViewChild('productFlashSaleProductMapMembershipTypeLookupTableModal', { static: true })
    productFlashSaleProductMapMembershipTypeLookupTableModal: ProductFlashSaleProductMapMembershipTypeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productFlashSaleProductMap: CreateOrEditProductFlashSaleProductMapDto =
        new CreateOrEditProductFlashSaleProductMapDto();

    productName = '';
    storeName = '';
    membershipTypeName = '';

    constructor(
        injector: Injector,
        private _productFlashSaleProductMapsServiceProxy: ProductFlashSaleProductMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productFlashSaleProductMapId?: number): void {
        if (!productFlashSaleProductMapId) {
            this.productFlashSaleProductMap = new CreateOrEditProductFlashSaleProductMapDto();
            this.productFlashSaleProductMap.id = productFlashSaleProductMapId;
            this.productFlashSaleProductMap.endDate = this._dateTimeService.getStartOfDay();
            this.productFlashSaleProductMap.startDate = this._dateTimeService.getStartOfDay();
            this.productName = '';
            this.storeName = '';
            this.membershipTypeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productFlashSaleProductMapsServiceProxy
                .getProductFlashSaleProductMapForEdit(productFlashSaleProductMapId)
                .subscribe((result) => {
                    this.productFlashSaleProductMap = result.productFlashSaleProductMap;

                    this.productName = result.productName;
                    this.storeName = result.storeName;
                    this.membershipTypeName = result.membershipTypeName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productFlashSaleProductMapsServiceProxy
            .createOrEdit(this.productFlashSaleProductMap)
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

    openSelectProductModal() {
        this.productFlashSaleProductMapProductLookupTableModal.id = this.productFlashSaleProductMap.productId;
        this.productFlashSaleProductMapProductLookupTableModal.displayName = this.productName;
        this.productFlashSaleProductMapProductLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.productFlashSaleProductMapStoreLookupTableModal.id = this.productFlashSaleProductMap.storeId;
        this.productFlashSaleProductMapStoreLookupTableModal.displayName = this.storeName;
        this.productFlashSaleProductMapStoreLookupTableModal.show();
    }
    openSelectMembershipTypeModal() {
        this.productFlashSaleProductMapMembershipTypeLookupTableModal.id =
            this.productFlashSaleProductMap.membershipTypeId;
        this.productFlashSaleProductMapMembershipTypeLookupTableModal.displayName = this.membershipTypeName;
        this.productFlashSaleProductMapMembershipTypeLookupTableModal.show();
    }

    setProductIdNull() {
        this.productFlashSaleProductMap.productId = null;
        this.productName = '';
    }
    setStoreIdNull() {
        this.productFlashSaleProductMap.storeId = null;
        this.storeName = '';
    }
    setMembershipTypeIdNull() {
        this.productFlashSaleProductMap.membershipTypeId = null;
        this.membershipTypeName = '';
    }

    getNewProductId() {
        this.productFlashSaleProductMap.productId = this.productFlashSaleProductMapProductLookupTableModal.id;
        this.productName = this.productFlashSaleProductMapProductLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.productFlashSaleProductMap.storeId = this.productFlashSaleProductMapStoreLookupTableModal.id;
        this.storeName = this.productFlashSaleProductMapStoreLookupTableModal.displayName;
    }
    getNewMembershipTypeId() {
        this.productFlashSaleProductMap.membershipTypeId =
            this.productFlashSaleProductMapMembershipTypeLookupTableModal.id;
        this.membershipTypeName = this.productFlashSaleProductMapMembershipTypeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
