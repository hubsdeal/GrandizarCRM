import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetContactTaskMapForViewDto, ContactTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewContactTaskMapModal',
    templateUrl: './view-contactTaskMap-modal.component.html',
})
export class ViewContactTaskMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetContactTaskMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetContactTaskMapForViewDto();
        this.item.contactTaskMap = new ContactTaskMapDto();
    }

    show(item: GetContactTaskMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
