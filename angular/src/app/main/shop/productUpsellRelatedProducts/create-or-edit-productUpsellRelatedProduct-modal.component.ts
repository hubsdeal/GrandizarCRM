import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductUpsellRelatedProductsServiceProxy,
    CreateOrEditProductUpsellRelatedProductDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductUpsellRelatedProductProductLookupTableModalComponent } from './productUpsellRelatedProduct-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductUpsellRelatedProductModal',
    templateUrl: './create-or-edit-productUpsellRelatedProduct-modal.component.html',
})
export class CreateOrEditProductUpsellRelatedProductModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productUpsellRelatedProductProductLookupTableModal', { static: true })
    productUpsellRelatedProductProductLookupTableModal: ProductUpsellRelatedProductProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productUpsellRelatedProduct: CreateOrEditProductUpsellRelatedProductDto =
        new CreateOrEditProductUpsellRelatedProductDto();

    productName = '';

    constructor(
        injector: Injector,
        private _productUpsellRelatedProductsServiceProxy: ProductUpsellRelatedProductsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productUpsellRelatedProductId?: number): void {
        if (!productUpsellRelatedProductId) {
            this.productUpsellRelatedProduct = new CreateOrEditProductUpsellRelatedProductDto();
            this.productUpsellRelatedProduct.id = productUpsellRelatedProductId;
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productUpsellRelatedProductsServiceProxy
                .getProductUpsellRelatedProductForEdit(productUpsellRelatedProductId)
                .subscribe((result) => {
                    this.productUpsellRelatedProduct = result.productUpsellRelatedProduct;

                    this.productName = result.productName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productUpsellRelatedProductsServiceProxy
            .createOrEdit(this.productUpsellRelatedProduct)
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
        this.productUpsellRelatedProductProductLookupTableModal.id = this.productUpsellRelatedProduct.primaryProductId;
        this.productUpsellRelatedProductProductLookupTableModal.displayName = this.productName;
        this.productUpsellRelatedProductProductLookupTableModal.show();
    }

    setPrimaryProductIdNull() {
        this.productUpsellRelatedProduct.primaryProductId = null;
        this.productName = '';
    }

    getNewPrimaryProductId() {
        this.productUpsellRelatedProduct.primaryProductId = this.productUpsellRelatedProductProductLookupTableModal.id;
        this.productName = this.productUpsellRelatedProductProductLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
