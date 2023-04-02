import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    DiscountCodeUserHistoriesServiceProxy,
    CreateOrEditDiscountCodeUserHistoryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DiscountCodeUserHistoryDiscountCodeGeneratorLookupTableModalComponent } from './discountCodeUserHistory-discountCodeGenerator-lookup-table-modal.component';
import { DiscountCodeUserHistoryOrderLookupTableModalComponent } from './discountCodeUserHistory-order-lookup-table-modal.component';
import { DiscountCodeUserHistoryContactLookupTableModalComponent } from './discountCodeUserHistory-contact-lookup-table-modal.component';

@Component({
    selector: 'createOrEditDiscountCodeUserHistoryModal',
    templateUrl: './create-or-edit-discountCodeUserHistory-modal.component.html',
})
export class CreateOrEditDiscountCodeUserHistoryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('discountCodeUserHistoryDiscountCodeGeneratorLookupTableModal', { static: true })
    discountCodeUserHistoryDiscountCodeGeneratorLookupTableModal: DiscountCodeUserHistoryDiscountCodeGeneratorLookupTableModalComponent;
    @ViewChild('discountCodeUserHistoryOrderLookupTableModal', { static: true })
    discountCodeUserHistoryOrderLookupTableModal: DiscountCodeUserHistoryOrderLookupTableModalComponent;
    @ViewChild('discountCodeUserHistoryContactLookupTableModal', { static: true })
    discountCodeUserHistoryContactLookupTableModal: DiscountCodeUserHistoryContactLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    discountCodeUserHistory: CreateOrEditDiscountCodeUserHistoryDto = new CreateOrEditDiscountCodeUserHistoryDto();

    discountCodeGeneratorName = '';
    orderInvoiceNumber = '';
    contactFullName = '';

    constructor(
        injector: Injector,
        private _discountCodeUserHistoriesServiceProxy: DiscountCodeUserHistoriesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(discountCodeUserHistoryId?: number): void {
        if (!discountCodeUserHistoryId) {
            this.discountCodeUserHistory = new CreateOrEditDiscountCodeUserHistoryDto();
            this.discountCodeUserHistory.id = discountCodeUserHistoryId;
            this.discountCodeUserHistory.amount = this._dateTimeService.getStartOfDay();
            this.discountCodeUserHistory.date = this._dateTimeService.getStartOfDay();
            this.discountCodeGeneratorName = '';
            this.orderInvoiceNumber = '';
            this.contactFullName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._discountCodeUserHistoriesServiceProxy
                .getDiscountCodeUserHistoryForEdit(discountCodeUserHistoryId)
                .subscribe((result) => {
                    this.discountCodeUserHistory = result.discountCodeUserHistory;

                    this.discountCodeGeneratorName = result.discountCodeGeneratorName;
                    this.orderInvoiceNumber = result.orderInvoiceNumber;
                    this.contactFullName = result.contactFullName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._discountCodeUserHistoriesServiceProxy
            .createOrEdit(this.discountCodeUserHistory)
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
        this.discountCodeUserHistoryDiscountCodeGeneratorLookupTableModal.id =
            this.discountCodeUserHistory.discountCodeGeneratorId;
        this.discountCodeUserHistoryDiscountCodeGeneratorLookupTableModal.displayName = this.discountCodeGeneratorName;
        this.discountCodeUserHistoryDiscountCodeGeneratorLookupTableModal.show();
    }
    openSelectOrderModal() {
        this.discountCodeUserHistoryOrderLookupTableModal.id = this.discountCodeUserHistory.orderId;
        this.discountCodeUserHistoryOrderLookupTableModal.displayName = this.orderInvoiceNumber;
        this.discountCodeUserHistoryOrderLookupTableModal.show();
    }
    openSelectContactModal() {
        this.discountCodeUserHistoryContactLookupTableModal.id = this.discountCodeUserHistory.contactId;
        this.discountCodeUserHistoryContactLookupTableModal.displayName = this.contactFullName;
        this.discountCodeUserHistoryContactLookupTableModal.show();
    }

    setDiscountCodeGeneratorIdNull() {
        this.discountCodeUserHistory.discountCodeGeneratorId = null;
        this.discountCodeGeneratorName = '';
    }
    setOrderIdNull() {
        this.discountCodeUserHistory.orderId = null;
        this.orderInvoiceNumber = '';
    }
    setContactIdNull() {
        this.discountCodeUserHistory.contactId = null;
        this.contactFullName = '';
    }

    getNewDiscountCodeGeneratorId() {
        this.discountCodeUserHistory.discountCodeGeneratorId =
            this.discountCodeUserHistoryDiscountCodeGeneratorLookupTableModal.id;
        this.discountCodeGeneratorName = this.discountCodeUserHistoryDiscountCodeGeneratorLookupTableModal.displayName;
    }
    getNewOrderId() {
        this.discountCodeUserHistory.orderId = this.discountCodeUserHistoryOrderLookupTableModal.id;
        this.orderInvoiceNumber = this.discountCodeUserHistoryOrderLookupTableModal.displayName;
    }
    getNewContactId() {
        this.discountCodeUserHistory.contactId = this.discountCodeUserHistoryContactLookupTableModal.id;
        this.contactFullName = this.discountCodeUserHistoryContactLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
