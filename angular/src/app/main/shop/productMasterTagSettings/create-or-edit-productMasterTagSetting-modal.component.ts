import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductMasterTagSettingsServiceProxy,
    CreateOrEditProductMasterTagSettingDto,
    ProductsServiceProxy,
    AnswerType,
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

    productCategoryOptions: any = []
    selectedProductCategory: any;

    selectedTagCategory: any;
    masterTagCategoryOptions: any = []
    answerTypeOptions: { value: any, label: string }[] = [];

    constructor(
        injector: Injector,
        private _productMasterTagSettingsServiceProxy: ProductMasterTagSettingsServiceProxy,
        private _productsServiceProxy: ProductsServiceProxy,
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
        this._productsServiceProxy.getAllProductCategoryForLookupTable('', '', 0, 10000).subscribe(result => {
            this.productCategoryOptions = result.items;
        });

        this._productMasterTagSettingsServiceProxy.getAllMasterTagCategoryForLookupTable('', '', 0, 1000).subscribe(result => {
            this.masterTagCategoryOptions = result.items;
        })
        for (const key in AnswerType) {
            if (isNaN(Number(key))) {
                this.answerTypeOptions.push({ value: AnswerType[key], label: key.replace(/_/g, ' ') });
            }
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

    onProductCategoryClick(event: any) {
        if (event.value != null) {
            this.productMasterTagSetting.productCategoryId = event.value.id;
        }
    }

    onMasterTagCategoryClick(event: any) {
        if (event.value != null) {
            this.productMasterTagSetting.masterTagCategoryId = event.value.id;
            this.productMasterTagSetting.customTagTitle = event.value.displayName;
            this.productMasterTagSetting.customTagChatQuestion = 'What is'+ ' ' + event.value.displayName + '?';
        }
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

    ngOnInit(): void { }
}
