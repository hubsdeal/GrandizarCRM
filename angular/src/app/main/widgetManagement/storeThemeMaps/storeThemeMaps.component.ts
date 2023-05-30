import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreThemeMapsServiceProxy, StoreThemeMapDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreThemeMapModalComponent } from './create-or-edit-storeThemeMap-modal.component';

import { ViewStoreThemeMapModalComponent } from './view-storeThemeMap-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeThemeMaps.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreThemeMapsComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreThemeMapModal', { static: true })
    createOrEditStoreThemeMapModal: CreateOrEditStoreThemeMapModalComponent;
    @ViewChild('viewStoreThemeMapModal', { static: true }) viewStoreThemeMapModal: ViewStoreThemeMapModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    activeFilter = -1;
    storeMasterThemeNameFilter = '';
    storeNameFilter = '';

    constructor(
        injector: Injector,
        private _storeThemeMapsServiceProxy: StoreThemeMapsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreThemeMaps(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeThemeMapsServiceProxy
            .getAll(
                this.filterText,
                this.activeFilter,
                this.storeMasterThemeNameFilter,
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

    createStoreThemeMap(): void {
        this.createOrEditStoreThemeMapModal.show();
    }

    deleteStoreThemeMap(storeThemeMap: StoreThemeMapDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeThemeMapsServiceProxy.delete(storeThemeMap.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeThemeMapsServiceProxy
            .getStoreThemeMapsToExcel(
                this.filterText,
                this.activeFilter,
                this.storeMasterThemeNameFilter,
                this.storeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.activeFilter = -1;
        this.storeMasterThemeNameFilter = '';
        this.storeNameFilter = '';

        this.getStoreThemeMaps();
    }
}
