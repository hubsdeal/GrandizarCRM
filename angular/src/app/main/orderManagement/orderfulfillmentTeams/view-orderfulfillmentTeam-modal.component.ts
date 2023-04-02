import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetOrderfulfillmentTeamForViewDto, OrderfulfillmentTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOrderfulfillmentTeamModal',
    templateUrl: './view-orderfulfillmentTeam-modal.component.html',
})
export class ViewOrderfulfillmentTeamModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOrderfulfillmentTeamForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetOrderfulfillmentTeamForViewDto();
        this.item.orderfulfillmentTeam = new OrderfulfillmentTeamDto();
    }

    show(item: GetOrderfulfillmentTeamForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
