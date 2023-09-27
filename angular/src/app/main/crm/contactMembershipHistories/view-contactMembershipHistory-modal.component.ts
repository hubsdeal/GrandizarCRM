import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetContactMembershipHistoryForViewDto, ContactMembershipHistoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewContactMembershipHistoryModal',
    templateUrl: './view-contactMembershipHistory-modal.component.html'
})
export class ViewContactMembershipHistoryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetContactMembershipHistoryForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetContactMembershipHistoryForViewDto();
        this.item.contactMembershipHistory = new ContactMembershipHistoryDto();
    }

    show(item: GetContactMembershipHistoryForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
