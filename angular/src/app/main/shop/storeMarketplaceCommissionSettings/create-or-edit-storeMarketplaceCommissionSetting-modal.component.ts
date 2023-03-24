import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreMarketplaceCommissionSettingsServiceProxy,
    CreateOrEditStoreMarketplaceCommissionSettingDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreMarketplaceCommissionSettingStoreLookupTableModalComponent } from './storeMarketplaceCommissionSetting-store-lookup-table-modal.component';
import { StoreMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableModalComponent } from './storeMarketplaceCommissionSetting-marketplaceCommissionType-lookup-table-modal.component';
import { StoreMarketplaceCommissionSettingProductCategoryLookupTableModalComponent } from './storeMarketplaceCommissionSetting-productCategory-lookup-table-modal.component';
import { StoreMarketplaceCommissionSettingProductLookupTableModalComponent } from './storeMarketplaceCommissionSetting-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreMarketplaceCommissionSettingModal',
    templateUrl: './create-or-edit-storeMarketplaceCommissionSetting-modal.component.html',
})
export class CreateOrEditStoreMarketplaceCommissionSettingModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeMarketplaceCommissionSettingStoreLookupTableModal', { static: true })
    storeMarketplaceCommissionSettingStoreLookupTableModal: StoreMarketplaceCommissionSettingStoreLookupTableModalComponent;
    @ViewChild('storeMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableModal', { static: true })
    storeMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableModal: StoreMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableModalComponent;
    @ViewChild('storeMarketplaceCommissionSettingProductCategoryLookupTableModal', { static: true })
    storeMarketplaceCommissionSettingProductCategoryLookupTableModal: StoreMarketplaceCommissionSettingProductCategoryLookupTableModalComponent;
    @ViewChild('storeMarketplaceCommissionSettingProductLookupTableModal', { static: true })
    storeMarketplaceCommissionSettingProductLookupTableModal: StoreMarketplaceCommissionSettingProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeMarketplaceCommissionSetting: CreateOrEditStoreMarketplaceCommissionSettingDto =
        new CreateOrEditStoreMarketplaceCommissionSettingDto();

    storeName = '';
    marketplaceCommissionTypeName = '';
    productCategoryName = '';
    productName = '';

    constructor(
        injector: Injector,
        private _storeMarketplaceCommissionSettingsServiceProxy: StoreMarketplaceCommissionSettingsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeMarketplaceCommissionSettingId?: number): void {
        if (!storeMarketplaceCommissionSettingId) {
            this.storeMarketplaceCommissionSetting = new CreateOrEditStoreMarketplaceCommissionSettingDto();
            this.storeMarketplaceCommissionSetting.id = storeMarketplaceCommissionSettingId;
            this.storeMarketplaceCommissionSetting.startDate = this._dateTimeService.getStartOfDay();
            this.storeMarketplaceCommissionSetting.endDate = this._dateTimeService.getStartOfDay();
            this.storeName = '';
            this.marketplaceCommissionTypeName = '';
            this.productCategoryName = '';
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeMarketplaceCommissionSettingsServiceProxy
                .getStoreMarketplaceCommissionSettingForEdit(storeMarketplaceCommissionSettingId)
                .subscribe((result) => {
                    this.storeMarketplaceCommissionSetting = result.storeMarketplaceCommissionSetting;

                    this.storeName = result.storeName;
                    this.marketplaceCommissionTypeName = result.marketplaceCommissionTypeName;
                    this.productCategoryName = result.productCategoryName;
                    this.productName = result.productName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._storeMarketplaceCommissionSettingsServiceProxy
            .createOrEdit(this.storeMarketplaceCommissionSetting)
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
        this.storeMarketplaceCommissionSettingStoreLookupTableModal.id = this.storeMarketplaceCommissionSetting.storeId;
        this.storeMarketplaceCommissionSettingStoreLookupTableModal.displayName = this.storeName;
        this.storeMarketplaceCommissionSettingStoreLookupTableModal.show();
    }
    openSelectMarketplaceCommissionTypeModal() {
        this.storeMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableModal.id =
            this.storeMarketplaceCommissionSetting.marketplaceCommissionTypeId;
        this.storeMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableModal.displayName =
            this.marketplaceCommissionTypeName;
        this.storeMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableModal.show();
    }
    openSelectProductCategoryModal() {
        this.storeMarketplaceCommissionSettingProductCategoryLookupTableModal.id =
            this.storeMarketplaceCommissionSetting.productCategoryId;
        this.storeMarketplaceCommissionSettingProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.storeMarketplaceCommissionSettingProductCategoryLookupTableModal.show();
    }
    openSelectProductModal() {
        this.storeMarketplaceCommissionSettingProductLookupTableModal.id =
            this.storeMarketplaceCommissionSetting.productId;
        this.storeMarketplaceCommissionSettingProductLookupTableModal.displayName = this.productName;
        this.storeMarketplaceCommissionSettingProductLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeMarketplaceCommissionSetting.storeId = null;
        this.storeName = '';
    }
    setMarketplaceCommissionTypeIdNull() {
        this.storeMarketplaceCommissionSetting.marketplaceCommissionTypeId = null;
        this.marketplaceCommissionTypeName = '';
    }
    setProductCategoryIdNull() {
        this.storeMarketplaceCommissionSetting.productCategoryId = null;
        this.productCategoryName = '';
    }
    setProductIdNull() {
        this.storeMarketplaceCommissionSetting.productId = null;
        this.productName = '';
    }

    getNewStoreId() {
        this.storeMarketplaceCommissionSetting.storeId = this.storeMarketplaceCommissionSettingStoreLookupTableModal.id;
        this.storeName = this.storeMarketplaceCommissionSettingStoreLookupTableModal.displayName;
    }
    getNewMarketplaceCommissionTypeId() {
        this.storeMarketplaceCommissionSetting.marketplaceCommissionTypeId =
            this.storeMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableModal.id;
        this.marketplaceCommissionTypeName =
            this.storeMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableModal.displayName;
    }
    getNewProductCategoryId() {
        this.storeMarketplaceCommissionSetting.productCategoryId =
            this.storeMarketplaceCommissionSettingProductCategoryLookupTableModal.id;
        this.productCategoryName = this.storeMarketplaceCommissionSettingProductCategoryLookupTableModal.displayName;
    }
    getNewProductId() {
        this.storeMarketplaceCommissionSetting.productId =
            this.storeMarketplaceCommissionSettingProductLookupTableModal.id;
        this.productName = this.storeMarketplaceCommissionSettingProductLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
