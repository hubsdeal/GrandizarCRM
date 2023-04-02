import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CustomerWalletsServiceProxy, CreateOrEditCustomerWalletDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CustomerWalletContactLookupTableModalComponent } from './customerWallet-contact-lookup-table-modal.component';
import { CustomerWalletUserLookupTableModalComponent } from './customerWallet-user-lookup-table-modal.component';
import { CustomerWalletCurrencyLookupTableModalComponent } from './customerWallet-currency-lookup-table-modal.component';

@Component({
    selector: 'createOrEditCustomerWalletModal',
    templateUrl: './create-or-edit-customerWallet-modal.component.html',
})
export class CreateOrEditCustomerWalletModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('customerWalletContactLookupTableModal', { static: true })
    customerWalletContactLookupTableModal: CustomerWalletContactLookupTableModalComponent;
    @ViewChild('customerWalletUserLookupTableModal', { static: true })
    customerWalletUserLookupTableModal: CustomerWalletUserLookupTableModalComponent;
    @ViewChild('customerWalletCurrencyLookupTableModal', { static: true })
    customerWalletCurrencyLookupTableModal: CustomerWalletCurrencyLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    customerWallet: CreateOrEditCustomerWalletDto = new CreateOrEditCustomerWalletDto();

    contactFullName = '';
    userName = '';
    currencyName = '';

    constructor(
        injector: Injector,
        private _customerWalletsServiceProxy: CustomerWalletsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(customerWalletId?: number): void {
        if (!customerWalletId) {
            this.customerWallet = new CreateOrEditCustomerWalletDto();
            this.customerWallet.id = customerWalletId;
            this.customerWallet.walletOpeningDate = this._dateTimeService.getStartOfDay();
            this.customerWallet.balanceDate = this._dateTimeService.getStartOfDay();
            this.contactFullName = '';
            this.userName = '';
            this.currencyName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._customerWalletsServiceProxy.getCustomerWalletForEdit(customerWalletId).subscribe((result) => {
                this.customerWallet = result.customerWallet;

                this.contactFullName = result.contactFullName;
                this.userName = result.userName;
                this.currencyName = result.currencyName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._customerWalletsServiceProxy
            .createOrEdit(this.customerWallet)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectContactModal() {
        this.customerWalletContactLookupTableModal.id = this.customerWallet.contactId;
        this.customerWalletContactLookupTableModal.displayName = this.contactFullName;
        this.customerWalletContactLookupTableModal.show();
    }
    openSelectUserModal() {
        this.customerWalletUserLookupTableModal.id = this.customerWallet.userId;
        this.customerWalletUserLookupTableModal.displayName = this.userName;
        this.customerWalletUserLookupTableModal.show();
    }
    openSelectCurrencyModal() {
        this.customerWalletCurrencyLookupTableModal.id = this.customerWallet.currencyId;
        this.customerWalletCurrencyLookupTableModal.displayName = this.currencyName;
        this.customerWalletCurrencyLookupTableModal.show();
    }

    setContactIdNull() {
        this.customerWallet.contactId = null;
        this.contactFullName = '';
    }
    setUserIdNull() {
        this.customerWallet.userId = null;
        this.userName = '';
    }
    setCurrencyIdNull() {
        this.customerWallet.currencyId = null;
        this.currencyName = '';
    }

    getNewContactId() {
        this.customerWallet.contactId = this.customerWalletContactLookupTableModal.id;
        this.contactFullName = this.customerWalletContactLookupTableModal.displayName;
    }
    getNewUserId() {
        this.customerWallet.userId = this.customerWalletUserLookupTableModal.id;
        this.userName = this.customerWalletUserLookupTableModal.displayName;
    }
    getNewCurrencyId() {
        this.customerWallet.currencyId = this.customerWalletCurrencyLookupTableModal.id;
        this.currencyName = this.customerWalletCurrencyLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
