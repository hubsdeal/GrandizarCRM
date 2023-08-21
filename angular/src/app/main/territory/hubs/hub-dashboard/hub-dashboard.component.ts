import { AfterViewInit, Component, Injector, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';
import { GeocodingService } from '@app/shared/chat-gpt-response-modal/services/chat-gpt-lat-long.service';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditHubDto, HubCountryLookupTableDto, HubStateLookupTableDto, HubCityLookupTableDto, HubCountyLookupTableDto, HubHubTypeLookupTableDto, HubCurrencyLookupTableDto, CountiesServiceProxy, CitiesServiceProxy, StatesServiceProxy, HubsServiceProxy, HubWidgetMapsServiceProxy, HubTopStatsForViewDto } from '@shared/service-proxies/service-proxies';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { SelectItem } from 'primeng/api';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-hub-dashboard',
  templateUrl: './hub-dashboard.component.html',
  styleUrls: ['./hub-dashboard.component.css']
})
export class HubDashboardComponent extends AppComponentBase implements OnInit, AfterViewInit {
  hubId: number;
  hubShortDesc: string;
  bindingData: any;

  hub: CreateOrEditHubDto = new CreateOrEditHubDto();

  countryName = '';
  stateName = '';
  cityName = '';
  countyName = '';
  hubTypeName = '';
  currencyName = '';

  allCountrys: HubCountryLookupTableDto[];
  allStates: HubStateLookupTableDto[];
  allCitys: HubCityLookupTableDto[];
  allCountys: HubCountyLookupTableDto[];
  allHubTypes: HubHubTypeLookupTableDto[];
  allCurrencys: HubCurrencyLookupTableDto[];

  partnerAndOwnedOptions: SelectItem[];
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
  hubAddress: string;

  allHubs: any[];

  saving = false;

  countView: HubTopStatsForViewDto = new HubTopStatsForViewDto();

  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService,
    private _hubsServiceProxy: HubsServiceProxy,
    private _stateServiceProxy: StatesServiceProxy,
    private _cityServiceProxy: CitiesServiceProxy,
    private _countyServiceProxy: CountiesServiceProxy,
    private _hubWidgetMapsServiceProxy: HubWidgetMapsServiceProxy,
    private geocodingService: GeocodingService,
    private dialog: MatDialog
  ) {
    super(injector);
  }

  ngOnInit(): void {
    let hubId = this.route.snapshot.paramMap.get('hubId')
    this.hubId = parseInt(hubId);
    this.getHubDetails(this.hubId);
    this.loadAllDropdown();
    this.initFileUploader();
  }
  ngAfterViewInit() {

  }

  getHubDetails(id: number) {
    this._hubsServiceProxy.getHubForEdit(id).subscribe(result => {
      this.hub = result.hub;
      this.countryName = result.countryName;
      this.stateName = result.stateName;
      this.cityName = result.cityName;
      this.countyName = result.countyName;
      this.hubTypeName = result.hubTypeName;
      this.currencyName = result.currencyName;
      var values = [];

      if (result.countyName) {
        values.push(result.countyName);
      }
      if (result.cityName) {
        values.push(result.cityName);
      }
      if (result.stateName) {
        values.push(result.stateName);
      }
      if (result.countryName) {
        values.push(result.countryName);
      }

      this.hubAddress = values.join(', ');
      //this.mediaLibraryName = result.pi;
      this.imageSrc = result.picture;
      this._hubsServiceProxy.getHubTopStats(id).subscribe(result => {
        this.countView = result;
      });
    });
  }

  saveHub(fileToken?: string): void {
    this.saving = true;
    this.hub.fileToken = fileToken;

    this._hubsServiceProxy
      .createOrEdit(this.hub)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
      });
  }


  save() {
    if (this.uploader.queue != null && this.uploader.queue.length > 0) {
      this.uploader.uploadAll();
    } else {
      this.saveHub();
    }
  }


  loadAllDropdown() {
    this._hubWidgetMapsServiceProxy
      .getAllHubForLookupTable(
        '',
        '',
        0,
        10000
      ).subscribe(result => {
        this.allHubs = result.items;
      });
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
    this.partnerAndOwnedOptions = [{ label: 'Corporate Owned', value: true }, { label: 'Partner', value: false }];
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

  async getCoordinates() {
    try {
      if (this.selectedCountry && this.selectedState && this.selectedCity && this.selectedCounty) {
        this.chatGPTPromt = 'Give me only the Latitude and longitude for ' + this.selectedCountry.displayName + ', ' + this.selectedState.displayName + ', ' + this.selectedCounty.displayName + ', ' + this.selectedCity.displayName + ' as json format as Key latitude and longitude';
      } else if (this.selectedCountry && this.selectedState && this.selectedCounty) {
        this.chatGPTPromt = 'Give me only the Latitude and longitude for ' + this.selectedCountry.displayName + ', ' + this.selectedState.displayName + ', ' + this.selectedCounty.displayName + ' as json format as Key latitude and longitude';
      } else if (this.selectedCountry && this.selectedState) {
        this.chatGPTPromt = 'Give me only the Latitude and longitude for ' + this.selectedCountry.displayName + ', ' + this.selectedState.displayName + ' as json format as Key latitude and longitude';
      } else if (this.selectedCountry) {
        this.chatGPTPromt = 'Give me only the Latitude and longitude for ' + this.selectedCountry.displayName + ' as json format as Key latitude and longitude';
      }
      console.log(this.chatGPTPromt);
      const location = 'Give me Latitude and longitude for New York as json format as Key latitude and longitude'; // Replace with the desired location
      const coordinates = await this.geocodingService.invokeGPT(this.chatGPTPromt);
      console.log('Coordinates:', coordinates);
      if (coordinates) {
        this.hub.latitude = coordinates.latitude;
        this.hub.longitude = coordinates.longitude;
      }
    } catch (error) {
      console.error('Error:', error);
    }
  }



  openAiModalPr(feildName: string): void {
    this.hubShortDesc = "Write a description for a HUB where hub name is New Delhi and hub type is City"
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: this.hubShortDesc, feildName: feildName },
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result)
      this.bindingData = result.data;
    });
  }
}
