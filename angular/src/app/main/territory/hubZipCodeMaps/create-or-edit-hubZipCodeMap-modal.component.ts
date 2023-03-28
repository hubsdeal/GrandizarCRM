import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { HubZipCodeMapsServiceProxy, CreateOrEditHubZipCodeMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubZipCodeMapHubLookupTableModalComponent } from './hubZipCodeMap-hub-lookup-table-modal.component';
import { HubZipCodeMapCityLookupTableModalComponent } from './hubZipCodeMap-city-lookup-table-modal.component';
import { HubZipCodeMapZipCodeLookupTableModalComponent } from './hubZipCodeMap-zipCode-lookup-table-modal.component';

@Component({
    selector: 'createOrEditHubZipCodeMapModal',
    templateUrl: './create-or-edit-hubZipCodeMap-modal.component.html',
})
export class CreateOrEditHubZipCodeMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubZipCodeMapHubLookupTableModal', { static: true })
    hubZipCodeMapHubLookupTableModal: HubZipCodeMapHubLookupTableModalComponent;
    @ViewChild('hubZipCodeMapCityLookupTableModal', { static: true })
    hubZipCodeMapCityLookupTableModal: HubZipCodeMapCityLookupTableModalComponent;
    @ViewChild('hubZipCodeMapZipCodeLookupTableModal', { static: true })
    hubZipCodeMapZipCodeLookupTableModal: HubZipCodeMapZipCodeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubZipCodeMap: CreateOrEditHubZipCodeMapDto = new CreateOrEditHubZipCodeMapDto();

    hubName = '';
    cityName = '';
    zipCodeName = '';

    constructor(
        injector: Injector,
        private _hubZipCodeMapsServiceProxy: HubZipCodeMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubZipCodeMapId?: number): void {
        if (!hubZipCodeMapId) {
            this.hubZipCodeMap = new CreateOrEditHubZipCodeMapDto();
            this.hubZipCodeMap.id = hubZipCodeMapId;
            this.hubName = '';
            this.cityName = '';
            this.zipCodeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._hubZipCodeMapsServiceProxy.getHubZipCodeMapForEdit(hubZipCodeMapId).subscribe((result) => {
                this.hubZipCodeMap = result.hubZipCodeMap;

                this.hubName = result.hubName;
                this.cityName = result.cityName;
                this.zipCodeName = result.zipCodeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._hubZipCodeMapsServiceProxy
            .createOrEdit(this.hubZipCodeMap)
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

    openSelectHubModal() {
        this.hubZipCodeMapHubLookupTableModal.id = this.hubZipCodeMap.hubId;
        this.hubZipCodeMapHubLookupTableModal.displayName = this.hubName;
        this.hubZipCodeMapHubLookupTableModal.show();
    }
    openSelectCityModal() {
        this.hubZipCodeMapCityLookupTableModal.id = this.hubZipCodeMap.cityId;
        this.hubZipCodeMapCityLookupTableModal.displayName = this.cityName;
        this.hubZipCodeMapCityLookupTableModal.show();
    }
    openSelectZipCodeModal() {
        this.hubZipCodeMapZipCodeLookupTableModal.id = this.hubZipCodeMap.zipCodeId;
        this.hubZipCodeMapZipCodeLookupTableModal.displayName = this.zipCodeName;
        this.hubZipCodeMapZipCodeLookupTableModal.show();
    }

    setHubIdNull() {
        this.hubZipCodeMap.hubId = null;
        this.hubName = '';
    }
    setCityIdNull() {
        this.hubZipCodeMap.cityId = null;
        this.cityName = '';
    }
    setZipCodeIdNull() {
        this.hubZipCodeMap.zipCodeId = null;
        this.zipCodeName = '';
    }

    getNewHubId() {
        this.hubZipCodeMap.hubId = this.hubZipCodeMapHubLookupTableModal.id;
        this.hubName = this.hubZipCodeMapHubLookupTableModal.displayName;
    }
    getNewCityId() {
        this.hubZipCodeMap.cityId = this.hubZipCodeMapCityLookupTableModal.id;
        this.cityName = this.hubZipCodeMapCityLookupTableModal.displayName;
    }
    getNewZipCodeId() {
        this.hubZipCodeMap.zipCodeId = this.hubZipCodeMapZipCodeLookupTableModal.id;
        this.zipCodeName = this.hubZipCodeMapZipCodeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
