import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    OrderProductInfosServiceProxy,
    CreateOrEditOrderProductInfoDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { OrderProductInfoOrderLookupTableModalComponent } from './orderProductInfo-order-lookup-table-modal.component';
import { OrderProductInfoStoreLookupTableModalComponent } from './orderProductInfo-store-lookup-table-modal.component';
import { OrderProductInfoProductLookupTableModalComponent } from './orderProductInfo-product-lookup-table-modal.component';
import { OrderProductInfoMeasurementUnitLookupTableModalComponent } from './orderProductInfo-measurementUnit-lookup-table-modal.component';

@Component({
    selector: 'createOrEditOrderProductInfoModal',
    templateUrl: './create-or-edit-orderProductInfo-modal.component.html',
})
export class CreateOrEditOrderProductInfoModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderProductInfoOrderLookupTableModal', { static: true })
    orderProductInfoOrderLookupTableModal: OrderProductInfoOrderLookupTableModalComponent;
    @ViewChild('orderProductInfoStoreLookupTableModal', { static: true })
    orderProductInfoStoreLookupTableModal: OrderProductInfoStoreLookupTableModalComponent;
    @ViewChild('orderProductInfoProductLookupTableModal', { static: true })
    orderProductInfoProductLookupTableModal: OrderProductInfoProductLookupTableModalComponent;
    @ViewChild('orderProductInfoMeasurementUnitLookupTableModal', { static: true })
    orderProductInfoMeasurementUnitLookupTableModal: OrderProductInfoMeasurementUnitLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderProductInfo: CreateOrEditOrderProductInfoDto = new CreateOrEditOrderProductInfoDto();

    orderInvoiceNumber = '';
    storeName = '';
    productName = '';
    measurementUnitName = '';

    constructor(
        injector: Injector,
        private _orderProductInfosServiceProxy: OrderProductInfosServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(orderProductInfoId?: number): void {
        if (!orderProductInfoId) {
            this.orderProductInfo = new CreateOrEditOrderProductInfoDto();
            this.orderProductInfo.id = orderProductInfoId;
            this.orderInvoiceNumber = '';
            this.storeName = '';
            this.productName = '';
            this.measurementUnitName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._orderProductInfosServiceProxy.getOrderProductInfoForEdit(orderProductInfoId).subscribe((result) => {
                this.orderProductInfo = result.orderProductInfo;

                this.orderInvoiceNumber = result.orderInvoiceNumber;
                this.storeName = result.storeName;
                this.productName = result.productName;
                this.measurementUnitName = result.measurementUnitName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._orderProductInfosServiceProxy
            .createOrEdit(this.orderProductInfo)
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

    openSelectOrderModal() {
        this.orderProductInfoOrderLookupTableModal.id = this.orderProductInfo.orderId;
        this.orderProductInfoOrderLookupTableModal.displayName = this.orderInvoiceNumber;
        this.orderProductInfoOrderLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.orderProductInfoStoreLookupTableModal.id = this.orderProductInfo.storeId;
        this.orderProductInfoStoreLookupTableModal.displayName = this.storeName;
        this.orderProductInfoStoreLookupTableModal.show();
    }
    openSelectProductModal() {
        this.orderProductInfoProductLookupTableModal.id = this.orderProductInfo.productId;
        this.orderProductInfoProductLookupTableModal.displayName = this.productName;
        this.orderProductInfoProductLookupTableModal.show();
    }
    openSelectMeasurementUnitModal() {
        this.orderProductInfoMeasurementUnitLookupTableModal.id = this.orderProductInfo.measurementUnitId;
        this.orderProductInfoMeasurementUnitLookupTableModal.displayName = this.measurementUnitName;
        this.orderProductInfoMeasurementUnitLookupTableModal.show();
    }

    setOrderIdNull() {
        this.orderProductInfo.orderId = null;
        this.orderInvoiceNumber = '';
    }
    setStoreIdNull() {
        this.orderProductInfo.storeId = null;
        this.storeName = '';
    }
    setProductIdNull() {
        this.orderProductInfo.productId = null;
        this.productName = '';
    }
    setMeasurementUnitIdNull() {
        this.orderProductInfo.measurementUnitId = null;
        this.measurementUnitName = '';
    }

    getNewOrderId() {
        this.orderProductInfo.orderId = this.orderProductInfoOrderLookupTableModal.id;
        this.orderInvoiceNumber = this.orderProductInfoOrderLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.orderProductInfo.storeId = this.orderProductInfoStoreLookupTableModal.id;
        this.storeName = this.orderProductInfoStoreLookupTableModal.displayName;
    }
    getNewProductId() {
        this.orderProductInfo.productId = this.orderProductInfoProductLookupTableModal.id;
        this.productName = this.orderProductInfoProductLookupTableModal.displayName;
    }
    getNewMeasurementUnitId() {
        this.orderProductInfo.measurementUnitId = this.orderProductInfoMeasurementUnitLookupTableModal.id;
        this.measurementUnitName = this.orderProductInfoMeasurementUnitLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
