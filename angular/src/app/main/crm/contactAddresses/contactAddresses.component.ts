import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ContactAddressesServiceProxy, ContactAddressDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditContactAddressModalComponent } from './create-or-edit-contactAddress-modal.component';

import { ViewContactAddressModalComponent } from './view-contactAddress-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contactAddresses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContactAddressesComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditContactAddressModal', { static: true }) createOrEditContactAddressModal: CreateOrEditContactAddressModalComponent;
    @ViewChild('viewContactAddressModal', { static: true }) viewContactAddressModal: ViewContactAddressModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    titleFilter = '';
    fullAddressFilter = '';
    addressFilter = '';
    zipCodeFilter = '';
    maxLatitudeFilter : number;
		maxLatitudeFilterEmpty : number;
		minLatitudeFilter : number;
		minLatitudeFilterEmpty : number;
    maxLongitudeFilter : number;
		maxLongitudeFilterEmpty : number;
		minLongitudeFilter : number;
		minLongitudeFilterEmpty : number;
    activeFilter = -1;
    cityFilter = '';
    noteForDeliveryFilter = '';
        contactFullNameFilter = '';
        countryNameFilter = '';
        stateNameFilter = '';






    constructor(
        injector: Injector,
        private _contactAddressesServiceProxy: ContactAddressesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getContactAddresses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._contactAddressesServiceProxy.getAll(
            this.filterText,
            this.titleFilter,
            this.fullAddressFilter,
            this.addressFilter,
            this.zipCodeFilter,
            this.maxLatitudeFilter == null ? this.maxLatitudeFilterEmpty: this.maxLatitudeFilter,
            this.minLatitudeFilter == null ? this.minLatitudeFilterEmpty: this.minLatitudeFilter,
            this.maxLongitudeFilter == null ? this.maxLongitudeFilterEmpty: this.maxLongitudeFilter,
            this.minLongitudeFilter == null ? this.minLongitudeFilterEmpty: this.minLongitudeFilter,
            this.activeFilter,
            this.cityFilter,
            this.noteForDeliveryFilter,
            this.contactFullNameFilter,
            this.countryNameFilter,
            this.stateNameFilter,
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

    createContactAddress(): void {
        this.createOrEditContactAddressModal.show();        
    }


    deleteContactAddress(contactAddress: ContactAddressDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._contactAddressesServiceProxy.delete(contactAddress.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._contactAddressesServiceProxy.getContactAddressesToExcel(
        this.filterText,
            this.titleFilter,
            this.fullAddressFilter,
            this.addressFilter,
            this.zipCodeFilter,
            this.maxLatitudeFilter == null ? this.maxLatitudeFilterEmpty: this.maxLatitudeFilter,
            this.minLatitudeFilter == null ? this.minLatitudeFilterEmpty: this.minLatitudeFilter,
            this.maxLongitudeFilter == null ? this.maxLongitudeFilterEmpty: this.maxLongitudeFilter,
            this.minLongitudeFilter == null ? this.minLongitudeFilterEmpty: this.minLongitudeFilter,
            this.activeFilter,
            this.cityFilter,
            this.noteForDeliveryFilter,
            this.contactFullNameFilter,
            this.countryNameFilter,
            this.stateNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.titleFilter = '';
    this.fullAddressFilter = '';
    this.addressFilter = '';
    this.zipCodeFilter = '';
    this.maxLatitudeFilter = this.maxLatitudeFilterEmpty;
		this.minLatitudeFilter = this.maxLatitudeFilterEmpty;
    this.maxLongitudeFilter = this.maxLongitudeFilterEmpty;
		this.minLongitudeFilter = this.maxLongitudeFilterEmpty;
    this.activeFilter = -1;
    this.cityFilter = '';
    this.noteForDeliveryFilter = '';
		this.contactFullNameFilter = '';
							this.countryNameFilter = '';
							this.stateNameFilter = '';
					
        this.getContactAddresses();
    }
}
