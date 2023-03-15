import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ZipCodesServiceProxy,
    CreateOrEditZipCodeDto,
    ZipCodeCountryLookupTableDto,
    ZipCodeStateLookupTableDto,
    ZipCodeCityLookupTableDto,
    ZipCodeCountyLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditZipCodeModal',
    templateUrl: './create-or-edit-zipCode-modal.component.html',
})
export class CreateOrEditZipCodeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    zipCode: CreateOrEditZipCodeDto = new CreateOrEditZipCodeDto();

    countryName = '';
    stateName = '';
    cityName = '';
    countyName = '';

    allCountrys: ZipCodeCountryLookupTableDto[];
    allStates: ZipCodeStateLookupTableDto[];
    allCitys: ZipCodeCityLookupTableDto[];
    allCountys: ZipCodeCountyLookupTableDto[];

    constructor(
        injector: Injector,
        private _zipCodesServiceProxy: ZipCodesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(zipCodeId?: number): void {
        if (!zipCodeId) {
            this.zipCode = new CreateOrEditZipCodeDto();
            this.zipCode.id = zipCodeId;
            this.countryName = '';
            this.stateName = '';
            this.cityName = '';
            this.countyName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._zipCodesServiceProxy.getZipCodeForEdit(zipCodeId).subscribe((result) => {
                this.zipCode = result.zipCode;

                this.countryName = result.countryName;
                this.stateName = result.stateName;
                this.cityName = result.cityName;
                this.countyName = result.countyName;

                this.active = true;
                this.modal.show();
            });
        }
        this._zipCodesServiceProxy.getAllCountryForTableDropdown().subscribe((result) => {
            this.allCountrys = result;
        });
        this._zipCodesServiceProxy.getAllStateForTableDropdown().subscribe((result) => {
            this.allStates = result;
        });
        this._zipCodesServiceProxy.getAllCityForTableDropdown().subscribe((result) => {
            this.allCitys = result;
        });
        this._zipCodesServiceProxy.getAllCountyForTableDropdown().subscribe((result) => {
            this.allCountys = result;
        });
    }

    save(): void {
        this.saving = true;

        this._zipCodesServiceProxy
            .createOrEdit(this.zipCode)
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
