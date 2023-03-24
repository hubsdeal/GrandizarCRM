import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreAccountTeamsServiceProxy, StoreAccountTeamDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreAccountTeamModalComponent } from './create-or-edit-storeAccountTeam-modal.component';

import { ViewStoreAccountTeamModalComponent } from './view-storeAccountTeam-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeAccountTeams.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreAccountTeamsComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreAccountTeamModal', { static: true })
    createOrEditStoreAccountTeamModal: CreateOrEditStoreAccountTeamModalComponent;
    @ViewChild('viewStoreAccountTeamModal', { static: true })
    viewStoreAccountTeamModal: ViewStoreAccountTeamModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    primaryFilter = -1;
    activeFilter = -1;
    orderEmailNotificationFilter = -1;
    orderSmsNotificationFilter = -1;
    storeNameFilter = '';
    employeeNameFilter = '';

    constructor(
        injector: Injector,
        private _storeAccountTeamsServiceProxy: StoreAccountTeamsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreAccountTeams(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeAccountTeamsServiceProxy
            .getAll(
                this.filterText,
                this.primaryFilter,
                this.activeFilter,
                this.orderEmailNotificationFilter,
                this.orderSmsNotificationFilter,
                this.storeNameFilter,
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

    createStoreAccountTeam(): void {
        this.createOrEditStoreAccountTeamModal.show();
    }

    deleteStoreAccountTeam(storeAccountTeam: StoreAccountTeamDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeAccountTeamsServiceProxy.delete(storeAccountTeam.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeAccountTeamsServiceProxy
            .getStoreAccountTeamsToExcel(
                this.filterText,
                this.primaryFilter,
                this.activeFilter,
                this.orderEmailNotificationFilter,
                this.orderSmsNotificationFilter,
                this.storeNameFilter,
                this.employeeNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.primaryFilter = -1;
        this.activeFilter = -1;
        this.orderEmailNotificationFilter = -1;
        this.orderSmsNotificationFilter = -1;
        this.storeNameFilter = '';
        this.employeeNameFilter = '';

        this.getStoreAccountTeams();
    }
}
