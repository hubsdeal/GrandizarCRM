import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetDiscountCodeByCustomerForViewDto,
    DiscountCodeByCustomerDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewDiscountCodeByCustomerModal',
    templateUrl: './view-discountCodeByCustomer-modal.component.html',
})
export class ViewDiscountCodeByCustomerModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetDiscountCodeByCustomerForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetDiscountCodeByCustomerForViewDto();
        this.item.discountCodeByCustomer = new DiscountCodeByCustomerDto();
    }

    show(item: GetDiscountCodeByCustomerForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
