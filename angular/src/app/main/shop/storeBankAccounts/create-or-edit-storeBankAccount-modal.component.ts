import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreBankAccountsServiceProxy,
    CreateOrEditStoreBankAccountDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreBankAccountStoreLookupTableModalComponent } from './storeBankAccount-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreBankAccountModal',
    templateUrl: './create-or-edit-storeBankAccount-modal.component.html',
})
export class CreateOrEditStoreBankAccountModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeBankAccountStoreLookupTableModal', { static: true })
    storeBankAccountStoreLookupTableModal: StoreBankAccountStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeBankAccount: CreateOrEditStoreBankAccountDto = new CreateOrEditStoreBankAccountDto();

    storeName = '';

    constructor(
        injector: Injector,
        private _storeBankAccountsServiceProxy: StoreBankAccountsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeBankAccountId?: number): void {
        if (!storeBankAccountId) {
            this.storeBankAccount = new CreateOrEditStoreBankAccountDto();
            this.storeBankAccount.id = storeBankAccountId;
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeBankAccountsServiceProxy.getStoreBankAccountForEdit(storeBankAccountId).subscribe((result) => {
                this.storeBankAccount = result.storeBankAccount;

                this.storeName = result.storeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._storeBankAccountsServiceProxy
            .createOrEdit(this.storeBankAccount)
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

    openSelectStoreModal() {
        this.storeBankAccountStoreLookupTableModal.id = this.storeBankAccount.storeId;
        this.storeBankAccountStoreLookupTableModal.displayName = this.storeName;
        this.storeBankAccountStoreLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeBankAccount.storeId = null;
        this.storeName = '';
    }

    getNewStoreId() {
        this.storeBankAccount.storeId = this.storeBankAccountStoreLookupTableModal.id;
        this.storeName = this.storeBankAccountStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
