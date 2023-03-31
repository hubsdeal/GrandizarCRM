import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetPaymentTypeForViewDto, PaymentTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewPaymentTypeModal',
    templateUrl: './view-paymentType-modal.component.html',
})
export class ViewPaymentTypeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetPaymentTypeForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetPaymentTypeForViewDto();
        this.item.paymentType = new PaymentTypeDto();
    }

    show(item: GetPaymentTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
