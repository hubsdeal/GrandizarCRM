import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetOrderSalesChannelForViewDto, OrderSalesChannelDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOrderSalesChannelModal',
    templateUrl: './view-orderSalesChannel-modal.component.html',
})
export class ViewOrderSalesChannelModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOrderSalesChannelForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetOrderSalesChannelForViewDto();
        this.item.orderSalesChannel = new OrderSalesChannelDto();
    }

    show(item: GetOrderSalesChannelForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
