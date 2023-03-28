import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetHubNavigationMenuForViewDto, HubNavigationMenuDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHubNavigationMenuModal',
    templateUrl: './view-hubNavigationMenu-modal.component.html',
})
export class ViewHubNavigationMenuModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHubNavigationMenuForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetHubNavigationMenuForViewDto();
        this.item.hubNavigationMenu = new HubNavigationMenuDto();
    }

    show(item: GetHubNavigationMenuForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
