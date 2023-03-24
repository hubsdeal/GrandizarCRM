import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductTagsServiceProxy, CreateOrEditProductTagDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductTagProductLookupTableModalComponent } from './productTag-product-lookup-table-modal.component';
import { ProductTagMasterTagCategoryLookupTableModalComponent } from './productTag-masterTagCategory-lookup-table-modal.component';
import { ProductTagMasterTagLookupTableModalComponent } from './productTag-masterTag-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductTagModal',
    templateUrl: './create-or-edit-productTag-modal.component.html',
})
export class CreateOrEditProductTagModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productTagProductLookupTableModal', { static: true })
    productTagProductLookupTableModal: ProductTagProductLookupTableModalComponent;
    @ViewChild('productTagMasterTagCategoryLookupTableModal', { static: true })
    productTagMasterTagCategoryLookupTableModal: ProductTagMasterTagCategoryLookupTableModalComponent;
    @ViewChild('productTagMasterTagLookupTableModal', { static: true })
    productTagMasterTagLookupTableModal: ProductTagMasterTagLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productTag: CreateOrEditProductTagDto = new CreateOrEditProductTagDto();

    productName = '';
    masterTagCategoryName = '';
    masterTagName = '';

    constructor(
        injector: Injector,
        private _productTagsServiceProxy: ProductTagsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productTagId?: number): void {
        if (!productTagId) {
            this.productTag = new CreateOrEditProductTagDto();
            this.productTag.id = productTagId;
            this.productName = '';
            this.masterTagCategoryName = '';
            this.masterTagName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productTagsServiceProxy.getProductTagForEdit(productTagId).subscribe((result) => {
                this.productTag = result.productTag;

                this.productName = result.productName;
                this.masterTagCategoryName = result.masterTagCategoryName;
                this.masterTagName = result.masterTagName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._productTagsServiceProxy
            .createOrEdit(this.productTag)
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
        this.productTagProductLookupTableModal.id = this.productTag.productId;
        this.productTagProductLookupTableModal.displayName = this.productName;
        this.productTagProductLookupTableModal.show();
    }
    openSelectMasterTagCategoryModal() {
        this.productTagMasterTagCategoryLookupTableModal.id = this.productTag.masterTagCategoryId;
        this.productTagMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.productTagMasterTagCategoryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.productTagMasterTagLookupTableModal.id = this.productTag.masterTagId;
        this.productTagMasterTagLookupTableModal.displayName = this.masterTagName;
        this.productTagMasterTagLookupTableModal.show();
    }

    setProductIdNull() {
        this.productTag.productId = null;
        this.productName = '';
    }
    setMasterTagCategoryIdNull() {
        this.productTag.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }
    setMasterTagIdNull() {
        this.productTag.masterTagId = null;
        this.masterTagName = '';
    }

    getNewProductId() {
        this.productTag.productId = this.productTagProductLookupTableModal.id;
        this.productName = this.productTagProductLookupTableModal.displayName;
    }
    getNewMasterTagCategoryId() {
        this.productTag.masterTagCategoryId = this.productTagMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.productTagMasterTagCategoryLookupTableModal.displayName;
    }
    getNewMasterTagId() {
        this.productTag.masterTagId = this.productTagMasterTagLookupTableModal.id;
        this.masterTagName = this.productTagMasterTagLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
