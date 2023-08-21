import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HubAccountTeamsServiceProxy, HubAccountTeamDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditHubAccountTeamModalComponent } from './create-or-edit-hubAccountTeam-modal.component';

import { ViewHubAccountTeamModalComponent } from './view-hubAccountTeam-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'app-hubAccountTeams',
    templateUrl: './hubAccountTeams.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class HubAccountTeamsComponent extends AppComponentBase implements OnInit{
    @ViewChild('createOrEditHubAccountTeamModal', { static: true })
    createOrEditHubAccountTeamModal: CreateOrEditHubAccountTeamModalComponent;
    @ViewChild('viewHubAccountTeamModal', { static: true }) viewHubAccountTeamModal: ViewHubAccountTeamModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    primaryManagerFilter = -1;
    maxStartDateFilter: DateTime;
    minStartDateFilter: DateTime;
    maxEndDateFilter: DateTime;
    minEndDateFilter: DateTime;
    hubNameFilter = '';
    employeeNameFilter = '';
    userNameFilter = '';

    @Input() hubId:number;
    constructor(
        injector: Injector,
        private _hubAccountTeamsServiceProxy: HubAccountTeamsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void{
        this.getHubAccountTeams();
    }


    getHubAccountTeams(event?: LazyLoadEvent) {
        // if (this.primengTableHelper.shouldResetPaging(event)) {
        //     this.paginator.changePage(0);
        //     if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
        //         return;
        //     }
        // }

        this.primengTableHelper.showLoadingIndicator();

        // this._hubAccountTeamsServiceProxy
        //     .getAll(
        //         this.filterText,
        //         this.primaryManagerFilter,
        //         this.maxStartDateFilter === undefined
        //             ? this.maxStartDateFilter
        //             : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
        //         this.minStartDateFilter === undefined
        //             ? this.minStartDateFilter
        //             : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
        //         this.maxEndDateFilter === undefined
        //             ? this.maxEndDateFilter
        //             : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
        //         this.minEndDateFilter === undefined
        //             ? this.minEndDateFilter
        //             : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
        //         this.hubNameFilter,
        //         this.employeeNameFilter,
        //         this.userNameFilter,
        //         this.primengTableHelper.getSorting(this.dataTable),
        //         this.primengTableHelper.getSkipCount(this.paginator, event),
        //         this.primengTableHelper.getMaxResultCount(this.paginator, event)
        //     )
        //     .subscribe((result) => {
        //         this.primengTableHelper.totalRecordsCount = result.totalCount;
        //         this.primengTableHelper.records = result.items;
        //         this.primengTableHelper.hideLoadingIndicator();
        //     });
        this._hubAccountTeamsServiceProxy.getAllByHubId(
            this.hubId
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this.getHubAccountTeams();
    }

    createHubAccountTeam(): void {
        this.createOrEditHubAccountTeamModal.hubId = this.hubId;
        this.createOrEditHubAccountTeamModal.show();
    }

    deleteHubAccountTeam(hubAccountTeam: HubAccountTeamDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._hubAccountTeamsServiceProxy.delete(hubAccountTeam.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._hubAccountTeamsServiceProxy
            .getHubAccountTeamsToExcel(
                this.filterText,
                this.primaryManagerFilter,
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
                this.hubNameFilter,
                this.employeeNameFilter,
                this.userNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.primaryManagerFilter = -1;
        this.maxStartDateFilter = undefined;
        this.minStartDateFilter = undefined;
        this.maxEndDateFilter = undefined;
        this.minEndDateFilter = undefined;
        this.hubNameFilter = '';
        this.employeeNameFilter = '';
        this.userNameFilter = '';

        this.getHubAccountTeams();
    }
}
