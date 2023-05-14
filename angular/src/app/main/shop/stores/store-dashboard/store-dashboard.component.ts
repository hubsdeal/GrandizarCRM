import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditStoreDto, GetStoreMediaForViewDto, StoreMediasServiceProxy, StoresServiceProxy } from '@shared/service-proxies/service-proxies';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { SelectItem } from 'primeng/api';
import { finalize } from 'rxjs';
import { CreateOrEditStoreMediaModalComponent } from '../../storeMedias/create-or-edit-storeMedia-modal.component';

@Component({
  selector: 'app-store-dashboard',
  templateUrl: './store-dashboard.component.html',
  styleUrls: ['./store-dashboard.component.css']
})
export class StoreDashboardComponent extends AppComponentBase implements OnInit, AfterViewInit {
  @ViewChild('createOrEditStoreMediaModal', { static: true })
  createOrEditStoreMediaModal: CreateOrEditStoreMediaModalComponent;
  saving = false;
  storeId: number;
  productShortDesc: string;
  modalTile: string
  bindingData: any;
  localOrVirtualStoreOptions: SelectItem[];
  storePublishedOptions: SelectItem[];
  store: CreateOrEditStoreDto = new CreateOrEditStoreDto();

  tags: string[] = [];

  stateName: string;
  countryName: string;
  picture: string;
  teams: any[] = [];
  taxOptions: any[] = [];

  publishOptions: SelectItem[];
  publish: Boolean = false;

  numberOfRatings: number;
  ratingScore: number;


  numberofTasks: number;
  numberOfNotes: number;
  primaryCategoryName: string;

  imageSrc: any = 'assets/common/images/c_logo.png';
  public uploader: FileUploader;
  public temporaryPictureUrl: string;
  private _uploaderOptions: FileUploaderOptions = {};

  allPrimaryCategories: any[];
  stateOptions: any = [];
  countryOptions: any = [];

  isUrlAvailble: boolean = false;
  isUrlNotAvailble: boolean = false;

  images: GetStoreMediaForViewDto[] = [];
  videos: any[] = [];

  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService,
    private _storeServiceProxy: StoresServiceProxy,
    private _storeMediaServiceProxy: StoreMediasServiceProxy,
    private dialog: MatDialog
  ) {
    super(injector);
    this.loadAllDropdown();
    this.store.isLocalOrOnlineStore = true;
    this.localOrVirtualStoreOptions = [{ label: 'Local Store', value: false }, { label: 'Virtual Store', value: true }];
    this.storePublishedOptions = [{ label: 'Draft', value: false }, { label: 'Published', value: true }];
  }

  ngOnInit(): void {
    let storeId = this.route.snapshot.paramMap.get('storeId')
    this.storeId = parseInt(storeId);
    this.getStoreDetails(this.storeId);
    this.initFileUploader();
  }
  ngAfterViewInit() {

  }

  loadAllDropdown() {
    this._storeServiceProxy.getAllCountryForTableDropdown().subscribe(result => {
      this.countryOptions = result;
    });

    this._storeServiceProxy.getAllStateForTableDropdown().subscribe(result => {
      this.stateOptions = result;
    });

    // this._storeServiceProxy.getAllRatingLikeForTableDropdown().subscribe(result => {
    //   this.ratingLikeOptions = result;
    // });
    // this._storeServiceProxy.getAllTaxRateForTableDropdown().subscribe(result => {
    //   this.taxOptions = result;
    // });
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
      this.picture = result.picture;
      this.tags = result.storeTags;
      this.publish = result.store.isPublished ? true : false;
      this.numberofTasks = result.numberOfTasks;
      this.numberOfNotes = result.numberOfNotes;
      this.numberOfRatings = result.numberOfRatings;
      this.ratingScore = result.ratingScore;
      this.primaryCategoryName = result.primaryCategoryName;
      if (result.picture != null) {
        this.imageSrc = result.picture;
      }
    })
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
    this._storeMediaServiceProxy.getall(
      this.storeId
    ).subscribe(result => {
      this.images.push(...result.items.filter(x => x.picture != null && x.videoUrl == null));
      if (this.images[0] != null) {
        this.picture = this.images[0].picture;
      }
      this.videos.push(...result.items.filter(x => x.videoUrl != null));
    });
  }

}
