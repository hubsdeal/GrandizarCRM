import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetOrderProductInfoForViewDto, OrderProductInfoDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOrderProductInfoModal',
    templateUrl: './view-orderProductInfo-modal.component.html',
})
export class ViewOrderProductInfoModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOrderProductInfoForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetOrderProductInfoForViewDto();
        this.item.orderProductInfo = new OrderProductInfoDto();
    }

    show(item: GetOrderProductInfoForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
