import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetBusinessUserForViewDto, BusinessUserDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBusinessUserModal',
    templateUrl: './view-businessUser-modal.component.html',
})
export class ViewBusinessUserModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBusinessUserForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetBusinessUserForViewDto();
        this.item.businessUser = new BusinessUserDto();
    }

    show(item: GetBusinessUserForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
