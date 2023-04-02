import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { DiscountCodeMapsServiceProxy, CreateOrEditDiscountCodeMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DiscountCodeMapDiscountCodeGeneratorLookupTableModalComponent } from './discountCodeMap-discountCodeGenerator-lookup-table-modal.component';
import { DiscountCodeMapStoreLookupTableModalComponent } from './discountCodeMap-store-lookup-table-modal.component';
import { DiscountCodeMapProductLookupTableModalComponent } from './discountCodeMap-product-lookup-table-modal.component';
import { DiscountCodeMapMembershipTypeLookupTableModalComponent } from './discountCodeMap-membershipType-lookup-table-modal.component';

@Component({
    selector: 'createOrEditDiscountCodeMapModal',
    templateUrl: './create-or-edit-discountCodeMap-modal.component.html',
})
export class CreateOrEditDiscountCodeMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('discountCodeMapDiscountCodeGeneratorLookupTableModal', { static: true })
    discountCodeMapDiscountCodeGeneratorLookupTableModal: DiscountCodeMapDiscountCodeGeneratorLookupTableModalComponent;
    @ViewChild('discountCodeMapStoreLookupTableModal', { static: true })
    discountCodeMapStoreLookupTableModal: DiscountCodeMapStoreLookupTableModalComponent;
    @ViewChild('discountCodeMapProductLookupTableModal', { static: true })
    discountCodeMapProductLookupTableModal: DiscountCodeMapProductLookupTableModalComponent;
    @ViewChild('discountCodeMapMembershipTypeLookupTableModal', { static: true })
    discountCodeMapMembershipTypeLookupTableModal: DiscountCodeMapMembershipTypeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    discountCodeMap: CreateOrEditDiscountCodeMapDto = new CreateOrEditDiscountCodeMapDto();

    discountCodeGeneratorName = '';
    storeName = '';
    productName = '';
    membershipTypeName = '';

    constructor(
        injector: Injector,
        private _discountCodeMapsServiceProxy: DiscountCodeMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(discountCodeMapId?: number): void {
        if (!discountCodeMapId) {
            this.discountCodeMap = new CreateOrEditDiscountCodeMapDto();
            this.discountCodeMap.id = discountCodeMapId;
            this.discountCodeGeneratorName = '';
            this.storeName = '';
            this.productName = '';
            this.membershipTypeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._discountCodeMapsServiceProxy.getDiscountCodeMapForEdit(discountCodeMapId).subscribe((result) => {
                this.discountCodeMap = result.discountCodeMap;

                this.discountCodeGeneratorName = result.discountCodeGeneratorName;
                this.storeName = result.storeName;
                this.productName = result.productName;
                this.membershipTypeName = result.membershipTypeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._discountCodeMapsServiceProxy
            .createOrEdit(this.discountCodeMap)
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

    openSelectDiscountCodeGeneratorModal() {
        this.discountCodeMapDiscountCodeGeneratorLookupTableModal.id = this.discountCodeMap.discountCodeGeneratorId;
        this.discountCodeMapDiscountCodeGeneratorLookupTableModal.displayName = this.discountCodeGeneratorName;
        this.discountCodeMapDiscountCodeGeneratorLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.discountCodeMapStoreLookupTableModal.id = this.discountCodeMap.storeId;
        this.discountCodeMapStoreLookupTableModal.displayName = this.storeName;
        this.discountCodeMapStoreLookupTableModal.show();
    }
    openSelectProductModal() {
        this.discountCodeMapProductLookupTableModal.id = this.discountCodeMap.productId;
        this.discountCodeMapProductLookupTableModal.displayName = this.productName;
        this.discountCodeMapProductLookupTableModal.show();
    }
    openSelectMembershipTypeModal() {
        this.discountCodeMapMembershipTypeLookupTableModal.id = this.discountCodeMap.membershipTypeId;
        this.discountCodeMapMembershipTypeLookupTableModal.displayName = this.membershipTypeName;
        this.discountCodeMapMembershipTypeLookupTableModal.show();
    }

    setDiscountCodeGeneratorIdNull() {
        this.discountCodeMap.discountCodeGeneratorId = null;
        this.discountCodeGeneratorName = '';
    }
    setStoreIdNull() {
        this.discountCodeMap.storeId = null;
        this.storeName = '';
    }
    setProductIdNull() {
        this.discountCodeMap.productId = null;
        this.productName = '';
    }
    setMembershipTypeIdNull() {
        this.discountCodeMap.membershipTypeId = null;
        this.membershipTypeName = '';
    }

    getNewDiscountCodeGeneratorId() {
        this.discountCodeMap.discountCodeGeneratorId = this.discountCodeMapDiscountCodeGeneratorLookupTableModal.id;
        this.discountCodeGeneratorName = this.discountCodeMapDiscountCodeGeneratorLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.discountCodeMap.storeId = this.discountCodeMapStoreLookupTableModal.id;
        this.storeName = this.discountCodeMapStoreLookupTableModal.displayName;
    }
    getNewProductId() {
        this.discountCodeMap.productId = this.discountCodeMapProductLookupTableModal.id;
        this.productName = this.discountCodeMapProductLookupTableModal.displayName;
    }
    getNewMembershipTypeId() {
        this.discountCodeMap.membershipTypeId = this.discountCodeMapMembershipTypeLookupTableModal.id;
        this.membershipTypeName = this.discountCodeMapMembershipTypeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
