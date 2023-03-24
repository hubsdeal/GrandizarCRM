import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreSalesAlertsServiceProxy, StoreSalesAlertDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreSalesAlertModalComponent } from './create-or-edit-storeSalesAlert-modal.component';

import { ViewStoreSalesAlertModalComponent } from './view-storeSalesAlert-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeSalesAlerts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreSalesAlertsComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreSalesAlertModal', { static: true })
    createOrEditStoreSalesAlertModal: CreateOrEditStoreSalesAlertModalComponent;
    @ViewChild('viewStoreSalesAlertModal', { static: true })
    viewStoreSalesAlertModal: ViewStoreSalesAlertModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    messageFilter = '';
    currentFilter = -1;
    maxStartDateFilter: DateTime;
    minStartDateFilter: DateTime;
    maxEndDateFilter: DateTime;
    minEndDateFilter: DateTime;
    storeNameFilter = '';

    constructor(
        injector: Injector,
        private _storeSalesAlertsServiceProxy: StoreSalesAlertsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreSalesAlerts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeSalesAlertsServiceProxy
            .getAll(
                this.filterText,
                this.messageFilter,
                this.currentFilter,
                this.maxStartDateFilter === undefined
                    ? this.maxStartDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
                this.minStartDateFilter === undefined
                    ? this.minStartDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
                this.maxEndDateFilter === undefined
                    ? this.maxEndDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
                this.minEndDateFilter === undefined
                    ? this.minEndDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
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

    createStoreSalesAlert(): void {
        this.createOrEditStoreSalesAlertModal.show();
    }

    deleteStoreSalesAlert(storeSalesAlert: StoreSalesAlertDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeSalesAlertsServiceProxy.delete(storeSalesAlert.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeSalesAlertsServiceProxy
            .getStoreSalesAlertsToExcel(
                this.filterText,
                this.messageFilter,
                this.currentFilter,
                this.maxStartDateFilter === undefined
                    ? this.maxStartDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
                this.minStartDateFilter === undefined
                    ? this.minStartDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
                this.maxEndDateFilter === undefined
                    ? this.maxEndDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
                this.minEndDateFilter === undefined
                    ? this.minEndDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
                this.storeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.messageFilter = '';
        this.currentFilter = -1;
        this.maxStartDateFilter = undefined;
        this.minStartDateFilter = undefined;
        this.maxEndDateFilter = undefined;
        this.minEndDateFilter = undefined;
        this.storeNameFilter = '';

        this.getStoreSalesAlerts();
    }
}
