import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetMasterNavigationMenuForViewDto, MasterNavigationMenuDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewMasterNavigationMenuModal',
    templateUrl: './view-masterNavigationMenu-modal.component.html',
})
export class ViewMasterNavigationMenuModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetMasterNavigationMenuForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetMasterNavigationMenuForViewDto();
        this.item.masterNavigationMenu = new MasterNavigationMenuDto();
    }

    show(item: GetMasterNavigationMenuForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
