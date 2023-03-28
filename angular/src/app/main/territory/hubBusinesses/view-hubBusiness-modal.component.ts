import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetHubBusinessForViewDto, HubBusinessDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHubBusinessModal',
    templateUrl: './view-hubBusiness-modal.component.html',
})
export class ViewHubBusinessModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHubBusinessForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetHubBusinessForViewDto();
        this.item.hubBusiness = new HubBusinessDto();
    }

    show(item: GetHubBusinessForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
