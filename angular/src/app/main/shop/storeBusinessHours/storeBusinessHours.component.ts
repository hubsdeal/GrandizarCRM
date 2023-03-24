import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreBusinessHoursServiceProxy, StoreBusinessHourDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreBusinessHourModalComponent } from './create-or-edit-storeBusinessHour-modal.component';

import { ViewStoreBusinessHourModalComponent } from './view-storeBusinessHour-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeBusinessHours.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreBusinessHoursComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreBusinessHourModal', { static: true })
    createOrEditStoreBusinessHourModal: CreateOrEditStoreBusinessHourModalComponent;
    @ViewChild('viewStoreBusinessHourModal', { static: true })
    viewStoreBusinessHourModal: ViewStoreBusinessHourModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nowOpenOrClosedFilter = -1;
    isOpen24HoursFilter = -1;
    mondayStartTimeFilter = '';
    mondayEndTimeFilter = '';
    tuesdayStartTimeFilter = '';
    tuesdayEndTimeFilter = '';
    wednesdayStartTimeFilter = '';
    wednesdayEndTimeFilter = '';
    thursdayStartTimeFilter = '';
    thursdayEndTimeFilter = '';
    fridayStartTimeFilter = '';
    fridayEndTimeFilter = '';
    saturdayStartTimeFilter = '';
    saturdayEndTimeFilter = '';
    sundayStartTimeFilter = '';
    sundayEndTimeFilter = '';
    isAcceptOnlyBusinessHourFilter = -1;
    storeNameFilter = '';
    masterTagCategoryNameFilter = '';
    masterTagNameFilter = '';

    constructor(
        injector: Injector,
        private _storeBusinessHoursServiceProxy: StoreBusinessHoursServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreBusinessHours(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeBusinessHoursServiceProxy
            .getAll(
                this.filterText,
                this.nowOpenOrClosedFilter,
                this.isOpen24HoursFilter,
                this.mondayStartTimeFilter,
                this.mondayEndTimeFilter,
                this.tuesdayStartTimeFilter,
                this.tuesdayEndTimeFilter,
                this.wednesdayStartTimeFilter,
                this.wednesdayEndTimeFilter,
                this.thursdayStartTimeFilter,
                this.thursdayEndTimeFilter,
                this.fridayStartTimeFilter,
                this.fridayEndTimeFilter,
                this.saturdayStartTimeFilter,
                this.saturdayEndTimeFilter,
                this.sundayStartTimeFilter,
                this.sundayEndTimeFilter,
                this.isAcceptOnlyBusinessHourFilter,
                this.storeNameFilter,
                this.masterTagCategoryNameFilter,
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

    createStoreBusinessHour(): void {
        this.createOrEditStoreBusinessHourModal.show();
    }

    deleteStoreBusinessHour(storeBusinessHour: StoreBusinessHourDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeBusinessHoursServiceProxy.delete(storeBusinessHour.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeBusinessHoursServiceProxy
            .getStoreBusinessHoursToExcel(
                this.filterText,
                this.nowOpenOrClosedFilter,
                this.isOpen24HoursFilter,
                this.mondayStartTimeFilter,
                this.mondayEndTimeFilter,
                this.tuesdayStartTimeFilter,
                this.tuesdayEndTimeFilter,
                this.wednesdayStartTimeFilter,
                this.wednesdayEndTimeFilter,
                this.thursdayStartTimeFilter,
                this.thursdayEndTimeFilter,
                this.fridayStartTimeFilter,
                this.fridayEndTimeFilter,
                this.saturdayStartTimeFilter,
                this.saturdayEndTimeFilter,
                this.sundayStartTimeFilter,
                this.sundayEndTimeFilter,
                this.isAcceptOnlyBusinessHourFilter,
                this.storeNameFilter,
                this.masterTagCategoryNameFilter,
                this.masterTagNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nowOpenOrClosedFilter = -1;
        this.isOpen24HoursFilter = -1;
        this.mondayStartTimeFilter = '';
        this.mondayEndTimeFilter = '';
        this.tuesdayStartTimeFilter = '';
        this.tuesdayEndTimeFilter = '';
        this.wednesdayStartTimeFilter = '';
        this.wednesdayEndTimeFilter = '';
        this.thursdayStartTimeFilter = '';
        this.thursdayEndTimeFilter = '';
        this.fridayStartTimeFilter = '';
        this.fridayEndTimeFilter = '';
        this.saturdayStartTimeFilter = '';
        this.saturdayEndTimeFilter = '';
        this.sundayStartTimeFilter = '';
        this.sundayEndTimeFilter = '';
        this.isAcceptOnlyBusinessHourFilter = -1;
        this.storeNameFilter = '';
        this.masterTagCategoryNameFilter = '';
        this.masterTagNameFilter = '';

        this.getStoreBusinessHours();
    }
}
