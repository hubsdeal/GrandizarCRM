import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoresServiceProxy, StoreDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreModalComponent } from './create-or-edit-store-modal.component';

import { ViewStoreModalComponent } from './view-store-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './stores.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoresComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreModal', { static: true }) createOrEditStoreModal: CreateOrEditStoreModalComponent;
    @ViewChild('viewStoreModal', { static: true }) viewStoreModal: ViewStoreModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

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
    }

    getStores(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storesServiceProxy
            .getAll(
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
                this.masterTagNameFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
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
}
