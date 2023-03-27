import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LeadPipelineStatusesServiceProxy, LeadPipelineStatusDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditLeadPipelineStatusModalComponent } from './create-or-edit-leadPipelineStatus-modal.component';

import { ViewLeadPipelineStatusModalComponent } from './view-leadPipelineStatus-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './leadPipelineStatuses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class LeadPipelineStatusesComponent extends AppComponentBase {
    @ViewChild('createOrEditLeadPipelineStatusModal', { static: true })
    createOrEditLeadPipelineStatusModal: CreateOrEditLeadPipelineStatusModalComponent;
    @ViewChild('viewLeadPipelineStatusModal', { static: true })
    viewLeadPipelineStatusModal: ViewLeadPipelineStatusModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxEntryDateFilter: DateTime;
    minEntryDateFilter: DateTime;
    exitDateFilter = '';
    maxEnteredAtFilter: DateTime;
    minEnteredAtFilter: DateTime;
    leadTitleFilter = '';
    leadPipelineStageNameFilter = '';
    employeeNameFilter = '';

    constructor(
        injector: Injector,
        private _leadPipelineStatusesServiceProxy: LeadPipelineStatusesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getLeadPipelineStatuses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._leadPipelineStatusesServiceProxy
            .getAll(
                this.filterText,
                this.maxEntryDateFilter === undefined
                    ? this.maxEntryDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEntryDateFilter),
                this.minEntryDateFilter === undefined
                    ? this.minEntryDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEntryDateFilter),
                this.exitDateFilter,
                this.maxEnteredAtFilter === undefined
                    ? this.maxEnteredAtFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEnteredAtFilter),
                this.minEnteredAtFilter === undefined
                    ? this.minEnteredAtFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEnteredAtFilter),
                this.leadTitleFilter,
                this.leadPipelineStageNameFilter,
                this.employeeNameFilter,
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

    createLeadPipelineStatus(): void {
        this.createOrEditLeadPipelineStatusModal.show();
    }

    deleteLeadPipelineStatus(leadPipelineStatus: LeadPipelineStatusDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._leadPipelineStatusesServiceProxy.delete(leadPipelineStatus.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._leadPipelineStatusesServiceProxy
            .getLeadPipelineStatusesToExcel(
                this.filterText,
                this.maxEntryDateFilter === undefined
                    ? this.maxEntryDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEntryDateFilter),
                this.minEntryDateFilter === undefined
                    ? this.minEntryDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEntryDateFilter),
                this.exitDateFilter,
                this.maxEnteredAtFilter === undefined
                    ? this.maxEnteredAtFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxEnteredAtFilter),
                this.minEnteredAtFilter === undefined
                    ? this.minEnteredAtFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minEnteredAtFilter),
                this.leadTitleFilter,
                this.leadPipelineStageNameFilter,
                this.employeeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxEntryDateFilter = undefined;
        this.minEntryDateFilter = undefined;
        this.exitDateFilter = '';
        this.maxEnteredAtFilter = undefined;
        this.minEnteredAtFilter = undefined;
        this.leadTitleFilter = '';
        this.leadPipelineStageNameFilter = '';
        this.employeeNameFilter = '';

        this.getLeadPipelineStatuses();
    }
}
