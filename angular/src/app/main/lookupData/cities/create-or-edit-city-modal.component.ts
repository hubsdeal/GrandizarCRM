import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    CitiesServiceProxy,
    CreateOrEditCityDto,
    CityCountryLookupTableDto,
    CityStateLookupTableDto,
    CityCountyLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditCityModal',
    templateUrl: './create-or-edit-city-modal.component.html',
})
export class CreateOrEditCityModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    city: CreateOrEditCityDto = new CreateOrEditCityDto();

    countryName = '';
    stateName = '';
    countyName = '';

    allCountrys: CityCountryLookupTableDto[];
    allStates: CityStateLookupTableDto[];
    allCountys: CityCountyLookupTableDto[];

    constructor(
        injector: Injector,
        private _citiesServiceProxy: CitiesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(cityId?: number): void {
        if (!cityId) {
            this.city = new CreateOrEditCityDto();
            this.city.id = cityId;
            this.countryName = '';
            this.stateName = '';
            this.countyName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._citiesServiceProxy.getCityForEdit(cityId).subscribe((result) => {
                this.city = result.city;

                this.countryName = result.countryName;
                this.stateName = result.stateName;
                this.countyName = result.countyName;

                this.active = true;
                this.modal.show();
            });
        }
        this._citiesServiceProxy.getAllCountryForTableDropdown().subscribe((result) => {
            this.allCountrys = result;
        });
        this._citiesServiceProxy.getAllStateForTableDropdown().subscribe((result) => {
            this.allStates = result;
        });
        this._citiesServiceProxy.getAllCountyForTableDropdown().subscribe((result) => {
            this.allCountys = result;
        });
    }

    save(): void {
        this.saving = true;

        this._citiesServiceProxy
            .createOrEdit(this.city)
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
