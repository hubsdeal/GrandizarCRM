import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetDeliveryTypeForViewDto, DeliveryTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewDeliveryTypeModal',
    templateUrl: './view-deliveryType-modal.component.html'
})
export class ViewDeliveryTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetDeliveryTypeForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetDeliveryTypeForViewDto();
        this.item.deliveryType = new DeliveryTypeDto();
    }

    show(item: GetDeliveryTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
