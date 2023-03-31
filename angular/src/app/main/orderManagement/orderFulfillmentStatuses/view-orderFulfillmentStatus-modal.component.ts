import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetOrderFulfillmentStatusForViewDto,
    OrderFulfillmentStatusDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOrderFulfillmentStatusModal',
    templateUrl: './view-orderFulfillmentStatus-modal.component.html',
})
export class ViewOrderFulfillmentStatusModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOrderFulfillmentStatusForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetOrderFulfillmentStatusForViewDto();
        this.item.orderFulfillmentStatus = new OrderFulfillmentStatusDto();
    }

    show(item: GetOrderFulfillmentStatusForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
