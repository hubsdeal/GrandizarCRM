import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    OrderFulfillmentStatusesServiceProxy,
    CreateOrEditOrderFulfillmentStatusDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { OrderFulfillmentStatusOrderStatusLookupTableModalComponent } from './orderFulfillmentStatus-orderStatus-lookup-table-modal.component';
import { OrderFulfillmentStatusOrderLookupTableModalComponent } from './orderFulfillmentStatus-order-lookup-table-modal.component';
import { OrderFulfillmentStatusEmployeeLookupTableModalComponent } from './orderFulfillmentStatus-employee-lookup-table-modal.component';

@Component({
    selector: 'createOrEditOrderFulfillmentStatusModal',
    templateUrl: './create-or-edit-orderFulfillmentStatus-modal.component.html',
})
export class CreateOrEditOrderFulfillmentStatusModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderFulfillmentStatusOrderStatusLookupTableModal', { static: true })
    orderFulfillmentStatusOrderStatusLookupTableModal: OrderFulfillmentStatusOrderStatusLookupTableModalComponent;
    @ViewChild('orderFulfillmentStatusOrderLookupTableModal', { static: true })
    orderFulfillmentStatusOrderLookupTableModal: OrderFulfillmentStatusOrderLookupTableModalComponent;
    @ViewChild('orderFulfillmentStatusEmployeeLookupTableModal', { static: true })
    orderFulfillmentStatusEmployeeLookupTableModal: OrderFulfillmentStatusEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderFulfillmentStatus: CreateOrEditOrderFulfillmentStatusDto = new CreateOrEditOrderFulfillmentStatusDto();

    orderStatusName = '';
    orderInvoiceNumber = '';
    employeeName = '';

    constructor(
        injector: Injector,
        private _orderFulfillmentStatusesServiceProxy: OrderFulfillmentStatusesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(orderFulfillmentStatusId?: number): void {
        if (!orderFulfillmentStatusId) {
            this.orderFulfillmentStatus = new CreateOrEditOrderFulfillmentStatusDto();
            this.orderFulfillmentStatus.id = orderFulfillmentStatusId;
            this.orderFulfillmentStatus.estimatedTime = this._dateTimeService.getStartOfDay();
            this.orderFulfillmentStatus.actualTime = this._dateTimeService.getStartOfDay();
            this.orderStatusName = '';
            this.orderInvoiceNumber = '';
            this.employeeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._orderFulfillmentStatusesServiceProxy
                .getOrderFulfillmentStatusForEdit(orderFulfillmentStatusId)
                .subscribe((result) => {
                    this.orderFulfillmentStatus = result.orderFulfillmentStatus;

                    this.orderStatusName = result.orderStatusName;
                    this.orderInvoiceNumber = result.orderInvoiceNumber;
                    this.employeeName = result.employeeName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._orderFulfillmentStatusesServiceProxy
            .createOrEdit(this.orderFulfillmentStatus)
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

    openSelectOrderStatusModal() {
        this.orderFulfillmentStatusOrderStatusLookupTableModal.id = this.orderFulfillmentStatus.orderStatusId;
        this.orderFulfillmentStatusOrderStatusLookupTableModal.displayName = this.orderStatusName;
        this.orderFulfillmentStatusOrderStatusLookupTableModal.show();
    }
    openSelectOrderModal() {
        this.orderFulfillmentStatusOrderLookupTableModal.id = this.orderFulfillmentStatus.orderId;
        this.orderFulfillmentStatusOrderLookupTableModal.displayName = this.orderInvoiceNumber;
        this.orderFulfillmentStatusOrderLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.orderFulfillmentStatusEmployeeLookupTableModal.id = this.orderFulfillmentStatus.employeeId;
        this.orderFulfillmentStatusEmployeeLookupTableModal.displayName = this.employeeName;
        this.orderFulfillmentStatusEmployeeLookupTableModal.show();
    }

    setOrderStatusIdNull() {
        this.orderFulfillmentStatus.orderStatusId = null;
        this.orderStatusName = '';
    }
    setOrderIdNull() {
        this.orderFulfillmentStatus.orderId = null;
        this.orderInvoiceNumber = '';
    }
    setEmployeeIdNull() {
        this.orderFulfillmentStatus.employeeId = null;
        this.employeeName = '';
    }

    getNewOrderStatusId() {
        this.orderFulfillmentStatus.orderStatusId = this.orderFulfillmentStatusOrderStatusLookupTableModal.id;
        this.orderStatusName = this.orderFulfillmentStatusOrderStatusLookupTableModal.displayName;
    }
    getNewOrderId() {
        this.orderFulfillmentStatus.orderId = this.orderFulfillmentStatusOrderLookupTableModal.id;
        this.orderInvoiceNumber = this.orderFulfillmentStatusOrderLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.orderFulfillmentStatus.employeeId = this.orderFulfillmentStatusEmployeeLookupTableModal.id;
        this.employeeName = this.orderFulfillmentStatusEmployeeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
