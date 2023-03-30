import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductByVariantsServiceProxy,
    CreateOrEditProductByVariantDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductByVariantProductLookupTableModalComponent } from './productByVariant-product-lookup-table-modal.component';
import { ProductByVariantProductVariantLookupTableModalComponent } from './productByVariant-productVariant-lookup-table-modal.component';
import { ProductByVariantProductVariantCategoryLookupTableModalComponent } from './productByVariant-productVariantCategory-lookup-table-modal.component';
import { ProductByVariantMediaLibraryLookupTableModalComponent } from './productByVariant-mediaLibrary-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductByVariantModal',
    templateUrl: './create-or-edit-productByVariant-modal.component.html',
})
export class CreateOrEditProductByVariantModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productByVariantProductLookupTableModal', { static: true })
    productByVariantProductLookupTableModal: ProductByVariantProductLookupTableModalComponent;
    @ViewChild('productByVariantProductVariantLookupTableModal', { static: true })
    productByVariantProductVariantLookupTableModal: ProductByVariantProductVariantLookupTableModalComponent;
    @ViewChild('productByVariantProductVariantCategoryLookupTableModal', { static: true })
    productByVariantProductVariantCategoryLookupTableModal: ProductByVariantProductVariantCategoryLookupTableModalComponent;
    @ViewChild('productByVariantMediaLibraryLookupTableModal', { static: true })
    productByVariantMediaLibraryLookupTableModal: ProductByVariantMediaLibraryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productByVariant: CreateOrEditProductByVariantDto = new CreateOrEditProductByVariantDto();

    productName = '';
    productVariantName = '';
    productVariantCategoryName = '';
    mediaLibraryName = '';

    constructor(
        injector: Injector,
        private _productByVariantsServiceProxy: ProductByVariantsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productByVariantId?: number): void {
        if (!productByVariantId) {
            this.productByVariant = new CreateOrEditProductByVariantDto();
            this.productByVariant.id = productByVariantId;
            this.productName = '';
            this.productVariantName = '';
            this.productVariantCategoryName = '';
            this.mediaLibraryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productByVariantsServiceProxy.getProductByVariantForEdit(productByVariantId).subscribe((result) => {
                this.productByVariant = result.productByVariant;

                this.productName = result.productName;
                this.productVariantName = result.productVariantName;
                this.productVariantCategoryName = result.productVariantCategoryName;
                this.mediaLibraryName = result.mediaLibraryName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._productByVariantsServiceProxy
            .createOrEdit(this.productByVariant)
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
        this.productByVariantProductLookupTableModal.id = this.productByVariant.productId;
        this.productByVariantProductLookupTableModal.displayName = this.productName;
        this.productByVariantProductLookupTableModal.show();
    }
    openSelectProductVariantModal() {
        this.productByVariantProductVariantLookupTableModal.id = this.productByVariant.productVariantId;
        this.productByVariantProductVariantLookupTableModal.displayName = this.productVariantName;
        this.productByVariantProductVariantLookupTableModal.show();
    }
    openSelectProductVariantCategoryModal() {
        this.productByVariantProductVariantCategoryLookupTableModal.id = this.productByVariant.productVariantCategoryId;
        this.productByVariantProductVariantCategoryLookupTableModal.displayName = this.productVariantCategoryName;
        this.productByVariantProductVariantCategoryLookupTableModal.show();
    }
    openSelectMediaLibraryModal() {
        this.productByVariantMediaLibraryLookupTableModal.id = this.productByVariant.mediaLibraryId;
        this.productByVariantMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.productByVariantMediaLibraryLookupTableModal.show();
    }

    setProductIdNull() {
        this.productByVariant.productId = null;
        this.productName = '';
    }
    setProductVariantIdNull() {
        this.productByVariant.productVariantId = null;
        this.productVariantName = '';
    }
    setProductVariantCategoryIdNull() {
        this.productByVariant.productVariantCategoryId = null;
        this.productVariantCategoryName = '';
    }
    setMediaLibraryIdNull() {
        this.productByVariant.mediaLibraryId = null;
        this.mediaLibraryName = '';
    }

    getNewProductId() {
        this.productByVariant.productId = this.productByVariantProductLookupTableModal.id;
        this.productName = this.productByVariantProductLookupTableModal.displayName;
    }
    getNewProductVariantId() {
        this.productByVariant.productVariantId = this.productByVariantProductVariantLookupTableModal.id;
        this.productVariantName = this.productByVariantProductVariantLookupTableModal.displayName;
    }
    getNewProductVariantCategoryId() {
        this.productByVariant.productVariantCategoryId = this.productByVariantProductVariantCategoryLookupTableModal.id;
        this.productVariantCategoryName = this.productByVariantProductVariantCategoryLookupTableModal.displayName;
    }
    getNewMediaLibraryId() {
        this.productByVariant.mediaLibraryId = this.productByVariantMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.productByVariantMediaLibraryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
