import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LeadSalesTeamsServiceProxy, LeadSalesTeamDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditLeadSalesTeamModalComponent } from './create-or-edit-leadSalesTeam-modal.component';

import { ViewLeadSalesTeamModalComponent } from './view-leadSalesTeam-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './leadSalesTeams.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class LeadSalesTeamsComponent extends AppComponentBase {
    @ViewChild('createOrEditLeadSalesTeamModal', { static: true })
    createOrEditLeadSalesTeamModal: CreateOrEditLeadSalesTeamModalComponent;
    @ViewChild('viewLeadSalesTeamModal', { static: true }) viewLeadSalesTeamModal: ViewLeadSalesTeamModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    primaryFilter = -1;
    maxAssignedDateFilter: DateTime;
    minAssignedDateFilter: DateTime;
    leadFirstNameFilter = '';
    employeeNameFilter = '';

    constructor(
        injector: Injector,
        private _leadSalesTeamsServiceProxy: LeadSalesTeamsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getLeadSalesTeams(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._leadSalesTeamsServiceProxy
            .getAll(
                this.filterText,
                this.primaryFilter,
                this.maxAssignedDateFilter === undefined
                    ? this.maxAssignedDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxAssignedDateFilter),
                this.minAssignedDateFilter === undefined
                    ? this.minAssignedDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minAssignedDateFilter),
                this.leadFirstNameFilter,
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

    createLeadSalesTeam(): void {
        this.createOrEditLeadSalesTeamModal.show();
    }

    deleteLeadSalesTeam(leadSalesTeam: LeadSalesTeamDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._leadSalesTeamsServiceProxy.delete(leadSalesTeam.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._leadSalesTeamsServiceProxy
            .getLeadSalesTeamsToExcel(
                this.filterText,
                this.primaryFilter,
                this.maxAssignedDateFilter === undefined
                    ? this.maxAssignedDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxAssignedDateFilter),
                this.minAssignedDateFilter === undefined
                    ? this.minAssignedDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minAssignedDateFilter),
                this.leadFirstNameFilter,
                this.employeeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.primaryFilter = -1;
        this.maxAssignedDateFilter = undefined;
        this.minAssignedDateFilter = undefined;
        this.leadFirstNameFilter = '';
        this.employeeNameFilter = '';

        this.getLeadSalesTeams();
    }
}
