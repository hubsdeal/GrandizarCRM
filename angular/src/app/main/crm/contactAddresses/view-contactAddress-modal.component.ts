import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetContactAddressForViewDto, ContactAddressDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewContactAddressModal',
    templateUrl: './view-contactAddress-modal.component.html'
})
export class ViewContactAddressModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetContactAddressForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetContactAddressForViewDto();
        this.item.contactAddress = new ContactAddressDto();
    }

    show(item: GetContactAddressForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
