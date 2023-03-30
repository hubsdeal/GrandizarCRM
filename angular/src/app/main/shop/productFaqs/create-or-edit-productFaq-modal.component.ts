import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductFaqsServiceProxy, CreateOrEditProductFaqDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductFaqProductLookupTableModalComponent } from './productFaq-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductFaqModal',
    templateUrl: './create-or-edit-productFaq-modal.component.html',
})
export class CreateOrEditProductFaqModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productFaqProductLookupTableModal', { static: true })
    productFaqProductLookupTableModal: ProductFaqProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productFaq: CreateOrEditProductFaqDto = new CreateOrEditProductFaqDto();

    productName = '';

    constructor(
        injector: Injector,
        private _productFaqsServiceProxy: ProductFaqsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productFaqId?: number): void {
        if (!productFaqId) {
            this.productFaq = new CreateOrEditProductFaqDto();
            this.productFaq.id = productFaqId;
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productFaqsServiceProxy.getProductFaqForEdit(productFaqId).subscribe((result) => {
                this.productFaq = result.productFaq;

                this.productName = result.productName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._productFaqsServiceProxy
            .createOrEdit(this.productFaq)
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
        this.productFaqProductLookupTableModal.id = this.productFaq.productId;
        this.productFaqProductLookupTableModal.displayName = this.productName;
        this.productFaqProductLookupTableModal.show();
    }

    setProductIdNull() {
        this.productFaq.productId = null;
        this.productName = '';
    }

    getNewProductId() {
        this.productFaq.productId = this.productFaqProductLookupTableModal.id;
        this.productName = this.productFaqProductLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
