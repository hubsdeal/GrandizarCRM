import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { OrderStatusesServiceProxy, CreateOrEditOrderStatusDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { OrderStatusRoleLookupTableModalComponent } from './orderStatus-role-lookup-table-modal.component';

@Component({
    selector: 'createOrEditOrderStatusModal',
    templateUrl: './create-or-edit-orderStatus-modal.component.html',
})
export class CreateOrEditOrderStatusModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderStatusRoleLookupTableModal', { static: true })
    orderStatusRoleLookupTableModal: OrderStatusRoleLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderStatus: CreateOrEditOrderStatusDto = new CreateOrEditOrderStatusDto();

    roleName = '';

    constructor(
        injector: Injector,
        private _orderStatusesServiceProxy: OrderStatusesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(orderStatusId?: number): void {
        if (!orderStatusId) {
            this.orderStatus = new CreateOrEditOrderStatusDto();
            this.orderStatus.id = orderStatusId;
            this.roleName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._orderStatusesServiceProxy.getOrderStatusForEdit(orderStatusId).subscribe((result) => {
                this.orderStatus = result.orderStatus;

                this.roleName = result.roleName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._orderStatusesServiceProxy
            .createOrEdit(this.orderStatus)
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

    openSelectRoleModal() {
        this.orderStatusRoleLookupTableModal.id = this.orderStatus.roleId;
        this.orderStatusRoleLookupTableModal.displayName = this.roleName;
        this.orderStatusRoleLookupTableModal.show();
    }

    setRoleIdNull() {
        this.orderStatus.roleId = null;
        this.roleName = '';
    }

    getNewRoleId() {
        this.orderStatus.roleId = this.orderStatusRoleLookupTableModal.id;
        this.roleName = this.orderStatusRoleLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
