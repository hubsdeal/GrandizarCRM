import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetOrderDeliveryInfoForViewDto, OrderDeliveryInfoDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOrderDeliveryInfoModal',
    templateUrl: './view-orderDeliveryInfo-modal.component.html',
})
export class ViewOrderDeliveryInfoModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOrderDeliveryInfoForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetOrderDeliveryInfoForViewDto();
        this.item.orderDeliveryInfo = new OrderDeliveryInfoDto();
    }

    show(item: GetOrderDeliveryInfoForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
