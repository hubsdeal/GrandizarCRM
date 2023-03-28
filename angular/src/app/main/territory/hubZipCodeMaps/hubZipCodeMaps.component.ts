import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HubZipCodeMapsServiceProxy, HubZipCodeMapDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditHubZipCodeMapModalComponent } from './create-or-edit-hubZipCodeMap-modal.component';

import { ViewHubZipCodeMapModalComponent } from './view-hubZipCodeMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './hubZipCodeMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class HubZipCodeMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditHubZipCodeMapModal', { static: true })
    createOrEditHubZipCodeMapModal: CreateOrEditHubZipCodeMapModalComponent;
    @ViewChild('viewHubZipCodeMapModal', { static: true }) viewHubZipCodeMapModal: ViewHubZipCodeMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    cityNameFilter = '';
    zipCodeFilter = '';
    hubNameFilter = '';
    cityNameFilter = '';
    zipCodeNameFilter = '';

    constructor(
        injector: Injector,
        private _hubZipCodeMapsServiceProxy: HubZipCodeMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getHubZipCodeMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._hubZipCodeMapsServiceProxy
            .getAll(
                this.filterText,
                this.cityNameFilter,
                this.zipCodeFilter,
                this.hubNameFilter,
                this.cityNameFilter,
                this.zipCodeNameFilter,
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

    createHubZipCodeMap(): void {
        this.createOrEditHubZipCodeMapModal.show();
    }

    deleteHubZipCodeMap(hubZipCodeMap: HubZipCodeMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._hubZipCodeMapsServiceProxy.delete(hubZipCodeMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._hubZipCodeMapsServiceProxy
            .getHubZipCodeMapsToExcel(
                this.filterText,
                this.cityNameFilter,
                this.zipCodeFilter,
                this.hubNameFilter,
                this.cityNameFilter,
                this.zipCodeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.cityNameFilter = '';
        this.zipCodeFilter = '';
        this.hubNameFilter = '';
        this.cityNameFilter = '';
        this.zipCodeNameFilter = '';

        this.getHubZipCodeMaps();
    }
}
