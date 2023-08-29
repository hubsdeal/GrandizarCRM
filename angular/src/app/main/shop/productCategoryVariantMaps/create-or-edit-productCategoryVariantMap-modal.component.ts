import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductCategoryVariantMapsServiceProxy,
    CreateOrEditProductCategoryVariantMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductCategoryVariantMapProductCategoryLookupTableModalComponent } from './productCategoryVariantMap-productCategory-lookup-table-modal.component';
import { ProductCategoryVariantMapProductVariantCategoryLookupTableModalComponent } from './productCategoryVariantMap-productVariantCategory-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductCategoryVariantMapModal',
    templateUrl: './create-or-edit-productCategoryVariantMap-modal.component.html',
})
export class CreateOrEditProductCategoryVariantMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productCategoryVariantMapProductCategoryLookupTableModal', { static: true })
    productCategoryVariantMapProductCategoryLookupTableModal: ProductCategoryVariantMapProductCategoryLookupTableModalComponent;
    @ViewChild('productCategoryVariantMapProductVariantCategoryLookupTableModal', { static: true })
    productCategoryVariantMapProductVariantCategoryLookupTableModal: ProductCategoryVariantMapProductVariantCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productCategoryVariantMap: CreateOrEditProductCategoryVariantMapDto =
        new CreateOrEditProductCategoryVariantMapDto();

    productCategoryName = '';
    productVariantCategoryName = '';
    productCategoryIdFilter:number;
    constructor(
        injector: Injector,
        private _productCategoryVariantMapsServiceProxy: ProductCategoryVariantMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productCategoryVariantMapId?: number): void {
        if (!productCategoryVariantMapId) {
            this.productCategoryVariantMap = new CreateOrEditProductCategoryVariantMapDto();
            this.productCategoryVariantMap.id = productCategoryVariantMapId;
            this.productCategoryName = '';
            this.productVariantCategoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productCategoryVariantMapsServiceProxy
                .getProductCategoryVariantMapForEdit(productCategoryVariantMapId)
                .subscribe((result) => {
                    this.productCategoryVariantMap = result.productCategoryVariantMap;

                    this.productCategoryName = result.productCategoryName;
                    this.productVariantCategoryName = result.productVariantCategoryName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

            if (this.productCategoryIdFilter) {
                this.productCategoryVariantMap.productCategoryId = this.productCategoryIdFilter;
            }
    
            this._productCategoryVariantMapsServiceProxy.createOrEdit(this.productCategoryVariantMap)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modalSave.emit(null);
                });
    }

    openSelectProductCategoryModal() {
        this.productCategoryVariantMapProductCategoryLookupTableModal.id =
            this.productCategoryVariantMap.productCategoryId;
        this.productCategoryVariantMapProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.productCategoryVariantMapProductCategoryLookupTableModal.show();
    }
    openSelectProductVariantCategoryModal() {
        this.productCategoryVariantMapProductVariantCategoryLookupTableModal.id =
            this.productCategoryVariantMap.productVariantCategoryId;
        this.productCategoryVariantMapProductVariantCategoryLookupTableModal.displayName =
            this.productVariantCategoryName;
        this.productCategoryVariantMapProductVariantCategoryLookupTableModal.show();
    }

    setProductCategoryIdNull() {
        this.productCategoryVariantMap.productCategoryId = null;
        this.productCategoryName = '';
    }
    setProductVariantCategoryIdNull() {
        this.productCategoryVariantMap.productVariantCategoryId = null;
        this.productVariantCategoryName = '';
    }

    getNewProductCategoryId() {
        this.productCategoryVariantMap.productCategoryId =
            this.productCategoryVariantMapProductCategoryLookupTableModal.id;
        this.productCategoryName = this.productCategoryVariantMapProductCategoryLookupTableModal.displayName;
    }
    getNewProductVariantCategoryId() {
        this.productCategoryVariantMap.productVariantCategoryId =
            this.productCategoryVariantMapProductVariantCategoryLookupTableModal.id;
        this.productVariantCategoryName =
            this.productCategoryVariantMapProductVariantCategoryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
