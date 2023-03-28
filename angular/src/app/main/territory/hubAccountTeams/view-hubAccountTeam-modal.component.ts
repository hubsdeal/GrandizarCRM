import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetHubAccountTeamForViewDto, HubAccountTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHubAccountTeamModal',
    templateUrl: './view-hubAccountTeam-modal.component.html',
})
export class ViewHubAccountTeamModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHubAccountTeamForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetHubAccountTeamForViewDto();
        this.item.hubAccountTeam = new HubAccountTeamDto();
    }

    show(item: GetHubAccountTeamForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
