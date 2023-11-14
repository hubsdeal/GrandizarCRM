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
    StatesServiceProxy,
    CitiesServiceProxy,
    CountiesServiceProxy,
    HubWidgetMapsServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubMediaLibraryLookupTableModalComponent } from './hub-mediaLibrary-lookup-table-modal.component';
import { SelectItem } from 'primeng/api';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';
import { MatDialog } from '@angular/material/dialog';
import { GeocodingService } from '@app/shared/chat-gpt-response-modal/services/chat-gpt-lat-long.service';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { AppConsts } from '@shared/AppConsts';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';

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
    liveOptions: SelectItem[];
    sidebarVisible2: boolean;

    selectedCountry: any;
    selectedState: any;
    selectedCity: any;
    selectedCounty: any;

    imageSrc: any = 'assets/common/images/location.png';
    public uploader: FileUploader;
    public temporaryPictureUrl: string;
    private _uploaderOptions: FileUploaderOptions = {};

    chatGPTPromt: string;
    productShortDesc: string;
    allHubs: any[];

    telOptions = { initialCountry: 'sa', preferredCountries: ['sa'] };
    dialCode: string;

    @ViewChild('phone') phone: ElementRef;
    isDialCodeAdded: boolean = false;

    constructor(
        injector: Injector,
        private _hubsServiceProxy: HubsServiceProxy,
        private _stateServiceProxy: StatesServiceProxy,
        private _cityServiceProxy: CitiesServiceProxy,
        private _countyServiceProxy: CountiesServiceProxy,
        private _dateTimeService: DateTimeService,
        private dialog: MatDialog,
        private _tokenService: TokenService,
        private _hubWidgetMapsServiceProxy: HubWidgetMapsServiceProxy,
        private geocodingService: GeocodingService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.initFileUploader();
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
        // this._hubsServiceProxy.getAllStateForTableDropdown().subscribe((result) => {
        //     this.allStates = result;
        // });
        // this._hubsServiceProxy.getAllCityForTableDropdown().subscribe((result) => {
        //     this.allCitys = result;
        // });
        // this._hubsServiceProxy.getAllCountyForTableDropdown().subscribe((result) => {
        //     this.allCountys = result;
        // });
        this._hubsServiceProxy.getAllHubTypeForTableDropdown().subscribe((result) => {
            this.allHubTypes = result;
        });
        this._hubsServiceProxy.getAllCurrencyForTableDropdown().subscribe((result) => {
            this.allCurrencys = result;
        });
        this.partnerAndOwnedOptions = [{ label: 'Owned', value: true }, { label: 'Partner', value: false }];
        this.liveOptions = [{ label: 'Live', value: true }, { label: 'Planning', value: false }];
        this._hubWidgetMapsServiceProxy
            .getAllHubForLookupTable(
                '',
                '',
                0,
                10000
            ).subscribe(result => {
                this.allHubs = result.items;
            });
    }

    saveHub(fileToken?: string): void {
        this.saving = true;
        this.hub.fileToken = fileToken;

        if (this.hub.phone) {
            this.phone.nativeElement.dispatchEvent(
                new KeyboardEvent('keyup', { bubbles: true })
            );
        };

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


    save() {
        if (this.uploader.queue != null && this.uploader.queue.length > 0) {
            this.uploader.uploadAll();
        } else {
            this.saveHub();
        }
    }

    fileChangeEvent(event: any) {

        if (event.target.files && event.target.files[0]) {
            var reader = new FileReader();

            reader.readAsDataURL(event.target.files[0]); // read file as data url

            reader.onload = (event) => { // called once readAsDataURL is completed

                this.imageSrc = event.target.result;
            }
        }
    }

    initFileUploader(): void {

        this.uploader = new FileUploader({ url: AppConsts.remoteServiceBaseUrl + '/api/MediaUpload/UploadPicture' });
        this._uploaderOptions.autoUpload = false;
        this._uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
        this._uploaderOptions.removeAfterUpload = true;
        this.uploader.onAfterAddingFile = (file) => {
            file.withCredentials = false;
        };

        this.uploader.onBuildItemForm = (fileItem: FileItem, form: any) => {
            form.append('FileToken', this.guid());
        };

        this.uploader.onSuccessItem = (item, response, status) => {
            const resp = <IAjaxResponse>JSON.parse(response);
            if (resp.success) {
                this.saveHub(resp.result.fileToken);
            } else {
                this.message.error(resp.error.message);
            }
        };

        this.uploader.setOptions(this._uploaderOptions);
    }

    guid(): string {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
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



    onCountryChange(event: any) {
        if (event.value != null) {
            this.hub.countryId = event.value.id;
            this._stateServiceProxy.getAllStateForTableDropdown(event.value.id).subscribe((result) => {
                this.allStates = result;
            });
        }
        console.log("countryId" + this.selectedCountry.displayName);
    }

    onStateChange(event: any) {
        if (event.value != null) {
            this.hub.stateId = event.value.id;
            this._countyServiceProxy.getAllCountyForTableDropdown(this.selectedCountry.id, event.value.id).subscribe((result) => {
                this.allCountys = result;
            });
        }
        console.log("State Id" + this.selectedState.displayName);
    }

    onCountyChange(event: any) {
        if (event.value != null) {
            this.hub.countyId = event.value.id;
            this._cityServiceProxy.getAllCityForTableDropdown(this.selectedCountry.id, this.selectedState.id, event.value.id).subscribe((result) => {
                this.allCitys = result;
            });
        }
        console.log("countyId" + this.selectedCounty.displayName);
    }

    onCityChange(event: any) {
        if (event.value != null) {
            this.hub.cityId = event.value.id;
        }
        console.log("City Id" + this.selectedCity.displayName);

    }



    getCoordinates(fieldName: string) {
        if (this.selectedCountry && this.selectedState && this.selectedCity && this.selectedCounty) {
            this.chatGPTPromt = `Give me only the Latitude and longitude for 
        Country: ${this.selectedCountry.displayName}, 
        State: ${this.selectedState.displayName}, 
        City: ${this.selectedCity}, 
        County: ${this.selectedCounty} 
        as json format as Key latitude and longitude`;
        } else if (this.selectedCountry && this.selectedState && this.selectedCity) {
            this.chatGPTPromt = `Give me only the Latitude and longitude for 
        Country: ${this.selectedCountry.displayName}, 
        State: ${this.selectedState.displayName}, 
        City: ${this.selectedCity} 
        as json format as Key latitude and longitude`;
        } else if (this.selectedCountry && this.selectedState) {
            this.chatGPTPromt = `Give me only the Latitude and longitude for 
        Country: ${this.selectedCountry.displayName}, 
        State: ${this.selectedState.displayName} 
        as json format as Key latitude and longitude`;
        } else if (this.selectedCountry) {
            this.chatGPTPromt = `Give me only the Latitude and longitude for 
        Country: ${this.selectedCountry.displayName} 
        as json format as Key latitude and longitude`;
        }

        var modalTitle = `AI Text Generator - Hub ${fieldName}`;
        const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
            data: { promtFromAnotherComponent: this.chatGPTPromt, feildName: fieldName, modalTitle: modalTitle },
            width: '1100px',
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result.data != null) {
                const responseText = this.extractCoordinates(result.data);
                if (responseText) {
                    this.hub.latitude = responseText.latitude;
                    this.hub.longitude = responseText.longitude;
                }
            }
        });
    }

    private extractCoordinates(responseText: string): { latitude: number, longitude: number } {
        // Remove HTML tags from the response text
        const cleanText = responseText.replace(/<[^>]+>/g, '');

        try {
            const response = JSON.parse(cleanText);

            if (response.latitude && response.longitude) {
                const latitude = response.latitude;
                const longitude = response.longitude;
                return { latitude, longitude };
            }
        } catch (error) {
            // JSON parsing failed, handle the error as needed
            throw new Error('Unable to parse the response as JSON');
        }

        // If the response does not contain the latitude and longitude, return null or throw an error
        throw new Error('Unable to extract coordinates from the response');
    }
    onCountryChangeMobile(event: any) {
        this.dialCode = "+" + event.dialCode;
    }

    onCountryChangePhone(event: any) {
        this.dialCode = "+" + event.dialCode;
    }

    getNumberPhone(event: any) {
        this.isDialCodeAdded = true;
        this.hub.phone = event;
    }

    telInputObject(event: any) {
    }
}
