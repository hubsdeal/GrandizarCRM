import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetHubTypeForViewDto, HubTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHubTypeModal',
    templateUrl: './view-hubType-modal.component.html',
})
export class ViewHubTypeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHubTypeForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetHubTypeForViewDto();
        this.item.hubType = new HubTypeDto();
    }

    show(item: GetHubTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
