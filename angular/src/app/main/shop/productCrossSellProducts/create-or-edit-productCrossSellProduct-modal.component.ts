import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductCrossSellProductsServiceProxy,
    CreateOrEditProductCrossSellProductDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductCrossSellProductProductLookupTableModalComponent } from './productCrossSellProduct-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductCrossSellProductModal',
    templateUrl: './create-or-edit-productCrossSellProduct-modal.component.html',
})
export class CreateOrEditProductCrossSellProductModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productCrossSellProductProductLookupTableModal', { static: true })
    productCrossSellProductProductLookupTableModal: ProductCrossSellProductProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productCrossSellProduct: CreateOrEditProductCrossSellProductDto = new CreateOrEditProductCrossSellProductDto();

    productName = '';

    constructor(
        injector: Injector,
        private _productCrossSellProductsServiceProxy: ProductCrossSellProductsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productCrossSellProductId?: number): void {
        if (!productCrossSellProductId) {
            this.productCrossSellProduct = new CreateOrEditProductCrossSellProductDto();
            this.productCrossSellProduct.id = productCrossSellProductId;
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productCrossSellProductsServiceProxy
                .getProductCrossSellProductForEdit(productCrossSellProductId)
                .subscribe((result) => {
                    this.productCrossSellProduct = result.productCrossSellProduct;

                    this.productName = result.productName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productCrossSellProductsServiceProxy
            .createOrEdit(this.productCrossSellProduct)
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
        this.productCrossSellProductProductLookupTableModal.id = this.productCrossSellProduct.primaryProductId;
        this.productCrossSellProductProductLookupTableModal.displayName = this.productName;
        this.productCrossSellProductProductLookupTableModal.show();
    }

    setPrimaryProductIdNull() {
        this.productCrossSellProduct.primaryProductId = null;
        this.productName = '';
    }

    getNewPrimaryProductId() {
        this.productCrossSellProduct.primaryProductId = this.productCrossSellProductProductLookupTableModal.id;
        this.productName = this.productCrossSellProductProductLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
