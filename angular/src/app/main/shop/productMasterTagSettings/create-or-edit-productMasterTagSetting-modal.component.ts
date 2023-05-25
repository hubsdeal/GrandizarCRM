import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductMasterTagSettingsServiceProxy,
    CreateOrEditProductMasterTagSettingDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductMasterTagSettingProductCategoryLookupTableModalComponent } from './productMasterTagSetting-productCategory-lookup-table-modal.component';
import { ProductMasterTagSettingMasterTagCategoryLookupTableModalComponent } from './productMasterTagSetting-masterTagCategory-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductMasterTagSettingModal',
    templateUrl: './create-or-edit-productMasterTagSetting-modal.component.html',
})
export class CreateOrEditProductMasterTagSettingModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productMasterTagSettingProductCategoryLookupTableModal', { static: true })
    productMasterTagSettingProductCategoryLookupTableModal: ProductMasterTagSettingProductCategoryLookupTableModalComponent;
    @ViewChild('productMasterTagSettingMasterTagCategoryLookupTableModal', { static: true })
    productMasterTagSettingMasterTagCategoryLookupTableModal: ProductMasterTagSettingMasterTagCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productMasterTagSetting: CreateOrEditProductMasterTagSettingDto = new CreateOrEditProductMasterTagSettingDto();

    productCategoryName = '';
    masterTagCategoryName = '';

    constructor(
        injector: Injector,
        private _productMasterTagSettingsServiceProxy: ProductMasterTagSettingsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productMasterTagSettingId?: number): void {
        if (!productMasterTagSettingId) {
            this.productMasterTagSetting = new CreateOrEditProductMasterTagSettingDto();
            this.productMasterTagSetting.id = productMasterTagSettingId;
            this.productCategoryName = '';
            this.masterTagCategoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productMasterTagSettingsServiceProxy
                .getProductMasterTagSettingForEdit(productMasterTagSettingId)
                .subscribe((result) => {
                    this.productMasterTagSetting = result.productMasterTagSetting;

                    this.productCategoryName = result.productCategoryName;
                    this.masterTagCategoryName = result.masterTagCategoryName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productMasterTagSettingsServiceProxy
            .createOrEdit(this.productMasterTagSetting)
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

    openSelectProductCategoryModal() {
        this.productMasterTagSettingProductCategoryLookupTableModal.id = this.productMasterTagSetting.productCategoryId;
        this.productMasterTagSettingProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.productMasterTagSettingProductCategoryLookupTableModal.show();
    }
    openSelectMasterTagCategoryModal() {
        this.productMasterTagSettingMasterTagCategoryLookupTableModal.id =
            this.productMasterTagSetting.masterTagCategoryId;
        this.productMasterTagSettingMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.productMasterTagSettingMasterTagCategoryLookupTableModal.show();
    }

    setProductCategoryIdNull() {
        this.productMasterTagSetting.productCategoryId = null;
        this.productCategoryName = '';
    }
    setMasterTagCategoryIdNull() {
        this.productMasterTagSetting.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }

    getNewProductCategoryId() {
        this.productMasterTagSetting.productCategoryId = this.productMasterTagSettingProductCategoryLookupTableModal.id;
        this.productCategoryName = this.productMasterTagSettingProductCategoryLookupTableModal.displayName;
    }
    getNewMasterTagCategoryId() {
        this.productMasterTagSetting.masterTagCategoryId =
            this.productMasterTagSettingMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.productMasterTagSettingMasterTagCategoryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
