import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditStoreDto, CreateOrEditStoreMediaDto, CreateOrEditStoreZipCodeMapDto, GetStoreAccountTeamForViewDto, GetStoreBusinessHourForViewDto, GetStoreLocationForViewDto, GetStoreMediaForViewDto, RatingLikesServiceProxy, StatesServiceProxy, StoreAccountTeamDto, StoreAccountTeamsServiceProxy, StoreBusinessHoursServiceProxy, StoreCountryLookupTableDto, StoreLocationDto, StoreLocationsServiceProxy, StoreMediasServiceProxy, StoreTopStatsForViewDto, StoreZipCodeMapsServiceProxy, StoresServiceProxy } from '@shared/service-proxies/service-proxies';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { SelectItem } from 'primeng/api';
import { finalize } from 'rxjs';
import { CreateOrEditStoreMediaModalComponent } from '../../storeMedias/create-or-edit-storeMedia-modal.component';
import { DomSanitizer } from '@angular/platform-browser';
import { GeocodingService } from '@app/shared/chat-gpt-response-modal/services/chat-gpt-lat-long.service';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { StoreMasterTagSettingsServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreTaskMapModalComponent } from '@app/main/taskManagement/storeTaskMaps/create-or-edit-storeTaskMap-modal.component';
import { CreateOrEditStoreNoteModalComponent } from '../../storeNotes/create-or-edit-storeNote-modal.component';
import { CreateOrEditStoreAccountTeamModalComponent } from '../../storeAccountTeams/create-or-edit-storeAccountTeam-modal.component';
import { OneToOneConnectModalComponent } from '@app/shared/one-to-one-connect-modal/one-to-one-connect-modal.component';
import { CreateOrEditStoreBusinessHourModalComponent } from '../../storeBusinessHours/create-or-edit-storeBusinessHour-modal.component';
import { StoreZipCodeMapDto } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreLocationModalComponent } from '../../storeLocations/create-or-edit-storeLocation-modal.component';

@Component({
  selector: 'app-store-dashboard',
  templateUrl: './store-dashboard.component.html',
  styleUrls: ['./store-dashboard.component.css']
})
export class StoreDashboardComponent extends AppComponentBase implements OnInit, AfterViewInit {
  @ViewChild('createOrEditStoreMediaModal', { static: true })
  createOrEditStoreMediaModal: CreateOrEditStoreMediaModalComponent;
  @ViewChild('createOrEditStoreNoteModal', { static: true })
  createOrEditStoreNoteModal: CreateOrEditStoreNoteModalComponent;
  @ViewChild('createOrEditStoreTaskMapModal', { static: true })
  createOrEditStoreTaskMapModal: CreateOrEditStoreTaskMapModalComponent;
  @ViewChild('createOrEditStoreAccountTeamModal', { static: true })
  createOrEditStoreAccountTeamModal: CreateOrEditStoreAccountTeamModalComponent;
  @ViewChild('oneToOneConnectModal', { static: true }) oneToOneConnectModal: OneToOneConnectModalComponent;
  @ViewChild('createOrEditStoreBusinessHourModal', { static: true })
  createOrEditStoreBusinessHourModal: CreateOrEditStoreBusinessHourModalComponent;

  saving = false;
  storeId: number;
  productShortDesc: string;
  modalTile: string
  bindingData: any;
  localOrVirtualStoreOptions: SelectItem[];
  storeVerifiedOptions: SelectItem[];
  storePublishedOptions: SelectItem[];
  store: CreateOrEditStoreDto;

  tags: string[] = [];

  chatGPTPromt: string;

  countView: StoreTopStatsForViewDto = new StoreTopStatsForViewDto();
  stateName: string;
  countryName: string;
  picture: string;
  teams: any[] = [];
  taxOptions: any[] = [];
  video: string;
  storeLogo: string;

  publishOptions: SelectItem[];
  publish: Boolean = false;

  numberOfRatings: number;
  ratingScore: number;


  numberofTasks: number;
  numberOfNotes: number;
  primaryCategoryName: string;

  imageSrc: any;
  public uploader: FileUploader;
  public temporaryPictureUrl: string;
  private _uploaderOptions: FileUploaderOptions = {};

  allPrimaryCategories: any[];
  stateOptions: any = [];
  countryOptions: StoreCountryLookupTableDto[];

  isUrlAvailble: boolean = false;
  isUrlNotAvailble: boolean = false;

  images: GetStoreMediaForViewDto[] = [];
  videos: any[] = [];

  selectedState: any;

  selectedStoreTagSettingCategory: any;
  storeTagSettingCategoryOptions: any = []

  storeTags = [
    {
      id: "1",
      value: "Sales Status"
    },

    {
      id: "2",
      value: "Delivery Types"
    },
    {
      id: "3",
      value: "Customer Group"
    }
  ];
  storeTagSettingCategoryId: number;

  showCalendarView: boolean;
  showListView: boolean;

  storeHour: GetStoreBusinessHourForViewDto = new GetStoreBusinessHourForViewDto();


  totalRecordsCount: number;
  allEmployee: GetStoreAccountTeamForViewDto[] = [];

  storeZipCodeMap: CreateOrEditStoreZipCodeMapDto = new CreateOrEditStoreZipCodeMapDto();
  allStoreZipcodes: any[] = [];

  @ViewChild('createOrEditStoreLocationModal', { static: true }) createOrEditStoreLocationModal: CreateOrEditStoreLocationModalComponent;
  storeLocations: GetStoreLocationForViewDto[] = [];

  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService,
    private _storeServiceProxy: StoresServiceProxy,
    private _storeMediaServiceProxy: StoreMediasServiceProxy,
    private _stateServiceProxy: StatesServiceProxy,
    private _sanitizer: DomSanitizer,
    private geocodingService: GeocodingService,
    private _storeMasterTagSettingsServiceProxy: StoreMasterTagSettingsServiceProxy,
    private _ratingLikesServiceProxy: RatingLikesServiceProxy,
    private _storeAccountTeamsServiceProxy: StoreAccountTeamsServiceProxy,
    private _storeHoursServiceProxy: StoreBusinessHoursServiceProxy,
    private _storeZipCodeMapsServiceProxy: StoreZipCodeMapsServiceProxy,
    private _storeLocationServiceProxy: StoreLocationsServiceProxy,
    private dialog: MatDialog
  ) {
    super(injector);
    this.loadAllDropdown();
  }

  ngOnInit(): void {
    let storeId = this.route.snapshot.paramMap.get('storeId')
    this.storeId = parseInt(storeId);
    this.getStoreDetails(this.storeId);
    this.getStoreHour(this.storeId);
    this.getStoreLocations(this.storeId);
    this.getALllStoreZipCodes(this.storeId);
    this.getAllStoreAccountTeams();
    this.initFileUploader();
    this.localOrVirtualStoreOptions = [{ label: 'Local Store', value: false }, { label: 'Virtual Store', value: true }];
    this.storeVerifiedOptions = [{ label: 'Verified', value: true }, { label: 'Not Verified', value: false }];
    this.storePublishedOptions = [{ label: 'Un Published', value: false }, { label: 'Published', value: true }];
  }
  ngAfterViewInit() {

  }


  loadAllDropdown() {
    this._storeServiceProxy.getAllCountryForTableDropdown().subscribe(result => {
      this.countryOptions = result;
    });
    this._storeMasterTagSettingsServiceProxy.getAllStoreTagSettingCategoryForLookupTable('', '', 0, 1000).subscribe(result => {
      this.storeTagSettingCategoryOptions = result.items;
    });
  }

  onCountryChange(event: any) {
    if (event.value != null) {
      this._stateServiceProxy.getAllStateForTableDropdown(event.value).subscribe((result) => {
        this.stateOptions = result;
      });
    }
  }



  openAiModal(feildName: string): void {
    this.productShortDesc = "Write Store About where store name is Saffola"
    var modalTitle = "AI Text Generator - About Store"
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: this.productShortDesc, feildName: feildName, modalTitle: modalTitle },
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result)
      this.bindingData = result.data;
    });
  }

  getStoreDetails(id: number) {
    this._storeServiceProxy.getStoreForEdit(id).subscribe(result => {
      this.store = result.store;
      if (this.store.countryId == null) {
        this.store.countryId = AppConsts.defaultCountryId;
      }
      this.stateName = result.stateName;
      this.countryName = result.countryName;
      this.storeLogo = result.picture;
      this.picture = result.picture;
      this.tags = result.storeTags;
      this.publish = result.store.isPublished ? true : false;
      this.numberofTasks = result.numberOfTasks;
      this.numberOfNotes = result.numberOfNotes;
      this.numberOfRatings = result.numberOfRatings;
      this.ratingScore = result.ratingScore;
      this.primaryCategoryName = result.primaryCategoryName;
      this.storeTagSettingCategoryId = result.store.storeTagSettingCategoryId;
      if (result.picture) {
        this.imageSrc = result.picture;
      }
      console.log(result)
      this.getStoreMedia();
      if (this.store.stateId && this.store.countryId) {
        this._stateServiceProxy.getAllStateForTableDropdown(this.store.countryId).subscribe((result) => {
          this.stateOptions = result;
        });
      }
    });
    this._storeServiceProxy.getStoreTopStats(id).subscribe(result => {
      this.countView = result;
    });
  }

  checkUrlAvailability(id: number, url: string) {
    this._storeServiceProxy.checkUrlAvailability(id, url).subscribe(result => {
      if (result) {
        this.isUrlAvailble = true;
        this.isUrlNotAvailble = false;
      } else {
        this.isUrlNotAvailble = true;
        this.isUrlAvailble = false;
      }
    });
  }



  saveStoreInfo() {
    if (this.uploader.queue != null && this.uploader.queue.length > 0) {
      this.uploader.uploadAll();
    } else {
      this.saveStore();
    }
  }
  saveStore(fileToken?: string): void {
    this.saving = true;

    this.store.fileToken = fileToken;
    this.store.storeUrl = this.store.storeUrl.toLowerCase();
    this._storeServiceProxy.createOrEdit(this.store)
      .pipe(finalize(() => { this.saving = false; }))
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.getStoreDetails(this.storeId);
      });

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
        this.saveStore(resp.result.fileToken);
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

  //store medias
  createStoreMediaPhoto(): void {
    this.createOrEditStoreMediaModal.selectUploadPhoto = true;
    this.createOrEditStoreMediaModal.storeId = this.storeId;
    this.createOrEditStoreMediaModal.show();
  }

  createStoreMediaVideo(): void {
    this.createOrEditStoreMediaModal.selectAddVideo = true;
    this.createOrEditStoreMediaModal.storeId = this.storeId;
    this.createOrEditStoreMediaModal.show();
  }


  getStoreMedia() {
    this.images = [];
    this.videos = [];
    this._storeMediaServiceProxy.getAllByStoreIdForStoreBuilder(
      this.storeId
    ).subscribe(result => {
      this.images.push(...result.items.filter(x => x.picture != null && x.videoUrl == null));
      if (this.images[0] != null) {
        this.picture = this.images[0].picture;
      }
      this.videos.push(...result.items.filter(x => x.videoUrl != null));
    });
  }

  deleteStoreMedia(id: number) {
    this._storeMediaServiceProxy.delete(id).subscribe(result => {
      this.notify.success(this.l('DeletedSuccessfully'));
      this.getStoreDetails(this.storeId);
    })
  }

  onStorePhotoOrVideoClick(data: any) {
    if (data.picture) {
      this.picture = data.picture;
    } else if (data.videoUrl) {
      this.video = data.videoUrl;
    }
  }
  getSafeEmbeddedVideoUrl(url: string) {
    return this._sanitizer.bypassSecurityTrustResourceUrl(url);
  }

  uploadStoreMedia(event: any) {
    if (event) {
      var media = new CreateOrEditStoreMediaDto();
      media.storeId = this.storeId;
      media.mediaLibraryId = event;
      this._storeMediaServiceProxy.createOrEdit(media).subscribe(result => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.getStoreDetails(this.storeId);
      });
    }
  }


  openAiModalForLatLong(fieldName: string): void {
    const storeName = this.store.name;
    this.chatGPTPromt = `Give me only the Latitude and longitude for 
    Country: ${this.countryName}, 
    State: ${this.stateName}, 
    City: ${this.store.city}, 
    Zipcode: ${this.store.zipCode} 
    as json format as Key latitude and longitude`;

    var modalTitle = `AI Text Generator - Store ${fieldName}`;
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: this.chatGPTPromt, feildName: fieldName, modalTitle: modalTitle },
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.data != null) {
        const responseText = this.extractCoordinates(result.data);
        if (responseText) {
          this.store.latitude = responseText.latitude;
          this.store.longitude = responseText.longitude;
        }
      }
    });
  }

  openAiModalForLatLongForOperator(fieldName: string): void {
    const storeName = this.store.name;
    if (this.countryName || this.stateName && this.store.city || this.store.zipCode) {
      this.chatGPTPromt = `Give me only the Latitude and longitude for 
    Country: ${this.countryName}, 
    State: ${this.stateName}, 
    City: ${this.store.city}, 
    Zipcode: ${this.store.zipCode} 
    as json format as Key latitude and longitude`;
    }

    var modalTitle = `AI Text Generator - Store ${fieldName}`;
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: this.chatGPTPromt, feildName: fieldName, modalTitle: modalTitle },
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.data != null) {
        const responseText = this.extractCoordinates(result.data);
        if (responseText) {
          this.store.latitude = responseText.latitude;
          this.store.longitude = responseText.longitude;
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


  copyPasteStoreDataAI(fieldName: string): void {
    this.chatGPTPromt = `Please take one value for address, Email, Web and convert this data to json format based on key and pair and insert just the specific data. 
    Ex. for mobile number just insert only the number and same goes for the others fields.
    Note: I just only want to insert these data to 
    Name, 
    Full Address, 
    Address, 
    Description, 
    Phone Number, 
    Mobile number, 
    Email, 
    Web
    Where Phone and Mobile is number.
    
    'Insert your data here'`;
    var modalTitle = `AI Text Generator - ${fieldName}`;
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: this.chatGPTPromt, feildName: fieldName, modalTitle: modalTitle },
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.data != null) {
        console.log(result.data);
        const responseText = this.extractStoreData(result.data);
        if (responseText) {
          this.store.name = responseText.Name;
          this.store.fullAddress = responseText.FullAddress;
          this.store.address = responseText.Address;
          this.store.description = responseText.Description;
          this.store.phone = responseText.PhoneNumber;
          this.store.mobile = responseText.MobileNumber;
          this.store.email = responseText.Email;
          this.store.website = responseText.Web;
        }
      }
    });
  }

  private extractStoreData(responseText: string): { Name: string, FullAddress: string, Address: string, Description: string, PhoneNumber: string, MobileNumber: string, Email: string, Web: string } {
    // Remove HTML tags and line breaks from the response text
    const cleanText = responseText.replace(/<br>/g, '').replace(/\n/g, '');
    console.log(cleanText);
    try {
      const response = JSON.parse(cleanText);

      if (response.Name && response['Full Address'] && response.Address && response.Description && response['Phone Number'] && response['Mobile Number'] && response.Email && response.Web) {
        const Name = response.Name;
        const FullAddress = response['Full Address'];
        const Address = response.Address;
        const Description = response.Description;
        const PhoneNumber = response['Phone Number'];
        const MobileNumber = response['Mobile Number'];
        const Email = response.Email;
        const Web = response.Web;
        return { Name, FullAddress, Address, Description, PhoneNumber, MobileNumber, Email, Web };
      }
    } catch (error) {
      // JSON parsing failed, handle the error as needed
      throw new Error('Unable to parse the response as JSON');
    }
    throw new Error('Unable to extract data from the response');
  }



  drop(event: CdkDragDrop<string[]>) {
    console.log(this.storeTags, event.previousIndex, event.currentIndex);
    moveItemInArray(this.storeTags, event.previousIndex, event.currentIndex);
  }


  createStoreTaskMap(): void {
    this.createOrEditStoreTaskMapModal.storeId = this.storeId;
    this.createOrEditStoreTaskMapModal.show();
  }

  getStoreTaskMaps() {
    window.location.reload();
  }

  createStoreNote(): void {
    this.createOrEditStoreNoteModal.storeId = this.storeId;
    this.createOrEditStoreNoteModal.show();
  }
  getAllStoreNotes() {
    window.location.reload();
  }



  onCalenderView() {
    this.showCalendarView = !this.showCalendarView;
    this.showListView = !this.showListView;
  }

  onListView() {
    this.showListView = !this.showListView;
    this.showCalendarView = !this.showCalendarView;
  }
  onConnectClick(id: number) {
    this.oneToOneConnectModal.storeId = id;
    this.oneToOneConnectModal.show();
  }

  createStoreBusinessHour(): void {
    this.createOrEditStoreBusinessHourModal.storeId = this.storeId;
    if (this.storeHour.storeBusinessHour) {
      this.createOrEditStoreBusinessHourModal.show(this.storeHour.storeBusinessHour.id);
    } else {
      this.createOrEditStoreBusinessHourModal.show();
    }
  }
  getStoreHour(storeId: number) {
    this._storeHoursServiceProxy.getStoreHourByStore(storeId).subscribe(result => {
      this.storeHour = result;
    });
  }
  isOpen24Hours() {
    this.storeHour.storeBusinessHour.isOpen24Hours = !this.storeHour.storeBusinessHour.isOpen24Hours;
    this._storeHoursServiceProxy.createOrEdit(this.storeHour.storeBusinessHour).subscribe(result => { this.getStoreHour(this.storeId) })
  }
  isAcceptInBusinessHours() {
    this.storeHour.storeBusinessHour.isAcceptOnlyBusinessHour = !this.storeHour.storeBusinessHour.isAcceptOnlyBusinessHour;
    this._storeHoursServiceProxy.createOrEdit(this.storeHour.storeBusinessHour).subscribe(result => { this.getStoreHour(this.storeId) })
  }


  createStoreAccountTeam(): void {
    this.createOrEditStoreAccountTeamModal.storeId = this.storeId;
    this.createOrEditStoreAccountTeamModal.show();
  }
  getAllStoreAccountTeams() {
    this._storeAccountTeamsServiceProxy.getAllByStoreId(this.storeId).subscribe(result => {
      this.totalRecordsCount = result.totalCount;
      this.allEmployee = result.items;
    });
  }
  deleteStoreAccountTeam(storeAccountTeam: StoreAccountTeamDto): void {
    this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
      if (isConfirmed) {
        this._storeAccountTeamsServiceProxy.delete(storeAccountTeam.id).subscribe(() => {
          this.getAllStoreAccountTeams();
          this.notify.success(this.l('SuccessfullyDeleted'));
        });
      }
    });
  }

  //store zip codes
  getALllStoreZipCodes(storeId: number) {
    this._storeZipCodeMapsServiceProxy.getAllByStoreId(storeId).subscribe(result => {
      this.allStoreZipcodes = result.items;
    })
  }

  onStoreZipCodeSave() {
    this.storeZipCodeMap.storeId = this.storeId;
    this._storeZipCodeMapsServiceProxy.createOrEdit(this.storeZipCodeMap)
      .pipe(finalize(() => { this.saving = false; }))
      .subscribe(() => {
        this.getALllStoreZipCodes(this.storeId);
        this.storeZipCodeMap = new CreateOrEditStoreZipCodeMapDto();
        this.notify.info(this.l('SavedSuccessfully'));
      });
  }

  deleteStoreZipCodeMap(storeZipCodeMap: StoreZipCodeMapDto): void {
    this._storeZipCodeMapsServiceProxy.delete(storeZipCodeMap.id)
      .subscribe(() => {
        this.getALllStoreZipCodes(this.storeId);
        this.notify.success(this.l('SuccessfullyDeleted'));
      });
  }

  //store location
  createStoreLocation() {
    if (this.storeId) {
      this.createOrEditStoreLocationModal.storeId = this.storeId;
      this.createOrEditStoreLocationModal.show();
    }
  }
  getStoreLocations(storeId: number) {
    this._storeLocationServiceProxy.getAllByStoreId(storeId).subscribe(result => {
      this.storeLocations = result.items;
    })
  }

  deleteStoreLocation(storeLocation: StoreLocationDto): void {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._storeLocationServiceProxy.delete(storeLocation.id)
            .subscribe(() => {
              this.getStoreLocations(this.storeId);
              this.notify.success(this.l('SuccessfullyDeleted'));
            });
        }
      }
    );
  }

}
