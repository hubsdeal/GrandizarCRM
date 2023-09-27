import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetConnectMessageForViewDto, ConnectMessageDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewConnectMessageModal',
    templateUrl: './view-connectMessage-modal.component.html'
})
export class ViewConnectMessageModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetConnectMessageForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetConnectMessageForViewDto();
        this.item.connectMessage = new ConnectMessageDto();
    }

    show(item: GetConnectMessageForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
