import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    DiscountCodeByCustomersServiceProxy,
    CreateOrEditDiscountCodeByCustomerDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DiscountCodeByCustomerDiscountCodeGeneratorLookupTableModalComponent } from './discountCodeByCustomer-discountCodeGenerator-lookup-table-modal.component';
import { DiscountCodeByCustomerContactLookupTableModalComponent } from './discountCodeByCustomer-contact-lookup-table-modal.component';

@Component({
    selector: 'createOrEditDiscountCodeByCustomerModal',
    templateUrl: './create-or-edit-discountCodeByCustomer-modal.component.html',
})
export class CreateOrEditDiscountCodeByCustomerModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('discountCodeByCustomerDiscountCodeGeneratorLookupTableModal', { static: true })
    discountCodeByCustomerDiscountCodeGeneratorLookupTableModal: DiscountCodeByCustomerDiscountCodeGeneratorLookupTableModalComponent;
    @ViewChild('discountCodeByCustomerContactLookupTableModal', { static: true })
    discountCodeByCustomerContactLookupTableModal: DiscountCodeByCustomerContactLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    discountCodeByCustomer: CreateOrEditDiscountCodeByCustomerDto = new CreateOrEditDiscountCodeByCustomerDto();

    discountCodeGeneratorName = '';
    contactFullName = '';

    constructor(
        injector: Injector,
        private _discountCodeByCustomersServiceProxy: DiscountCodeByCustomersServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(discountCodeByCustomerId?: number): void {
        if (!discountCodeByCustomerId) {
            this.discountCodeByCustomer = new CreateOrEditDiscountCodeByCustomerDto();
            this.discountCodeByCustomer.id = discountCodeByCustomerId;
            this.discountCodeGeneratorName = '';
            this.contactFullName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._discountCodeByCustomersServiceProxy
                .getDiscountCodeByCustomerForEdit(discountCodeByCustomerId)
                .subscribe((result) => {
                    this.discountCodeByCustomer = result.discountCodeByCustomer;

                    this.discountCodeGeneratorName = result.discountCodeGeneratorName;
                    this.contactFullName = result.contactFullName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._discountCodeByCustomersServiceProxy
            .createOrEdit(this.discountCodeByCustomer)
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

    openSelectDiscountCodeGeneratorModal() {
        this.discountCodeByCustomerDiscountCodeGeneratorLookupTableModal.id =
            this.discountCodeByCustomer.discountCodeGeneratorId;
        this.discountCodeByCustomerDiscountCodeGeneratorLookupTableModal.displayName = this.discountCodeGeneratorName;
        this.discountCodeByCustomerDiscountCodeGeneratorLookupTableModal.show();
    }
    openSelectContactModal() {
        this.discountCodeByCustomerContactLookupTableModal.id = this.discountCodeByCustomer.contactId;
        this.discountCodeByCustomerContactLookupTableModal.displayName = this.contactFullName;
        this.discountCodeByCustomerContactLookupTableModal.show();
    }

    setDiscountCodeGeneratorIdNull() {
        this.discountCodeByCustomer.discountCodeGeneratorId = null;
        this.discountCodeGeneratorName = '';
    }
    setContactIdNull() {
        this.discountCodeByCustomer.contactId = null;
        this.contactFullName = '';
    }

    getNewDiscountCodeGeneratorId() {
        this.discountCodeByCustomer.discountCodeGeneratorId =
            this.discountCodeByCustomerDiscountCodeGeneratorLookupTableModal.id;
        this.discountCodeGeneratorName = this.discountCodeByCustomerDiscountCodeGeneratorLookupTableModal.displayName;
    }
    getNewContactId() {
        this.discountCodeByCustomer.contactId = this.discountCodeByCustomerContactLookupTableModal.id;
        this.contactFullName = this.discountCodeByCustomerContactLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
