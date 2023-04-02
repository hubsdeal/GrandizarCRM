import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    OrderfulfillmentTeamsServiceProxy,
    CreateOrEditOrderfulfillmentTeamDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { OrderfulfillmentTeamOrderLookupTableModalComponent } from './orderfulfillmentTeam-order-lookup-table-modal.component';
import { OrderfulfillmentTeamEmployeeLookupTableModalComponent } from './orderfulfillmentTeam-employee-lookup-table-modal.component';
import { OrderfulfillmentTeamContactLookupTableModalComponent } from './orderfulfillmentTeam-contact-lookup-table-modal.component';
import { OrderfulfillmentTeamUserLookupTableModalComponent } from './orderfulfillmentTeam-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditOrderfulfillmentTeamModal',
    templateUrl: './create-or-edit-orderfulfillmentTeam-modal.component.html',
})
export class CreateOrEditOrderfulfillmentTeamModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderfulfillmentTeamOrderLookupTableModal', { static: true })
    orderfulfillmentTeamOrderLookupTableModal: OrderfulfillmentTeamOrderLookupTableModalComponent;
    @ViewChild('orderfulfillmentTeamEmployeeLookupTableModal', { static: true })
    orderfulfillmentTeamEmployeeLookupTableModal: OrderfulfillmentTeamEmployeeLookupTableModalComponent;
    @ViewChild('orderfulfillmentTeamContactLookupTableModal', { static: true })
    orderfulfillmentTeamContactLookupTableModal: OrderfulfillmentTeamContactLookupTableModalComponent;
    @ViewChild('orderfulfillmentTeamUserLookupTableModal', { static: true })
    orderfulfillmentTeamUserLookupTableModal: OrderfulfillmentTeamUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderfulfillmentTeam: CreateOrEditOrderfulfillmentTeamDto = new CreateOrEditOrderfulfillmentTeamDto();

    orderFullName = '';
    employeeName = '';
    contactFullName = '';
    userName = '';

    constructor(
        injector: Injector,
        private _orderfulfillmentTeamsServiceProxy: OrderfulfillmentTeamsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(orderfulfillmentTeamId?: number): void {
        if (!orderfulfillmentTeamId) {
            this.orderfulfillmentTeam = new CreateOrEditOrderfulfillmentTeamDto();
            this.orderfulfillmentTeam.id = orderfulfillmentTeamId;
            this.orderFullName = '';
            this.employeeName = '';
            this.contactFullName = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._orderfulfillmentTeamsServiceProxy
                .getOrderfulfillmentTeamForEdit(orderfulfillmentTeamId)
                .subscribe((result) => {
                    this.orderfulfillmentTeam = result.orderfulfillmentTeam;

                    this.orderFullName = result.orderFullName;
                    this.employeeName = result.employeeName;
                    this.contactFullName = result.contactFullName;
                    this.userName = result.userName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._orderfulfillmentTeamsServiceProxy
            .createOrEdit(this.orderfulfillmentTeam)
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
        this.orderfulfillmentTeamOrderLookupTableModal.id = this.orderfulfillmentTeam.orderId;
        this.orderfulfillmentTeamOrderLookupTableModal.displayName = this.orderFullName;
        this.orderfulfillmentTeamOrderLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.orderfulfillmentTeamEmployeeLookupTableModal.id = this.orderfulfillmentTeam.employeeId;
        this.orderfulfillmentTeamEmployeeLookupTableModal.displayName = this.employeeName;
        this.orderfulfillmentTeamEmployeeLookupTableModal.show();
    }
    openSelectContactModal() {
        this.orderfulfillmentTeamContactLookupTableModal.id = this.orderfulfillmentTeam.contactId;
        this.orderfulfillmentTeamContactLookupTableModal.displayName = this.contactFullName;
        this.orderfulfillmentTeamContactLookupTableModal.show();
    }
    openSelectUserModal() {
        this.orderfulfillmentTeamUserLookupTableModal.id = this.orderfulfillmentTeam.userId;
        this.orderfulfillmentTeamUserLookupTableModal.displayName = this.userName;
        this.orderfulfillmentTeamUserLookupTableModal.show();
    }

    setOrderIdNull() {
        this.orderfulfillmentTeam.orderId = null;
        this.orderFullName = '';
    }
    setEmployeeIdNull() {
        this.orderfulfillmentTeam.employeeId = null;
        this.employeeName = '';
    }
    setContactIdNull() {
        this.orderfulfillmentTeam.contactId = null;
        this.contactFullName = '';
    }
    setUserIdNull() {
        this.orderfulfillmentTeam.userId = null;
        this.userName = '';
    }

    getNewOrderId() {
        this.orderfulfillmentTeam.orderId = this.orderfulfillmentTeamOrderLookupTableModal.id;
        this.orderFullName = this.orderfulfillmentTeamOrderLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.orderfulfillmentTeam.employeeId = this.orderfulfillmentTeamEmployeeLookupTableModal.id;
        this.employeeName = this.orderfulfillmentTeamEmployeeLookupTableModal.displayName;
    }
    getNewContactId() {
        this.orderfulfillmentTeam.contactId = this.orderfulfillmentTeamContactLookupTableModal.id;
        this.contactFullName = this.orderfulfillmentTeamContactLookupTableModal.displayName;
    }
    getNewUserId() {
        this.orderfulfillmentTeam.userId = this.orderfulfillmentTeamUserLookupTableModal.id;
        this.userName = this.orderfulfillmentTeamUserLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
