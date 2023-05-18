import { Component, Injector, Input, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { StoreCountryLookupTableDto, StoreStateLookupTableDto, StoresServiceProxy, TokenAuthServiceProxy, StoreDto } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { NotifyService } from 'abp-ng2-module';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { CreateOrEditStoreModalComponent } from '../create-or-edit-store-modal.component';
import { ViewStoreModalComponent } from '../view-store-modal.component';

@Component({
  selector: 'app-my-stores',
  templateUrl: './my-stores.component.html',
  styleUrls: ['./my-stores.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class MyStoresComponent extends AppComponentBase {
  @ViewChild('createOrEditStoreModal', { static: true }) createOrEditStoreModal: CreateOrEditStoreModalComponent;
  @ViewChild('viewStoreModal', { static: true }) viewStoreModal: ViewStoreModalComponent;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  @Input() isFromDashboard: boolean = false;

  advancedFiltersAreShown = false;
  filterText = '';
  nameFilter = '';
  storeUrlFilter = '';
  descriptionFilter = '';
  metaTagFilter = '';
  metaDescriptionFilter = '';
  fullAddressFilter = '';
  addressFilter = '';
  cityFilter = '';
  maxLatitudeFilter: number;
  maxLatitudeFilterEmpty: number;
  minLatitudeFilter: number;
  minLatitudeFilterEmpty: number;
  maxLongitudeFilter: number;
  maxLongitudeFilterEmpty: number;
  minLongitudeFilter: number;
  minLongitudeFilterEmpty: number;
  phoneFilter = '';
  mobileFilter = '';
  emailFilter = '';
  isPublishedFilter = -1;
  facebookFilter = '';
  instagramFilter = '';
  linkedInFilter = '';
  youtubeFilter = '';
  faxFilter = '';
  zipCodeFilter = '';
  websiteFilter = '';
  yearOfEstablishmentFilter = '';
  maxDisplaySequenceFilter: number;
  maxDisplaySequenceFilterEmpty: number;
  minDisplaySequenceFilter: number;
  minDisplaySequenceFilterEmpty: number;
  maxScoreFilter: number;
  maxScoreFilterEmpty: number;
  minScoreFilter: number;
  minScoreFilterEmpty: number;
  legalNameFilter = '';
  isLocalOrOnlineStoreFilter = -1;
  isVerifiedFilter = -1;
  mediaLibraryNameFilter = '';
  countryNameFilter = '';
  stateNameFilter = '';
  ratingLikeNameFilter = '';
  masterTagNameFilter = '';
  stateIdFilter: number;
  countryIdFilter: number;
  employeeIdFilter: number;
  isFavoriteOnly: number;
  isFromMasterList: boolean;
  contactIdFilter: number;
  masterTagCategoryIdFilter: number;
  masterTagIdFilter: number

  skipCount: number;
  maxResultCount: number = 10;

  favoriteCount: number;

  isFavorite: number;
  publishedCount: number = 0;
  unPublishedCount: number = 0;
  verifiedCount: number = 0;

  selectedAll: boolean = false;

  allCountrys: StoreCountryLookupTableDto[];
  allStates: StoreStateLookupTableDto[];
  state:any;
  country:any;

  constructor(
      injector: Injector,
      private _storesServiceProxy: StoresServiceProxy,
      private _notifyService: NotifyService,
      private _tokenAuth: TokenAuthServiceProxy,
      private _activatedRoute: ActivatedRoute,
      private _fileDownloadService: FileDownloadService,
      private _dateTimeService: DateTimeService
  ) {
      super(injector);
      this._storesServiceProxy.getAllCountryForTableDropdown().subscribe((result) => {
          this.allCountrys = result;
      });
      this._storesServiceProxy.getAllStateForTableDropdown().subscribe((result) => {
          this.allStates = result;
      });
  }

  getStores(event?: LazyLoadEvent) {
      // if (this.primengTableHelper.shouldResetPaging(event)) {
      //     this.paginator.changePage(0);
      //     if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
      //         return;
      //     }
      // }

      this.primengTableHelper.showLoadingIndicator();

      this._storesServiceProxy
          .getAllStoresBySp(
              this.filterText,
              this.nameFilter,
              this.addressFilter,
              this.cityFilter,
              this.phoneFilter,
              this.mobileFilter,
              this.emailFilter,
              this.isPublishedFilter,
              this.isLocalOrOnlineStoreFilter,
              this.isVerifiedFilter,
              this.stateIdFilter,
              this.countryIdFilter,
              this.employeeIdFilter,
              1,
              true,
              this.contactIdFilter,
              this.zipCodeFilter,
              this.masterTagCategoryIdFilter,
              this.masterTagIdFilter,
              '',
              this.skipCount,
              this.maxResultCount
              // this.primengTableHelper.getSorting(this.dataTable),
              // this.primengTableHelper.getSkipCount(this.paginator, event),
              // this.primengTableHelper.getMaxResultCount(this.paginator, event)
          )
          .subscribe((result) => {
              this.primengTableHelper.totalRecordsCount = result.totalCount;
              this.isFavorite = result.isFavoriteOnly;
              this.favoriteCount = result.favorite;
              this.publishedCount = result.published;
              this.unPublishedCount = result.unPublished;
              this.verifiedCount = result.verified;
              this.primengTableHelper.records = result.stores;
              this.primengTableHelper.hideLoadingIndicator();
          });
  }

  paginate(event: any) {
      this.skipCount = event.first;
      this.maxResultCount = event.rows;
      this.getStores();
  }

  reloadPage(): void {
      this.paginator.changePage(this.paginator.getPage());
  }

  createStore(): void {
      this.createOrEditStoreModal.show();
  }

  deleteStore(store: StoreDto): void {
      this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
          if (isConfirmed) {
              this._storesServiceProxy.delete(store.id).subscribe(() => {
                  this.reloadPage();
                  this.notify.success(this.l('SuccessfullyDeleted'));
              });
          }
      });
  }

  exportToExcel(): void {
      this._storesServiceProxy
          .getStoresToExcel(
              this.filterText,
              this.nameFilter,
              this.storeUrlFilter,
              this.descriptionFilter,
              this.metaTagFilter,
              this.metaDescriptionFilter,
              this.fullAddressFilter,
              this.addressFilter,
              this.cityFilter,
              this.maxLatitudeFilter == null ? this.maxLatitudeFilterEmpty : this.maxLatitudeFilter,
              this.minLatitudeFilter == null ? this.minLatitudeFilterEmpty : this.minLatitudeFilter,
              this.maxLongitudeFilter == null ? this.maxLongitudeFilterEmpty : this.maxLongitudeFilter,
              this.minLongitudeFilter == null ? this.minLongitudeFilterEmpty : this.minLongitudeFilter,
              this.phoneFilter,
              this.mobileFilter,
              this.emailFilter,
              this.isPublishedFilter,
              this.facebookFilter,
              this.instagramFilter,
              this.linkedInFilter,
              this.youtubeFilter,
              this.faxFilter,
              this.zipCodeFilter,
              this.websiteFilter,
              this.yearOfEstablishmentFilter,
              this.maxDisplaySequenceFilter == null
                  ? this.maxDisplaySequenceFilterEmpty
                  : this.maxDisplaySequenceFilter,
              this.minDisplaySequenceFilter == null
                  ? this.minDisplaySequenceFilterEmpty
                  : this.minDisplaySequenceFilter,
              this.maxScoreFilter == null ? this.maxScoreFilterEmpty : this.maxScoreFilter,
              this.minScoreFilter == null ? this.minScoreFilterEmpty : this.minScoreFilter,
              this.legalNameFilter,
              this.isLocalOrOnlineStoreFilter,
              this.isVerifiedFilter,
              this.mediaLibraryNameFilter,
              this.countryNameFilter,
              this.stateNameFilter,
              this.ratingLikeNameFilter,
              this.masterTagNameFilter
          )
          .subscribe((result) => {
              this._fileDownloadService.downloadTempFile(result);
          });
  }

  resetFilters(): void {
      this.filterText = '';
      this.nameFilter = '';
      this.storeUrlFilter = '';
      this.descriptionFilter = '';
      this.metaTagFilter = '';
      this.metaDescriptionFilter = '';
      this.fullAddressFilter = '';
      this.addressFilter = '';
      this.cityFilter = '';
      this.maxLatitudeFilter = this.maxLatitudeFilterEmpty;
      this.minLatitudeFilter = this.maxLatitudeFilterEmpty;
      this.maxLongitudeFilter = this.maxLongitudeFilterEmpty;
      this.minLongitudeFilter = this.maxLongitudeFilterEmpty;
      this.phoneFilter = '';
      this.mobileFilter = '';
      this.emailFilter = '';
      this.isPublishedFilter = -1;
      this.facebookFilter = '';
      this.instagramFilter = '';
      this.linkedInFilter = '';
      this.youtubeFilter = '';
      this.faxFilter = '';
      this.zipCodeFilter = '';
      this.websiteFilter = '';
      this.yearOfEstablishmentFilter = '';
      this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
      this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
      this.maxScoreFilter = this.maxScoreFilterEmpty;
      this.minScoreFilter = this.maxScoreFilterEmpty;
      this.legalNameFilter = '';
      this.isLocalOrOnlineStoreFilter = -1;
      this.isVerifiedFilter = -1;
      this.mediaLibraryNameFilter = '';
      this.countryNameFilter = '';
      this.stateNameFilter = '';
      this.ratingLikeNameFilter = '';
      this.masterTagNameFilter = '';

      this.getStores();
  }

  onPublishClick(value: number) {
      this.isPublishedFilter = value;
      this.getStores();
  }

  onVerifiedClick(value: number) {
      this.isVerifiedFilter = value;
      this.getStores();
  }

  onTotalClick() {
      this.isPublishedFilter = -1;
      this.isVerifiedFilter = -1;
      this.getStores();
  }

  onChangesSelectAll() {
      for (var i = 0; i < this.primengTableHelper.records.length; i++) {
          this.primengTableHelper.records[i].selected = this.selectedAll;
      }
  }

  checkIfAllSelected() {
      this.selectedAll = this.primengTableHelper.records.every(function (item: any) {
          return item.selected == true;
      })
  }

  onLikeClick(id: number) {
      // this.showMainSpinner();
      // this._wishListServiceProxy.delete(id).subscribe(result => {
      //     this.getStores();
      //     this.hideMainSpinner();
      // });
  }

  onDisLikeClick(id: number) {
      // this.showMainSpinner();
      // let item = new CreateOrEditWishListDto();
      // item.storeId = id;
      // item.contactId = this._appSessionService.contactId;
      // this._wishListServiceProxy.createOrEdit(item).subscribe(result => {
      //     this.getStores();
      //     this.hideMainSpinner();
      // });
  }

  onCountrySelect(event:any){
      if (event.value && event.value.id){
          this.countryIdFilter = event.value.id; 
      }
  }

  onStateSelect(event:any){
      if (event.value && event.value.id){
          this.stateIdFilter = event.value.id;
      }
  }
}
