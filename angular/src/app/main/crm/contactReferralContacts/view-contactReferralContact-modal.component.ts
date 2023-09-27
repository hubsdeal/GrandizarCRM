import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetContactReferralContactForViewDto, ContactReferralContactDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewContactReferralContactModal',
    templateUrl: './view-contactReferralContact-modal.component.html'
})
export class ViewContactReferralContactModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetContactReferralContactForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetContactReferralContactForViewDto();
        this.item.contactReferralContact = new ContactReferralContactDto();
    }

    show(item: GetContactReferralContactForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
