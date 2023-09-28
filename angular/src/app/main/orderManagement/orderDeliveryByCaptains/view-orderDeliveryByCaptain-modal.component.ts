import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetOrderDeliveryByCaptainForViewDto, OrderDeliveryByCaptainDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOrderDeliveryByCaptainModal',
    templateUrl: './view-orderDeliveryByCaptain-modal.component.html'
})
export class ViewOrderDeliveryByCaptainModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOrderDeliveryByCaptainForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetOrderDeliveryByCaptainForViewDto();
        this.item.orderDeliveryByCaptain = new OrderDeliveryByCaptainDto();
    }

    show(item: GetOrderDeliveryByCaptainForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
