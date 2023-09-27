import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetCustomerWalletTransactionForViewDto, CustomerWalletTransactionDto , WalletTransactionType} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewCustomerWalletTransactionModal',
    templateUrl: './view-customerWalletTransaction-modal.component.html'
})
export class ViewCustomerWalletTransactionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCustomerWalletTransactionForViewDto;
    walletTransactionType = WalletTransactionType;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetCustomerWalletTransactionForViewDto();
        this.item.customerWalletTransaction = new CustomerWalletTransactionDto();
    }

    show(item: GetCustomerWalletTransactionForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
