import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { PaymentTypesServiceProxy, CreateOrEditPaymentTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditPaymentTypeModal',
    templateUrl: './create-or-edit-paymentType-modal.component.html',
})
export class CreateOrEditPaymentTypeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    paymentType: CreateOrEditPaymentTypeDto = new CreateOrEditPaymentTypeDto();

    constructor(
        injector: Injector,
        private _paymentTypesServiceProxy: PaymentTypesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(paymentTypeId?: number): void {
        if (!paymentTypeId) {
            this.paymentType = new CreateOrEditPaymentTypeDto();
            this.paymentType.id = paymentTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._paymentTypesServiceProxy.getPaymentTypeForEdit(paymentTypeId).subscribe((result) => {
                this.paymentType = result.paymentType;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._paymentTypesServiceProxy
            .createOrEdit(this.paymentType)
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

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
