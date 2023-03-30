import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductCategoryTeamForViewDto, ProductCategoryTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductCategoryTeamModal',
    templateUrl: './view-productCategoryTeam-modal.component.html',
})
export class ViewProductCategoryTeamModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductCategoryTeamForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductCategoryTeamForViewDto();
        this.item.productCategoryTeam = new ProductCategoryTeamDto();
    }

    show(item: GetProductCategoryTeamForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
