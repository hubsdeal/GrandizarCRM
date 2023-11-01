import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreLocationsServiceProxy, CreateOrEditStoreLocationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreLocationCityLookupTableModalComponent } from './storeLocation-city-lookup-table-modal.component';
import { StoreLocationStateLookupTableModalComponent } from './storeLocation-state-lookup-table-modal.component';
import { StoreLocationCountryLookupTableModalComponent } from './storeLocation-country-lookup-table-modal.component';
import { StoreLocationStoreLookupTableModalComponent } from './storeLocation-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreLocationModal',
    templateUrl: './create-or-edit-storeLocation-modal.component.html',
})
export class CreateOrEditStoreLocationModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeLocationCityLookupTableModal', { static: true })
    storeLocationCityLookupTableModal: StoreLocationCityLookupTableModalComponent;
    @ViewChild('storeLocationStateLookupTableModal', { static: true })
    storeLocationStateLookupTableModal: StoreLocationStateLookupTableModalComponent;
    @ViewChild('storeLocationCountryLookupTableModal', { static: true })
    storeLocationCountryLookupTableModal: StoreLocationCountryLookupTableModalComponent;
    @ViewChild('storeLocationStoreLookupTableModal', { static: true })
    storeLocationStoreLookupTableModal: StoreLocationStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeLocation: CreateOrEditStoreLocationDto = new CreateOrEditStoreLocationDto();

    cityName = '';
    stateName = '';
    countryName = '';
    storeName = '';
    storeId:number;
    constructor(
        injector: Injector,
        private _storeLocationsServiceProxy: StoreLocationsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeLocationId?: number): void {
        if (!storeLocationId) {
            this.storeLocation = new CreateOrEditStoreLocationDto();
            this.storeLocation.id = storeLocationId;
            this.cityName = '';
            this.stateName = '';
            this.countryName = '';
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeLocationsServiceProxy.getStoreLocationForEdit(storeLocationId).subscribe((result) => {
                this.storeLocation = result.storeLocation;
                this.storeId = result.storeLocation.storeId;
                this.cityName = result.cityName;
                this.stateName = result.stateName;
                this.countryName = result.countryName;
                this.storeName = result.storeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this.storeLocation.storeId = this.storeId;
        this._storeLocationsServiceProxy
            .createOrEdit(this.storeLocation)
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

    openSelectCityModal() {
        this.storeLocationCityLookupTableModal.id = this.storeLocation.cityId;
        this.storeLocationCityLookupTableModal.displayName = this.cityName;
        this.storeLocationCityLookupTableModal.show();
    }
    openSelectStateModal() {
        this.storeLocationStateLookupTableModal.id = this.storeLocation.stateId;
        this.storeLocationStateLookupTableModal.displayName = this.stateName;
        this.storeLocationStateLookupTableModal.show();
    }
    openSelectCountryModal() {
        this.storeLocationCountryLookupTableModal.id = this.storeLocation.countryId;
        this.storeLocationCountryLookupTableModal.displayName = this.countryName;
        this.storeLocationCountryLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.storeLocationStoreLookupTableModal.id = this.storeLocation.storeId;
        this.storeLocationStoreLookupTableModal.displayName = this.storeName;
        this.storeLocationStoreLookupTableModal.show();
    }

    setCityIdNull() {
        this.storeLocation.cityId = null;
        this.cityName = '';
    }
    setStateIdNull() {
        this.storeLocation.stateId = null;
        this.stateName = '';
    }
    setCountryIdNull() {
        this.storeLocation.countryId = null;
        this.countryName = '';
    }
    setStoreIdNull() {
        this.storeLocation.storeId = null;
        this.storeName = '';
    }

    getNewCityId() {
        this.storeLocation.cityId = this.storeLocationCityLookupTableModal.id;
        this.cityName = this.storeLocationCityLookupTableModal.displayName;
    }
    getNewStateId() {
        this.storeLocation.stateId = this.storeLocationStateLookupTableModal.id;
        this.stateName = this.storeLocationStateLookupTableModal.displayName;
    }
    getNewCountryId() {
        this.storeLocation.countryId = this.storeLocationCountryLookupTableModal.id;
        this.countryName = this.storeLocationCountryLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.storeLocation.storeId = this.storeLocationStoreLookupTableModal.id;
        this.storeName = this.storeLocationStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
