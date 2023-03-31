import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    OrderPaymentInfosServiceProxy,
    CreateOrEditOrderPaymentInfoDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { OrderPaymentInfoOrderLookupTableModalComponent } from './orderPaymentInfo-order-lookup-table-modal.component';
import { OrderPaymentInfoCurrencyLookupTableModalComponent } from './orderPaymentInfo-currency-lookup-table-modal.component';
import { OrderPaymentInfoPaymentTypeLookupTableModalComponent } from './orderPaymentInfo-paymentType-lookup-table-modal.component';

@Component({
    selector: 'createOrEditOrderPaymentInfoModal',
    templateUrl: './create-or-edit-orderPaymentInfo-modal.component.html',
})
export class CreateOrEditOrderPaymentInfoModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderPaymentInfoOrderLookupTableModal', { static: true })
    orderPaymentInfoOrderLookupTableModal: OrderPaymentInfoOrderLookupTableModalComponent;
    @ViewChild('orderPaymentInfoCurrencyLookupTableModal', { static: true })
    orderPaymentInfoCurrencyLookupTableModal: OrderPaymentInfoCurrencyLookupTableModalComponent;
    @ViewChild('orderPaymentInfoPaymentTypeLookupTableModal', { static: true })
    orderPaymentInfoPaymentTypeLookupTableModal: OrderPaymentInfoPaymentTypeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderPaymentInfo: CreateOrEditOrderPaymentInfoDto = new CreateOrEditOrderPaymentInfoDto();

    orderInvoiceNumber = '';
    currencyName = '';
    paymentTypeName = '';

    constructor(
        injector: Injector,
        private _orderPaymentInfosServiceProxy: OrderPaymentInfosServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(orderPaymentInfoId?: number): void {
        if (!orderPaymentInfoId) {
            this.orderPaymentInfo = new CreateOrEditOrderPaymentInfoDto();
            this.orderPaymentInfo.id = orderPaymentInfoId;
            this.orderPaymentInfo.paidDate = this._dateTimeService.getStartOfDay();
            this.orderInvoiceNumber = '';
            this.currencyName = '';
            this.paymentTypeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._orderPaymentInfosServiceProxy.getOrderPaymentInfoForEdit(orderPaymentInfoId).subscribe((result) => {
                this.orderPaymentInfo = result.orderPaymentInfo;

                this.orderInvoiceNumber = result.orderInvoiceNumber;
                this.currencyName = result.currencyName;
                this.paymentTypeName = result.paymentTypeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._orderPaymentInfosServiceProxy
            .createOrEdit(this.orderPaymentInfo)
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
        this.orderPaymentInfoOrderLookupTableModal.id = this.orderPaymentInfo.orderId;
        this.orderPaymentInfoOrderLookupTableModal.displayName = this.orderInvoiceNumber;
        this.orderPaymentInfoOrderLookupTableModal.show();
    }
    openSelectCurrencyModal() {
        this.orderPaymentInfoCurrencyLookupTableModal.id = this.orderPaymentInfo.currencyId;
        this.orderPaymentInfoCurrencyLookupTableModal.displayName = this.currencyName;
        this.orderPaymentInfoCurrencyLookupTableModal.show();
    }
    openSelectPaymentTypeModal() {
        this.orderPaymentInfoPaymentTypeLookupTableModal.id = this.orderPaymentInfo.paymentTypeId;
        this.orderPaymentInfoPaymentTypeLookupTableModal.displayName = this.paymentTypeName;
        this.orderPaymentInfoPaymentTypeLookupTableModal.show();
    }

    setOrderIdNull() {
        this.orderPaymentInfo.orderId = null;
        this.orderInvoiceNumber = '';
    }
    setCurrencyIdNull() {
        this.orderPaymentInfo.currencyId = null;
        this.currencyName = '';
    }
    setPaymentTypeIdNull() {
        this.orderPaymentInfo.paymentTypeId = null;
        this.paymentTypeName = '';
    }

    getNewOrderId() {
        this.orderPaymentInfo.orderId = this.orderPaymentInfoOrderLookupTableModal.id;
        this.orderInvoiceNumber = this.orderPaymentInfoOrderLookupTableModal.displayName;
    }
    getNewCurrencyId() {
        this.orderPaymentInfo.currencyId = this.orderPaymentInfoCurrencyLookupTableModal.id;
        this.currencyName = this.orderPaymentInfoCurrencyLookupTableModal.displayName;
    }
    getNewPaymentTypeId() {
        this.orderPaymentInfo.paymentTypeId = this.orderPaymentInfoPaymentTypeLookupTableModal.id;
        this.paymentTypeName = this.orderPaymentInfoPaymentTypeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
