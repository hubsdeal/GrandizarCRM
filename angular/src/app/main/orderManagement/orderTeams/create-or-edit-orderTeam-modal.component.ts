import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { OrderTeamsServiceProxy, CreateOrEditOrderTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { OrderTeamOrderLookupTableModalComponent } from './orderTeam-order-lookup-table-modal.component';
import { OrderTeamEmployeeLookupTableModalComponent } from './orderTeam-employee-lookup-table-modal.component';

@Component({
    selector: 'createOrEditOrderTeamModal',
    templateUrl: './create-or-edit-orderTeam-modal.component.html',
})
export class CreateOrEditOrderTeamModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderTeamOrderLookupTableModal', { static: true })
    orderTeamOrderLookupTableModal: OrderTeamOrderLookupTableModalComponent;
    @ViewChild('orderTeamEmployeeLookupTableModal', { static: true })
    orderTeamEmployeeLookupTableModal: OrderTeamEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderTeam: CreateOrEditOrderTeamDto = new CreateOrEditOrderTeamDto();

    orderInvoiceNumber = '';
    employeeName = '';

    constructor(
        injector: Injector,
        private _orderTeamsServiceProxy: OrderTeamsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(orderTeamId?: number): void {
        if (!orderTeamId) {
            this.orderTeam = new CreateOrEditOrderTeamDto();
            this.orderTeam.id = orderTeamId;
            this.orderInvoiceNumber = '';
            this.employeeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._orderTeamsServiceProxy.getOrderTeamForEdit(orderTeamId).subscribe((result) => {
                this.orderTeam = result.orderTeam;

                this.orderInvoiceNumber = result.orderInvoiceNumber;
                this.employeeName = result.employeeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._orderTeamsServiceProxy
            .createOrEdit(this.orderTeam)
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
        this.orderTeamOrderLookupTableModal.id = this.orderTeam.orderId;
        this.orderTeamOrderLookupTableModal.displayName = this.orderInvoiceNumber;
        this.orderTeamOrderLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.orderTeamEmployeeLookupTableModal.id = this.orderTeam.employeeId;
        this.orderTeamEmployeeLookupTableModal.displayName = this.employeeName;
        this.orderTeamEmployeeLookupTableModal.show();
    }

    setOrderIdNull() {
        this.orderTeam.orderId = null;
        this.orderInvoiceNumber = '';
    }
    setEmployeeIdNull() {
        this.orderTeam.employeeId = null;
        this.employeeName = '';
    }

    getNewOrderId() {
        this.orderTeam.orderId = this.orderTeamOrderLookupTableModal.id;
        this.orderInvoiceNumber = this.orderTeamOrderLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.orderTeam.employeeId = this.orderTeamEmployeeLookupTableModal.id;
        this.employeeName = this.orderTeamEmployeeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
