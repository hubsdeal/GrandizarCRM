import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    BusinessesServiceProxy,
    CreateOrEditBusinessDto,
    BusinessCountryLookupTableDto,
    BusinessStateLookupTableDto,
    BusinessCityLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BusinessMediaLibraryLookupTableModalComponent } from './business-mediaLibrary-lookup-table-modal.component';

@Component({
    selector: 'createOrEditBusinessModal',
    templateUrl: './create-or-edit-business-modal.component.html',
})
export class CreateOrEditBusinessModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('businessMediaLibraryLookupTableModal', { static: true })
    businessMediaLibraryLookupTableModal: BusinessMediaLibraryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    business: CreateOrEditBusinessDto = new CreateOrEditBusinessDto();

    countryName = '';
    stateName = '';
    cityName = '';
    mediaLibraryName = '';

    allCountrys: BusinessCountryLookupTableDto[];
    allStates: BusinessStateLookupTableDto[];
    allCitys: BusinessCityLookupTableDto[];

    constructor(
        injector: Injector,
        private _businessesServiceProxy: BusinessesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(businessId?: number): void {
        if (!businessId) {
            this.business = new CreateOrEditBusinessDto();
            this.business.id = businessId;
            this.countryName = '';
            this.stateName = '';
            this.cityName = '';
            this.mediaLibraryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._businessesServiceProxy.getBusinessForEdit(businessId).subscribe((result) => {
                this.business = result.business;

                this.countryName = result.countryName;
                this.stateName = result.stateName;
                this.cityName = result.cityName;
                this.mediaLibraryName = result.mediaLibraryName;

                this.active = true;
                this.modal.show();
            });
        }
        this._businessesServiceProxy.getAllCountryForTableDropdown().subscribe((result) => {
            this.allCountrys = result;
        });
        this._businessesServiceProxy.getAllStateForTableDropdown().subscribe((result) => {
            this.allStates = result;
        });
        this._businessesServiceProxy.getAllCityForTableDropdown().subscribe((result) => {
            this.allCitys = result;
        });
    }

    save(): void {
        this.saving = true;

        this._businessesServiceProxy
            .createOrEdit(this.business)
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

    openSelectMediaLibraryModal() {
        this.businessMediaLibraryLookupTableModal.id = this.business.logoMediaLibraryId;
        this.businessMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.businessMediaLibraryLookupTableModal.show();
    }

    setLogoMediaLibraryIdNull() {
        this.business.logoMediaLibraryId = null;
        this.mediaLibraryName = '';
    }

    getNewLogoMediaLibraryId() {
        this.business.logoMediaLibraryId = this.businessMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.businessMediaLibraryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
