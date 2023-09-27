import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetOrderDeliveryChangeCaptainForViewDto, OrderDeliveryChangeCaptainDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOrderDeliveryChangeCaptainModal',
    templateUrl: './view-orderDeliveryChangeCaptain-modal.component.html'
})
export class ViewOrderDeliveryChangeCaptainModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOrderDeliveryChangeCaptainForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetOrderDeliveryChangeCaptainForViewDto();
        this.item.orderDeliveryChangeCaptain = new OrderDeliveryChangeCaptainDto();
    }

    show(item: GetOrderDeliveryChangeCaptainForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
