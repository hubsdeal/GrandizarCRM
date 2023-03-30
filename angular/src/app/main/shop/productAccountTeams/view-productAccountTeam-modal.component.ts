import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductAccountTeamForViewDto, ProductAccountTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductAccountTeamModal',
    templateUrl: './view-productAccountTeam-modal.component.html',
})
export class ViewProductAccountTeamModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductAccountTeamForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductAccountTeamForViewDto();
        this.item.productAccountTeam = new ProductAccountTeamDto();
    }

    show(item: GetProductAccountTeamForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
