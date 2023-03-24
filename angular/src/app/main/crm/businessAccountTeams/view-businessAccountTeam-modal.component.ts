import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetBusinessAccountTeamForViewDto, BusinessAccountTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBusinessAccountTeamModal',
    templateUrl: './view-businessAccountTeam-modal.component.html',
})
export class ViewBusinessAccountTeamModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBusinessAccountTeamForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetBusinessAccountTeamForViewDto();
        this.item.businessAccountTeam = new BusinessAccountTeamDto();
    }

    show(item: GetBusinessAccountTeamForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
