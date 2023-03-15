import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CurrenciesServiceProxy, CreateOrEditCurrencyDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditCurrencyModal',
    templateUrl: './create-or-edit-currency-modal.component.html',
})
export class CreateOrEditCurrencyModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    currency: CreateOrEditCurrencyDto = new CreateOrEditCurrencyDto();

    constructor(
        injector: Injector,
        private _currenciesServiceProxy: CurrenciesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(currencyId?: number): void {
        if (!currencyId) {
            this.currency = new CreateOrEditCurrencyDto();
            this.currency.id = currencyId;

            this.active = true;
            this.modal.show();
        } else {
            this._currenciesServiceProxy.getCurrencyForEdit(currencyId).subscribe((result) => {
                this.currency = result.currency;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._currenciesServiceProxy
            .createOrEdit(this.currency)
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
