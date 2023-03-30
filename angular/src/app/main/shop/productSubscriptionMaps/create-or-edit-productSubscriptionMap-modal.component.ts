import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductSubscriptionMapsServiceProxy,
    CreateOrEditProductSubscriptionMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductSubscriptionMapProductLookupTableModalComponent } from './productSubscriptionMap-product-lookup-table-modal.component';
import { ProductSubscriptionMapSubscriptionTypeLookupTableModalComponent } from './productSubscriptionMap-subscriptionType-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductSubscriptionMapModal',
    templateUrl: './create-or-edit-productSubscriptionMap-modal.component.html',
})
export class CreateOrEditProductSubscriptionMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productSubscriptionMapProductLookupTableModal', { static: true })
    productSubscriptionMapProductLookupTableModal: ProductSubscriptionMapProductLookupTableModalComponent;
    @ViewChild('productSubscriptionMapSubscriptionTypeLookupTableModal', { static: true })
    productSubscriptionMapSubscriptionTypeLookupTableModal: ProductSubscriptionMapSubscriptionTypeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productSubscriptionMap: CreateOrEditProductSubscriptionMapDto = new CreateOrEditProductSubscriptionMapDto();

    productName = '';
    subscriptionTypeName = '';

    constructor(
        injector: Injector,
        private _productSubscriptionMapsServiceProxy: ProductSubscriptionMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productSubscriptionMapId?: number): void {
        if (!productSubscriptionMapId) {
            this.productSubscriptionMap = new CreateOrEditProductSubscriptionMapDto();
            this.productSubscriptionMap.id = productSubscriptionMapId;
            this.productName = '';
            this.subscriptionTypeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productSubscriptionMapsServiceProxy
                .getProductSubscriptionMapForEdit(productSubscriptionMapId)
                .subscribe((result) => {
                    this.productSubscriptionMap = result.productSubscriptionMap;

                    this.productName = result.productName;
                    this.subscriptionTypeName = result.subscriptionTypeName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productSubscriptionMapsServiceProxy
            .createOrEdit(this.productSubscriptionMap)
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
        this.productSubscriptionMapProductLookupTableModal.id = this.productSubscriptionMap.productId;
        this.productSubscriptionMapProductLookupTableModal.displayName = this.productName;
        this.productSubscriptionMapProductLookupTableModal.show();
    }
    openSelectSubscriptionTypeModal() {
        this.productSubscriptionMapSubscriptionTypeLookupTableModal.id = this.productSubscriptionMap.subscriptionTypeId;
        this.productSubscriptionMapSubscriptionTypeLookupTableModal.displayName = this.subscriptionTypeName;
        this.productSubscriptionMapSubscriptionTypeLookupTableModal.show();
    }

    setProductIdNull() {
        this.productSubscriptionMap.productId = null;
        this.productName = '';
    }
    setSubscriptionTypeIdNull() {
        this.productSubscriptionMap.subscriptionTypeId = null;
        this.subscriptionTypeName = '';
    }

    getNewProductId() {
        this.productSubscriptionMap.productId = this.productSubscriptionMapProductLookupTableModal.id;
        this.productName = this.productSubscriptionMapProductLookupTableModal.displayName;
    }
    getNewSubscriptionTypeId() {
        this.productSubscriptionMap.subscriptionTypeId = this.productSubscriptionMapSubscriptionTypeLookupTableModal.id;
        this.subscriptionTypeName = this.productSubscriptionMapSubscriptionTypeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
