import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductVariantCategoriesServiceProxy,
    CreateOrEditProductVariantCategoryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductVariantCategoryStoreLookupTableModalComponent } from './productVariantCategory-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductVariantCategoryModal',
    templateUrl: './create-or-edit-productVariantCategory-modal.component.html',
})
export class CreateOrEditProductVariantCategoryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productVariantCategoryStoreLookupTableModal', { static: true })
    productVariantCategoryStoreLookupTableModal: ProductVariantCategoryStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productVariantCategory: CreateOrEditProductVariantCategoryDto = new CreateOrEditProductVariantCategoryDto();

    storeName = '';

    constructor(
        injector: Injector,
        private _productVariantCategoriesServiceProxy: ProductVariantCategoriesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productVariantCategoryId?: number): void {
        if (!productVariantCategoryId) {
            this.productVariantCategory = new CreateOrEditProductVariantCategoryDto();
            this.productVariantCategory.id = productVariantCategoryId;
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productVariantCategoriesServiceProxy
                .getProductVariantCategoryForEdit(productVariantCategoryId)
                .subscribe((result) => {
                    this.productVariantCategory = result.productVariantCategory;

                    this.storeName = result.storeName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productVariantCategoriesServiceProxy
            .createOrEdit(this.productVariantCategory)
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
        this.productVariantCategoryStoreLookupTableModal.id = this.productVariantCategory.storeId;
        this.productVariantCategoryStoreLookupTableModal.displayName = this.storeName;
        this.productVariantCategoryStoreLookupTableModal.show();
    }

    setStoreIdNull() {
        this.productVariantCategory.storeId = null;
        this.storeName = '';
    }

    getNewStoreId() {
        this.productVariantCategory.storeId = this.productVariantCategoryStoreLookupTableModal.id;
        this.storeName = this.productVariantCategoryStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
