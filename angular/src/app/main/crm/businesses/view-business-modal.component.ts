import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetBusinessForViewDto, BusinessDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBusinessModal',
    templateUrl: './view-business-modal.component.html',
})
export class ViewBusinessModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBusinessForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetBusinessForViewDto();
        this.item.business = new BusinessDto();
    }

    show(item: GetBusinessForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
