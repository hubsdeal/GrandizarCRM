import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { CustomerLocalitiesZipCodeMapsServiceProxy, CustomerLocalitiesZipCodeMapDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditCustomerLocalitiesZipCodeMapModalComponent } from './create-or-edit-customerLocalitiesZipCodeMap-modal.component';

import { ViewCustomerLocalitiesZipCodeMapModalComponent } from './view-customerLocalitiesZipCodeMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './customerLocalitiesZipCodeMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class CustomerLocalitiesZipCodeMapsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditCustomerLocalitiesZipCodeMapModal', { static: true }) createOrEditCustomerLocalitiesZipCodeMapModal: CreateOrEditCustomerLocalitiesZipCodeMapModalComponent;
    @ViewChild('viewCustomerLocalitiesZipCodeMapModal', { static: true }) viewCustomerLocalitiesZipCodeMapModal: ViewCustomerLocalitiesZipCodeMapModalComponent;   
    
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
        contactFullNameFilter = '';
        zipCodeNameFilter = '';
        cityNameFilter = '';
        stateNameFilter = '';
        countryNameFilter = '';
        hubNameFilter = '';






    constructor(
        injector: Injector,
        private _customerLocalitiesZipCodeMapsServiceProxy: CustomerLocalitiesZipCodeMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getCustomerLocalitiesZipCodeMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._customerLocalitiesZipCodeMapsServiceProxy.getAll(
            this.filterText,
            this.localityNameFilter,
            this.zipCodeFilter,
            this.maxLatitudeFilter == null ? this.maxLatitudeFilterEmpty: this.maxLatitudeFilter,
            this.minLatitudeFilter == null ? this.minLatitudeFilterEmpty: this.minLatitudeFilter,
            this.maxLongitudeFilter == null ? this.maxLongitudeFilterEmpty: this.maxLongitudeFilter,
            this.minLongitudeFilter == null ? this.minLongitudeFilterEmpty: this.minLongitudeFilter,
            this.contactFullNameFilter,
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

    createCustomerLocalitiesZipCodeMap(): void {
        this.createOrEditCustomerLocalitiesZipCodeMapModal.show();        
    }


    deleteCustomerLocalitiesZipCodeMap(customerLocalitiesZipCodeMap: CustomerLocalitiesZipCodeMapDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._customerLocalitiesZipCodeMapsServiceProxy.delete(customerLocalitiesZipCodeMap.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._customerLocalitiesZipCodeMapsServiceProxy.getCustomerLocalitiesZipCodeMapsToExcel(
        this.filterText,
            this.localityNameFilter,
            this.zipCodeFilter,
            this.maxLatitudeFilter == null ? this.maxLatitudeFilterEmpty: this.maxLatitudeFilter,
            this.minLatitudeFilter == null ? this.minLatitudeFilterEmpty: this.minLatitudeFilter,
            this.maxLongitudeFilter == null ? this.maxLongitudeFilterEmpty: this.maxLongitudeFilter,
            this.minLongitudeFilter == null ? this.minLongitudeFilterEmpty: this.minLongitudeFilter,
            this.contactFullNameFilter,
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
		this.contactFullNameFilter = '';
							this.zipCodeNameFilter = '';
							this.cityNameFilter = '';
							this.stateNameFilter = '';
							this.countryNameFilter = '';
							this.hubNameFilter = '';
					
        this.getCustomerLocalitiesZipCodeMaps();
    }
}
