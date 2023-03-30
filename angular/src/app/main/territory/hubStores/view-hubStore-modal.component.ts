import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetHubStoreForViewDto, HubStoreDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHubStoreModal',
    templateUrl: './view-hubStore-modal.component.html',
})
export class ViewHubStoreModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHubStoreForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetHubStoreForViewDto();
        this.item.hubStore = new HubStoreDto();
    }

    show(item: GetHubStoreForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
