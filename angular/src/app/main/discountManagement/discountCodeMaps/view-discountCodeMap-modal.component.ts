import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetDiscountCodeMapForViewDto, DiscountCodeMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewDiscountCodeMapModal',
    templateUrl: './view-discountCodeMap-modal.component.html',
})
export class ViewDiscountCodeMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetDiscountCodeMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetDiscountCodeMapForViewDto();
        this.item.discountCodeMap = new DiscountCodeMapDto();
    }

    show(item: GetDiscountCodeMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
