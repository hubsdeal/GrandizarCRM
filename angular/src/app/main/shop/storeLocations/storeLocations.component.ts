import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreLocationsServiceProxy, StoreLocationDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreLocationModalComponent } from './create-or-edit-storeLocation-modal.component';

import { ViewStoreLocationModalComponent } from './view-storeLocation-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeLocations.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreLocationsComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreLocationModal', { static: true })
    createOrEditStoreLocationModal: CreateOrEditStoreLocationModalComponent;
    @ViewChild('viewStoreLocationModal', { static: true }) viewStoreLocationModal: ViewStoreLocationModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    locationNameFilter = '';
    fullAddressFilter = '';
    maxLatitudeFilter: number;
    maxLatitudeFilterEmpty: number;
    minLatitudeFilter: number;
    minLatitudeFilterEmpty: number;
    maxLongitudeFilter: number;
    maxLongitudeFilterEmpty: number;
    minLongitudeFilter: number;
    minLongitudeFilterEmpty: number;
    addressFilter = '';
    mobileFilter = '';
    emailFilter = '';
    zipCodeFilter = '';
    cityNameFilter = '';
    stateNameFilter = '';
    countryNameFilter = '';
    storeNameFilter = '';

    constructor(
        injector: Injector,
        private _storeLocationsServiceProxy: StoreLocationsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreLocations(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeLocationsServiceProxy
            .getAll(
                this.filterText,
                this.locationNameFilter,
                this.fullAddressFilter,
                this.maxLatitudeFilter == null ? this.maxLatitudeFilterEmpty : this.maxLatitudeFilter,
                this.minLatitudeFilter == null ? this.minLatitudeFilterEmpty : this.minLatitudeFilter,
                this.maxLongitudeFilter == null ? this.maxLongitudeFilterEmpty : this.maxLongitudeFilter,
                this.minLongitudeFilter == null ? this.minLongitudeFilterEmpty : this.minLongitudeFilter,
                this.addressFilter,
                this.mobileFilter,
                this.emailFilter,
                this.zipCodeFilter,
                this.cityNameFilter,
                this.stateNameFilter,
                this.countryNameFilter,
                this.storeNameFilter,
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

    createStoreLocation(): void {
        this.createOrEditStoreLocationModal.show();
    }

    deleteStoreLocation(storeLocation: StoreLocationDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeLocationsServiceProxy.delete(storeLocation.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeLocationsServiceProxy
            .getStoreLocationsToExcel(
                this.filterText,
                this.locationNameFilter,
                this.fullAddressFilter,
                this.maxLatitudeFilter == null ? this.maxLatitudeFilterEmpty : this.maxLatitudeFilter,
                this.minLatitudeFilter == null ? this.minLatitudeFilterEmpty : this.minLatitudeFilter,
                this.maxLongitudeFilter == null ? this.maxLongitudeFilterEmpty : this.maxLongitudeFilter,
                this.minLongitudeFilter == null ? this.minLongitudeFilterEmpty : this.minLongitudeFilter,
                this.addressFilter,
                this.mobileFilter,
                this.emailFilter,
                this.zipCodeFilter,
                this.cityNameFilter,
                this.stateNameFilter,
                this.countryNameFilter,
                this.storeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.locationNameFilter = '';
        this.fullAddressFilter = '';
        this.maxLatitudeFilter = this.maxLatitudeFilterEmpty;
        this.minLatitudeFilter = this.maxLatitudeFilterEmpty;
        this.maxLongitudeFilter = this.maxLongitudeFilterEmpty;
        this.minLongitudeFilter = this.maxLongitudeFilterEmpty;
        this.addressFilter = '';
        this.mobileFilter = '';
        this.emailFilter = '';
        this.zipCodeFilter = '';
        this.cityNameFilter = '';
        this.stateNameFilter = '';
        this.countryNameFilter = '';
        this.storeNameFilter = '';

        this.getStoreLocations();
    }
}
