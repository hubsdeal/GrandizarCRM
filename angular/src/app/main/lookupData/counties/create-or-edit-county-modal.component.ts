import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    CountiesServiceProxy,
    CreateOrEditCountyDto,
    CountyCountryLookupTableDto,
    CountyStateLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditCountyModal',
    templateUrl: './create-or-edit-county-modal.component.html',
})
export class CreateOrEditCountyModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    county: CreateOrEditCountyDto = new CreateOrEditCountyDto();

    countryName = '';
    stateName = '';

    allCountrys: CountyCountryLookupTableDto[];
    allStates: CountyStateLookupTableDto[];

    constructor(
        injector: Injector,
        private _countiesServiceProxy: CountiesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(countyId?: number): void {
        if (!countyId) {
            this.county = new CreateOrEditCountyDto();
            this.county.id = countyId;
            this.countryName = '';
            this.stateName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._countiesServiceProxy.getCountyForEdit(countyId).subscribe((result) => {
                this.county = result.county;

                this.countryName = result.countryName;
                this.stateName = result.stateName;

                this.active = true;
                this.modal.show();
            });
        }
        this._countiesServiceProxy.getAllCountryForTableDropdown().subscribe((result) => {
            this.allCountrys = result;
        });
        this._countiesServiceProxy.getAllStateForTableDropdown().subscribe((result) => {
            this.allStates = result;
        });
    }

    save(): void {
        this.saving = true;

        this._countiesServiceProxy
            .createOrEdit(this.county)
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
