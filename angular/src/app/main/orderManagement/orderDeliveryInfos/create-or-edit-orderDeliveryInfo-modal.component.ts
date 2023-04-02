import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    OrderDeliveryInfosServiceProxy,
    CreateOrEditOrderDeliveryInfoDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { OrderDeliveryInfoEmployeeLookupTableModalComponent } from './orderDeliveryInfo-employee-lookup-table-modal.component';
import { OrderDeliveryInfoOrderLookupTableModalComponent } from './orderDeliveryInfo-order-lookup-table-modal.component';

@Component({
    selector: 'createOrEditOrderDeliveryInfoModal',
    templateUrl: './create-or-edit-orderDeliveryInfo-modal.component.html',
})
export class CreateOrEditOrderDeliveryInfoModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderDeliveryInfoEmployeeLookupTableModal', { static: true })
    orderDeliveryInfoEmployeeLookupTableModal: OrderDeliveryInfoEmployeeLookupTableModalComponent;
    @ViewChild('orderDeliveryInfoOrderLookupTableModal', { static: true })
    orderDeliveryInfoOrderLookupTableModal: OrderDeliveryInfoOrderLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderDeliveryInfo: CreateOrEditOrderDeliveryInfoDto = new CreateOrEditOrderDeliveryInfoDto();

    employeeName = '';
    orderInvoiceNumber = '';

    constructor(
        injector: Injector,
        private _orderDeliveryInfosServiceProxy: OrderDeliveryInfosServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(orderDeliveryInfoId?: number): void {
        if (!orderDeliveryInfoId) {
            this.orderDeliveryInfo = new CreateOrEditOrderDeliveryInfoDto();
            this.orderDeliveryInfo.id = orderDeliveryInfoId;
            this.orderDeliveryInfo.dispatchDate = this._dateTimeService.getStartOfDay();
            this.orderDeliveryInfo.deliverToCustomerDate = this._dateTimeService.getStartOfDay();
            this.orderDeliveryInfo.cateringDate = this._dateTimeService.getStartOfDay();
            this.orderDeliveryInfo.deliveryDate = this._dateTimeService.getStartOfDay();
            this.orderDeliveryInfo.dineInDate = this._dateTimeService.getStartOfDay();
            this.orderDeliveryInfo.pickupDate = this._dateTimeService.getStartOfDay();
            this.employeeName = '';
            this.orderInvoiceNumber = '';

            this.active = true;
            this.modal.show();
        } else {
            this._orderDeliveryInfosServiceProxy
                .getOrderDeliveryInfoForEdit(orderDeliveryInfoId)
                .subscribe((result) => {
                    this.orderDeliveryInfo = result.orderDeliveryInfo;

                    this.employeeName = result.employeeName;
                    this.orderInvoiceNumber = result.orderInvoiceNumber;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._orderDeliveryInfosServiceProxy
            .createOrEdit(this.orderDeliveryInfo)
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

    openSelectEmployeeModal() {
        this.orderDeliveryInfoEmployeeLookupTableModal.id = this.orderDeliveryInfo.employeeId;
        this.orderDeliveryInfoEmployeeLookupTableModal.displayName = this.employeeName;
        this.orderDeliveryInfoEmployeeLookupTableModal.show();
    }
    openSelectOrderModal() {
        this.orderDeliveryInfoOrderLookupTableModal.id = this.orderDeliveryInfo.orderId;
        this.orderDeliveryInfoOrderLookupTableModal.displayName = this.orderInvoiceNumber;
        this.orderDeliveryInfoOrderLookupTableModal.show();
    }

    setEmployeeIdNull() {
        this.orderDeliveryInfo.employeeId = null;
        this.employeeName = '';
    }
    setOrderIdNull() {
        this.orderDeliveryInfo.orderId = null;
        this.orderInvoiceNumber = '';
    }

    getNewEmployeeId() {
        this.orderDeliveryInfo.employeeId = this.orderDeliveryInfoEmployeeLookupTableModal.id;
        this.employeeName = this.orderDeliveryInfoEmployeeLookupTableModal.displayName;
    }
    getNewOrderId() {
        this.orderDeliveryInfo.orderId = this.orderDeliveryInfoOrderLookupTableModal.id;
        this.orderInvoiceNumber = this.orderDeliveryInfoOrderLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
