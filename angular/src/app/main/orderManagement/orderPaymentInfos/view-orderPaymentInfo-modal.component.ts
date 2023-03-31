import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetOrderPaymentInfoForViewDto, OrderPaymentInfoDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOrderPaymentInfoModal',
    templateUrl: './view-orderPaymentInfo-modal.component.html',
})
export class ViewOrderPaymentInfoModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOrderPaymentInfoForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetOrderPaymentInfoForViewDto();
        this.item.orderPaymentInfo = new OrderPaymentInfoDto();
    }

    show(item: GetOrderPaymentInfoForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
