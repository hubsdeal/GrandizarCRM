import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { StoreProductServiceLocalityMapsServiceProxy, StoreProductServiceLocalityMapDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreProductServiceLocalityMapModalComponent } from './create-or-edit-storeProductServiceLocalityMap-modal.component';

import { ViewStoreProductServiceLocalityMapModalComponent } from './view-storeProductServiceLocalityMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeProductServiceLocalityMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class StoreProductServiceLocalityMapsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditStoreProductServiceLocalityMapModal', { static: true }) createOrEditStoreProductServiceLocalityMapModal: CreateOrEditStoreProductServiceLocalityMapModalComponent;
    @ViewChild('viewStoreProductServiceLocalityMapModal', { static: true }) viewStoreProductServiceLocalityMapModal: ViewStoreProductServiceLocalityMapModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    localityNameFilter = '';
    zipCodeFilter = '';
    maxLatitudeFilter : number;
		maxLatitudeFilterEmpty : number;
		minLatitudeFilter : number;
		minLatitudeFilterEmpty : number;
    maxLongitudeFilter : number;
		maxLongitudeFilterEmpty : number;
		minLongitudeFilter : number;
		minLongitudeFilterEmpty : number;
        productNameFilter = '';
        storeNameFilter = '';
        zipCodeNameFilter = '';
        cityNameFilter = '';
        stateNameFilter = '';
        countryNameFilter = '';
        hubNameFilter = '';






    constructor(
        injector: Injector,
        private _storeProductServiceLocalityMapsServiceProxy: StoreProductServiceLocalityMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreProductServiceLocalityMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeProductServiceLocalityMapsServiceProxy.getAll(
            this.filterText,
            this.localityNameFilter,
            this.zipCodeFilter,
            this.maxLatitudeFilter == null ? this.maxLatitudeFilterEmpty: this.maxLatitudeFilter,
            this.minLatitudeFilter == null ? this.minLatitudeFilterEmpty: this.minLatitudeFilter,
            this.maxLongitudeFilter == null ? this.maxLongitudeFilterEmpty: this.maxLongitudeFilter,
            this.minLongitudeFilter == null ? this.minLongitudeFilterEmpty: this.minLongitudeFilter,
            this.productNameFilter,
            this.storeNameFilter,
            this.zipCodeNameFilter,
            this.cityNameFilter,
            this.stateNameFilter,
            this.countryNameFilter,
            this.hubNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createStoreProductServiceLocalityMap(): void {
        this.createOrEditStoreProductServiceLocalityMapModal.show();        
    }


    deleteStoreProductServiceLocalityMap(storeProductServiceLocalityMap: StoreProductServiceLocalityMapDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._storeProductServiceLocalityMapsServiceProxy.delete(storeProductServiceLocalityMap.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._storeProductServiceLocalityMapsServiceProxy.getStoreProductServiceLocalityMapsToExcel(
        this.filterText,
            this.localityNameFilter,
            this.zipCodeFilter,
            this.maxLatitudeFilter == null ? this.maxLatitudeFilterEmpty: this.maxLatitudeFilter,
            this.minLatitudeFilter == null ? this.minLatitudeFilterEmpty: this.minLatitudeFilter,
            this.maxLongitudeFilter == null ? this.maxLongitudeFilterEmpty: this.maxLongitudeFilter,
            this.minLongitudeFilter == null ? this.minLongitudeFilterEmpty: this.minLongitudeFilter,
            this.productNameFilter,
            this.storeNameFilter,
            this.zipCodeNameFilter,
            this.cityNameFilter,
            this.stateNameFilter,
            this.countryNameFilter,
            this.hubNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.localityNameFilter = '';
    this.zipCodeFilter = '';
    this.maxLatitudeFilter = this.maxLatitudeFilterEmpty;
		this.minLatitudeFilter = this.maxLatitudeFilterEmpty;
    this.maxLongitudeFilter = this.maxLongitudeFilterEmpty;
		this.minLongitudeFilter = this.maxLongitudeFilterEmpty;
		this.productNameFilter = '';
							this.storeNameFilter = '';
							this.zipCodeNameFilter = '';
							this.cityNameFilter = '';
							this.stateNameFilter = '';
							this.countryNameFilter = '';
							this.hubNameFilter = '';
					
        this.getStoreProductServiceLocalityMaps();
    }
}
