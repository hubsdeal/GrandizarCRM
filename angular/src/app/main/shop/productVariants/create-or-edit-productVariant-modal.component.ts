import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductVariantsServiceProxy, CreateOrEditProductVariantDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductVariantProductVariantCategoryLookupTableModalComponent } from './productVariant-productVariantCategory-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductVariantModal',
    templateUrl: './create-or-edit-productVariant-modal.component.html',
})
export class CreateOrEditProductVariantModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productVariantProductVariantCategoryLookupTableModal', { static: true })
    productVariantProductVariantCategoryLookupTableModal: ProductVariantProductVariantCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productVariant: CreateOrEditProductVariantDto = new CreateOrEditProductVariantDto();

    productVariantCategoryName = '';

    constructor(
        injector: Injector,
        private _productVariantsServiceProxy: ProductVariantsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productVariantId?: number): void {
        if (!productVariantId) {
            this.productVariant = new CreateOrEditProductVariantDto();
            this.productVariant.id = productVariantId;
            this.productVariantCategoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productVariantsServiceProxy.getProductVariantForEdit(productVariantId).subscribe((result) => {
                this.productVariant = result.productVariant;

                this.productVariantCategoryName = result.productVariantCategoryName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._productVariantsServiceProxy
            .createOrEdit(this.productVariant)
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

    openSelectProductVariantCategoryModal() {
        this.productVariantProductVariantCategoryLookupTableModal.id = this.productVariant.productVariantCategoryId;
        this.productVariantProductVariantCategoryLookupTableModal.displayName = this.productVariantCategoryName;
        this.productVariantProductVariantCategoryLookupTableModal.show();
    }

    setProductVariantCategoryIdNull() {
        this.productVariant.productVariantCategoryId = null;
        this.productVariantCategoryName = '';
    }

    getNewProductVariantCategoryId() {
        this.productVariant.productVariantCategoryId = this.productVariantProductVariantCategoryLookupTableModal.id;
        this.productVariantCategoryName = this.productVariantProductVariantCategoryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
