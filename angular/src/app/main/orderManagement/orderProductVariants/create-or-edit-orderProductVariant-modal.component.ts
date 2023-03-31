import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    OrderProductVariantsServiceProxy,
    CreateOrEditOrderProductVariantDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { OrderProductVariantProductVariantCategoryLookupTableModalComponent } from './orderProductVariant-productVariantCategory-lookup-table-modal.component';
import { OrderProductVariantProductVariantLookupTableModalComponent } from './orderProductVariant-productVariant-lookup-table-modal.component';

@Component({
    selector: 'createOrEditOrderProductVariantModal',
    templateUrl: './create-or-edit-orderProductVariant-modal.component.html',
})
export class CreateOrEditOrderProductVariantModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderProductVariantProductVariantCategoryLookupTableModal', { static: true })
    orderProductVariantProductVariantCategoryLookupTableModal: OrderProductVariantProductVariantCategoryLookupTableModalComponent;
    @ViewChild('orderProductVariantProductVariantLookupTableModal', { static: true })
    orderProductVariantProductVariantLookupTableModal: OrderProductVariantProductVariantLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderProductVariant: CreateOrEditOrderProductVariantDto = new CreateOrEditOrderProductVariantDto();

    productVariantCategoryName = '';
    productVariantName = '';

    constructor(
        injector: Injector,
        private _orderProductVariantsServiceProxy: OrderProductVariantsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(orderProductVariantId?: number): void {
        if (!orderProductVariantId) {
            this.orderProductVariant = new CreateOrEditOrderProductVariantDto();
            this.orderProductVariant.id = orderProductVariantId;
            this.productVariantCategoryName = '';
            this.productVariantName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._orderProductVariantsServiceProxy
                .getOrderProductVariantForEdit(orderProductVariantId)
                .subscribe((result) => {
                    this.orderProductVariant = result.orderProductVariant;

                    this.productVariantCategoryName = result.productVariantCategoryName;
                    this.productVariantName = result.productVariantName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._orderProductVariantsServiceProxy
            .createOrEdit(this.orderProductVariant)
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
        this.orderProductVariantProductVariantCategoryLookupTableModal.id =
            this.orderProductVariant.productVariantCategoryId;
        this.orderProductVariantProductVariantCategoryLookupTableModal.displayName = this.productVariantCategoryName;
        this.orderProductVariantProductVariantCategoryLookupTableModal.show();
    }
    openSelectProductVariantModal() {
        this.orderProductVariantProductVariantLookupTableModal.id = this.orderProductVariant.productVariantId;
        this.orderProductVariantProductVariantLookupTableModal.displayName = this.productVariantName;
        this.orderProductVariantProductVariantLookupTableModal.show();
    }

    setProductVariantCategoryIdNull() {
        this.orderProductVariant.productVariantCategoryId = null;
        this.productVariantCategoryName = '';
    }
    setProductVariantIdNull() {
        this.orderProductVariant.productVariantId = null;
        this.productVariantName = '';
    }

    getNewProductVariantCategoryId() {
        this.orderProductVariant.productVariantCategoryId =
            this.orderProductVariantProductVariantCategoryLookupTableModal.id;
        this.productVariantCategoryName = this.orderProductVariantProductVariantCategoryLookupTableModal.displayName;
    }
    getNewProductVariantId() {
        this.orderProductVariant.productVariantId = this.orderProductVariantProductVariantLookupTableModal.id;
        this.productVariantName = this.orderProductVariantProductVariantLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
