import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CountriesServiceProxy, CreateOrEditCountryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditCountryModal',
    templateUrl: './create-or-edit-country-modal.component.html',
})
export class CreateOrEditCountryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    country: CreateOrEditCountryDto = new CreateOrEditCountryDto();

    constructor(
        injector: Injector,
        private _countriesServiceProxy: CountriesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(countryId?: number): void {
        if (!countryId) {
            this.country = new CreateOrEditCountryDto();
            this.country.id = countryId;

            this.active = true;
            this.modal.show();
        } else {
            this._countriesServiceProxy.getCountryForEdit(countryId).subscribe((result) => {
                this.country = result.country;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._countriesServiceProxy
            .createOrEdit(this.country)
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
