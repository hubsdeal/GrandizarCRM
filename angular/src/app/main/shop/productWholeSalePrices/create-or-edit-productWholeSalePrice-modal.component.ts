import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductWholeSalePricesServiceProxy,
    CreateOrEditProductWholeSalePriceDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductWholeSalePriceProductLookupTableModalComponent } from './productWholeSalePrice-product-lookup-table-modal.component';
import { ProductWholeSalePriceProductWholeSaleQuantityTypeLookupTableModalComponent } from './productWholeSalePrice-productWholeSaleQuantityType-lookup-table-modal.component';
import { ProductWholeSalePriceMeasurementUnitLookupTableModalComponent } from './productWholeSalePrice-measurementUnit-lookup-table-modal.component';
import { ProductWholeSalePriceCurrencyLookupTableModalComponent } from './productWholeSalePrice-currency-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductWholeSalePriceModal',
    templateUrl: './create-or-edit-productWholeSalePrice-modal.component.html',
})
export class CreateOrEditProductWholeSalePriceModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productWholeSalePriceProductLookupTableModal', { static: true })
    productWholeSalePriceProductLookupTableModal: ProductWholeSalePriceProductLookupTableModalComponent;
    @ViewChild('productWholeSalePriceProductWholeSaleQuantityTypeLookupTableModal', { static: true })
    productWholeSalePriceProductWholeSaleQuantityTypeLookupTableModal: ProductWholeSalePriceProductWholeSaleQuantityTypeLookupTableModalComponent;
    @ViewChild('productWholeSalePriceMeasurementUnitLookupTableModal', { static: true })
    productWholeSalePriceMeasurementUnitLookupTableModal: ProductWholeSalePriceMeasurementUnitLookupTableModalComponent;
    @ViewChild('productWholeSalePriceCurrencyLookupTableModal', { static: true })
    productWholeSalePriceCurrencyLookupTableModal: ProductWholeSalePriceCurrencyLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productWholeSalePrice: CreateOrEditProductWholeSalePriceDto = new CreateOrEditProductWholeSalePriceDto();

    productName = '';
    productWholeSaleQuantityTypeName = '';
    measurementUnitName = '';
    currencyName = '';

    constructor(
        injector: Injector,
        private _productWholeSalePricesServiceProxy: ProductWholeSalePricesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productWholeSalePriceId?: number): void {
        if (!productWholeSalePriceId) {
            this.productWholeSalePrice = new CreateOrEditProductWholeSalePriceDto();
            this.productWholeSalePrice.id = productWholeSalePriceId;
            this.productName = '';
            this.productWholeSaleQuantityTypeName = '';
            this.measurementUnitName = '';
            this.currencyName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productWholeSalePricesServiceProxy
                .getProductWholeSalePriceForEdit(productWholeSalePriceId)
                .subscribe((result) => {
                    this.productWholeSalePrice = result.productWholeSalePrice;

                    this.productName = result.productName;
                    this.productWholeSaleQuantityTypeName = result.productWholeSaleQuantityTypeName;
                    this.measurementUnitName = result.measurementUnitName;
                    this.currencyName = result.currencyName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productWholeSalePricesServiceProxy
            .createOrEdit(this.productWholeSalePrice)
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
        this.productWholeSalePriceProductLookupTableModal.id = this.productWholeSalePrice.productId;
        this.productWholeSalePriceProductLookupTableModal.displayName = this.productName;
        this.productWholeSalePriceProductLookupTableModal.show();
    }
    openSelectProductWholeSaleQuantityTypeModal() {
        this.productWholeSalePriceProductWholeSaleQuantityTypeLookupTableModal.id =
            this.productWholeSalePrice.productWholeSaleQuantityTypeId;
        this.productWholeSalePriceProductWholeSaleQuantityTypeLookupTableModal.displayName =
            this.productWholeSaleQuantityTypeName;
        this.productWholeSalePriceProductWholeSaleQuantityTypeLookupTableModal.show();
    }
    openSelectMeasurementUnitModal() {
        this.productWholeSalePriceMeasurementUnitLookupTableModal.id = this.productWholeSalePrice.measurementUnitId;
        this.productWholeSalePriceMeasurementUnitLookupTableModal.displayName = this.measurementUnitName;
        this.productWholeSalePriceMeasurementUnitLookupTableModal.show();
    }
    openSelectCurrencyModal() {
        this.productWholeSalePriceCurrencyLookupTableModal.id = this.productWholeSalePrice.currencyId;
        this.productWholeSalePriceCurrencyLookupTableModal.displayName = this.currencyName;
        this.productWholeSalePriceCurrencyLookupTableModal.show();
    }

    setProductIdNull() {
        this.productWholeSalePrice.productId = null;
        this.productName = '';
    }
    setProductWholeSaleQuantityTypeIdNull() {
        this.productWholeSalePrice.productWholeSaleQuantityTypeId = null;
        this.productWholeSaleQuantityTypeName = '';
    }
    setMeasurementUnitIdNull() {
        this.productWholeSalePrice.measurementUnitId = null;
        this.measurementUnitName = '';
    }
    setCurrencyIdNull() {
        this.productWholeSalePrice.currencyId = null;
        this.currencyName = '';
    }

    getNewProductId() {
        this.productWholeSalePrice.productId = this.productWholeSalePriceProductLookupTableModal.id;
        this.productName = this.productWholeSalePriceProductLookupTableModal.displayName;
    }
    getNewProductWholeSaleQuantityTypeId() {
        this.productWholeSalePrice.productWholeSaleQuantityTypeId =
            this.productWholeSalePriceProductWholeSaleQuantityTypeLookupTableModal.id;
        this.productWholeSaleQuantityTypeName =
            this.productWholeSalePriceProductWholeSaleQuantityTypeLookupTableModal.displayName;
    }
    getNewMeasurementUnitId() {
        this.productWholeSalePrice.measurementUnitId = this.productWholeSalePriceMeasurementUnitLookupTableModal.id;
        this.measurementUnitName = this.productWholeSalePriceMeasurementUnitLookupTableModal.displayName;
    }
    getNewCurrencyId() {
        this.productWholeSalePrice.currencyId = this.productWholeSalePriceCurrencyLookupTableModal.id;
        this.currencyName = this.productWholeSalePriceCurrencyLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
