import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductCategoryMapsServiceProxy,
    CreateOrEditProductCategoryMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductCategoryMapProductLookupTableModalComponent } from './productCategoryMap-product-lookup-table-modal.component';
import { ProductCategoryMapProductCategoryLookupTableModalComponent } from './productCategoryMap-productCategory-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductCategoryMapModal',
    templateUrl: './create-or-edit-productCategoryMap-modal.component.html',
})
export class CreateOrEditProductCategoryMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productCategoryMapProductLookupTableModal', { static: true })
    productCategoryMapProductLookupTableModal: ProductCategoryMapProductLookupTableModalComponent;
    @ViewChild('productCategoryMapProductCategoryLookupTableModal', { static: true })
    productCategoryMapProductCategoryLookupTableModal: ProductCategoryMapProductCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productCategoryMap: CreateOrEditProductCategoryMapDto = new CreateOrEditProductCategoryMapDto();

    productName = '';
    productCategoryName = '';

    constructor(
        injector: Injector,
        private _productCategoryMapsServiceProxy: ProductCategoryMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productCategoryMapId?: number): void {
        if (!productCategoryMapId) {
            this.productCategoryMap = new CreateOrEditProductCategoryMapDto();
            this.productCategoryMap.id = productCategoryMapId;
            this.productName = '';
            this.productCategoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productCategoryMapsServiceProxy
                .getProductCategoryMapForEdit(productCategoryMapId)
                .subscribe((result) => {
                    this.productCategoryMap = result.productCategoryMap;

                    this.productName = result.productName;
                    this.productCategoryName = result.productCategoryName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productCategoryMapsServiceProxy
            .createOrEdit(this.productCategoryMap)
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
        this.productCategoryMapProductLookupTableModal.id = this.productCategoryMap.productId;
        this.productCategoryMapProductLookupTableModal.displayName = this.productName;
        this.productCategoryMapProductLookupTableModal.show();
    }
    openSelectProductCategoryModal() {
        this.productCategoryMapProductCategoryLookupTableModal.id = this.productCategoryMap.productCategoryId;
        this.productCategoryMapProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.productCategoryMapProductCategoryLookupTableModal.show();
    }

    setProductIdNull() {
        this.productCategoryMap.productId = null;
        this.productName = '';
    }
    setProductCategoryIdNull() {
        this.productCategoryMap.productCategoryId = null;
        this.productCategoryName = '';
    }

    getNewProductId() {
        this.productCategoryMap.productId = this.productCategoryMapProductLookupTableModal.id;
        this.productName = this.productCategoryMapProductLookupTableModal.displayName;
    }
    getNewProductCategoryId() {
        this.productCategoryMap.productCategoryId = this.productCategoryMapProductCategoryLookupTableModal.id;
        this.productCategoryName = this.productCategoryMapProductCategoryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
