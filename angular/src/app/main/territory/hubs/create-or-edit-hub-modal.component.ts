import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    HubsServiceProxy,
    CreateOrEditHubDto,
    HubCountryLookupTableDto,
    HubStateLookupTableDto,
    HubCityLookupTableDto,
    HubCountyLookupTableDto,
    HubHubTypeLookupTableDto,
    HubCurrencyLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubMediaLibraryLookupTableModalComponent } from './hub-mediaLibrary-lookup-table-modal.component';
import { SelectItem } from 'primeng/api';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';
import { MatDialog } from '@angular/material/dialog';
import { GeocodingService } from '@app/shared/chat-gpt-response-modal/services/chat-gpt-lat-long.service';

@Component({
    selector: 'createOrEditHubModal',
    templateUrl: './create-or-edit-hub-modal.component.html',
})
export class CreateOrEditHubModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubMediaLibraryLookupTableModal', { static: true })
    hubMediaLibraryLookupTableModal: HubMediaLibraryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hub: CreateOrEditHubDto = new CreateOrEditHubDto();

    countryName = '';
    stateName = '';
    cityName = '';
    countyName = '';
    hubTypeName = '';
    currencyName = '';
    mediaLibraryName = '';

    allCountrys: HubCountryLookupTableDto[];
    allStates: HubStateLookupTableDto[];
    allCitys: HubCityLookupTableDto[];
    allCountys: HubCountyLookupTableDto[];
    allHubTypes: HubHubTypeLookupTableDto[];
    allCurrencys: HubCurrencyLookupTableDto[];

    partnerAndOwnedOptions: SelectItem[];
    sidebarVisible2: boolean;

    constructor(
        injector: Injector,
        private _hubsServiceProxy: HubsServiceProxy,
        private _dateTimeService: DateTimeService,
        private dialog: MatDialog,
        private geocodingService: GeocodingService
    ) {
        super(injector);
    }

    show(hubId?: number): void {
        if (!hubId) {
            this.hub = new CreateOrEditHubDto();
            this.hub.id = hubId;
            this.countryName = '';
            this.stateName = '';
            this.cityName = '';
            this.countyName = '';
            this.hubTypeName = '';
            this.currencyName = '';
            this.mediaLibraryName = '';
            this.hub.partnerOrOwned = true;

            this.active = true;
            this.modal.show();
        } else {
            this._hubsServiceProxy.getHubForEdit(hubId).subscribe((result) => {
                this.hub = result.hub;

                this.countryName = result.countryName;
                this.stateName = result.stateName;
                this.cityName = result.cityName;
                this.countyName = result.countyName;
                this.hubTypeName = result.hubTypeName;
                this.currencyName = result.currencyName;
                this.mediaLibraryName = result.mediaLibraryName;

                this.active = true;
                this.modal.show();
            });
        }
        this._hubsServiceProxy.getAllCountryForTableDropdown().subscribe((result) => {
            this.allCountrys = result;
        });
        this._hubsServiceProxy.getAllStateForTableDropdown().subscribe((result) => {
            this.allStates = result;
        });
        // this._hubsServiceProxy.getAllCityForTableDropdown().subscribe((result) => {
        //     this.allCitys = result;
        // });
        this._hubsServiceProxy.getAllCountyForTableDropdown().subscribe((result) => {
            this.allCountys = result;
        });
        this._hubsServiceProxy.getAllHubTypeForTableDropdown().subscribe((result) => {
            this.allHubTypes = result;
        });
        this._hubsServiceProxy.getAllCurrencyForTableDropdown().subscribe((result) => {
            this.allCurrencys = result;
        });
        this.partnerAndOwnedOptions = [{ label: 'Corporate Owned', value: true }, { label: 'Partner', value: false }];
    }

    save(): void {
        this.saving = true;

        this._hubsServiceProxy
            .createOrEdit(this.hub)
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
        this.hubMediaLibraryLookupTableModal.id = this.hub.pictureMediaLibraryId;
        this.hubMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.hubMediaLibraryLookupTableModal.show();
    }

    setPictureMediaLibraryIdNull() {
        this.hub.pictureMediaLibraryId = null;
        this.mediaLibraryName = '';
    }

    getNewPictureMediaLibraryId() {
        this.hub.pictureMediaLibraryId = this.hubMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.hubMediaLibraryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void { }


    // openAiModal(feildName: string): void {
    //     var promtText = "Get latitude and longitude for New York"
    //     var modalTitle = "AI Latitude Longitude Response"
    //     const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
    //       data: { promtFromAnotherComponent: promtText, feildName: feildName, modalTitle: modalTitle },
    //       width: '1100px',
    //     });
    
    //     dialogRef.afterClosed().subscribe(result => {
    //       console.log(result)
    //       //this.bindingData = result.data;
    //     });
    //   }

    async getCoordinates() {
        try {
          const location = 'Get Latitude and longitude for New York'; // Replace with the desired location
          const coordinates = await this.geocodingService.invokeGPT(location);
          console.log('Coordinates:', coordinates);
          if(coordinates){
            this.hub.latitude = coordinates.latitude;
            this.hub.longitude = coordinates.longitude;
          }
        } catch (error) {
          console.error('Error:', error);
        }
      }
}
