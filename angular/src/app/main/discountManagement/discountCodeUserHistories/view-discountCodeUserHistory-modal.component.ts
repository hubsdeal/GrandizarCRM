import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetDiscountCodeUserHistoryForViewDto,
    DiscountCodeUserHistoryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewDiscountCodeUserHistoryModal',
    templateUrl: './view-discountCodeUserHistory-modal.component.html',
})
export class ViewDiscountCodeUserHistoryModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetDiscountCodeUserHistoryForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetDiscountCodeUserHistoryForViewDto();
        this.item.discountCodeUserHistory = new DiscountCodeUserHistoryDto();
    }

    show(item: GetDiscountCodeUserHistoryForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
