import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreAccountTeamForViewDto, StoreAccountTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreAccountTeamModal',
    templateUrl: './view-storeAccountTeam-modal.component.html',
})
export class ViewStoreAccountTeamModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreAccountTeamForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreAccountTeamForViewDto();
        this.item.storeAccountTeam = new StoreAccountTeamDto();
    }

    show(item: GetStoreAccountTeamForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
