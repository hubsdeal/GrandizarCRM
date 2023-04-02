import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetCustomerWalletForViewDto, CustomerWalletDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewCustomerWalletModal',
    templateUrl: './view-customerWallet-modal.component.html',
})
export class ViewCustomerWalletModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCustomerWalletForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetCustomerWalletForViewDto();
        this.item.customerWallet = new CustomerWalletDto();
    }

    show(item: GetCustomerWalletForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
