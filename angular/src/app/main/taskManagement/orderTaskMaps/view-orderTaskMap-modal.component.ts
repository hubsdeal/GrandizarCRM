import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetOrderTaskMapForViewDto, OrderTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOrderTaskMapModal',
    templateUrl: './view-orderTaskMap-modal.component.html'
})
export class ViewOrderTaskMapModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOrderTaskMapForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetOrderTaskMapForViewDto();
        this.item.orderTaskMap = new OrderTaskMapDto();
    }

    show(item: GetOrderTaskMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
