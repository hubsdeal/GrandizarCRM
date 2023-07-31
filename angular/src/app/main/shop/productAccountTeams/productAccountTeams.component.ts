import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductAccountTeamsServiceProxy, ProductAccountTeamDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductAccountTeamModalComponent } from './create-or-edit-productAccountTeam-modal.component';

import { ViewProductAccountTeamModalComponent } from './view-productAccountTeam-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'app-productAccountTeams',
    templateUrl: './productAccountTeams.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ProductAccountTeamsComponent extends AppComponentBase {
    @ViewChild('createOrEditProductAccountTeamModal', { static: true })
    createOrEditProductAccountTeamModal: CreateOrEditProductAccountTeamModalComponent;
    @ViewChild('viewProductAccountTeamModal', { static: true })
    viewProductAccountTeamModal: ViewProductAccountTeamModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    primaryFilter = -1;
    activeFilter = -1;
    maxRemoveDateFilter: DateTime;
    minRemoveDateFilter: DateTime;
    maxAssignDateFilter: DateTime;
    minAssignDateFilter: DateTime;
    employeeNameFilter = '';
    productNameFilter = '';

    constructor(
        injector: Injector,
        private _productAccountTeamsServiceProxy: ProductAccountTeamsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getProductAccountTeams(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productAccountTeamsServiceProxy
            .getAll(
                this.filterText,
                this.primaryFilter,
                this.activeFilter,
                this.maxRemoveDateFilter === undefined
                    ? this.maxRemoveDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxRemoveDateFilter),
                this.minRemoveDateFilter === undefined
                    ? this.minRemoveDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minRemoveDateFilter),
                this.maxAssignDateFilter === undefined
                    ? this.maxAssignDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxAssignDateFilter),
                this.minAssignDateFilter === undefined
                    ? this.minAssignDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minAssignDateFilter),
                this.employeeNameFilter,
                this.productNameFilter,
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

    createProductAccountTeam(): void {
        this.createOrEditProductAccountTeamModal.show();
    }

    deleteProductAccountTeam(productAccountTeam: ProductAccountTeamDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productAccountTeamsServiceProxy.delete(productAccountTeam.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productAccountTeamsServiceProxy
            .getProductAccountTeamsToExcel(
                this.filterText,
                this.primaryFilter,
                this.activeFilter,
                this.maxRemoveDateFilter === undefined
                    ? this.maxRemoveDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxRemoveDateFilter),
                this.minRemoveDateFilter === undefined
                    ? this.minRemoveDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minRemoveDateFilter),
                this.maxAssignDateFilter === undefined
                    ? this.maxAssignDateFilter
                    : this._dateTimeService.getEndOfDayForDate(this.maxAssignDateFilter),
                this.minAssignDateFilter === undefined
                    ? this.minAssignDateFilter
                    : this._dateTimeService.getStartOfDayForDate(this.minAssignDateFilter),
                this.employeeNameFilter,
                this.productNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.primaryFilter = -1;
        this.activeFilter = -1;
        this.maxRemoveDateFilter = undefined;
        this.minRemoveDateFilter = undefined;
        this.maxAssignDateFilter = undefined;
        this.minAssignDateFilter = undefined;
        this.employeeNameFilter = '';
        this.productNameFilter = '';

        this.getProductAccountTeams();
    }
}
