import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { StoreOwnerTeamsServiceProxy, StoreOwnerTeamDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStoreOwnerTeamModalComponent } from './create-or-edit-storeOwnerTeam-modal.component';

import { ViewStoreOwnerTeamModalComponent } from './view-storeOwnerTeam-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './storeOwnerTeams.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class StoreOwnerTeamsComponent extends AppComponentBase {
    @ViewChild('createOrEditStoreOwnerTeamModal', { static: true })
    createOrEditStoreOwnerTeamModal: CreateOrEditStoreOwnerTeamModalComponent;
    @ViewChild('viewStoreOwnerTeamModal', { static: true }) viewStoreOwnerTeamModal: ViewStoreOwnerTeamModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    activeFilter = -1;
    primaryFilter = -1;
    orderEmailNotificationFilter = -1;
    orderSmsNotificationFilter = -1;
    storeNameFilter = '';
    userNameFilter = '';

    constructor(
        injector: Injector,
        private _storeOwnerTeamsServiceProxy: StoreOwnerTeamsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getStoreOwnerTeams(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._storeOwnerTeamsServiceProxy
            .getAll(
                this.filterText,
                this.activeFilter,
                this.primaryFilter,
                this.orderEmailNotificationFilter,
                this.orderSmsNotificationFilter,
                this.storeNameFilter,
                this.userNameFilter,
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

    createStoreOwnerTeam(): void {
        this.createOrEditStoreOwnerTeamModal.show();
    }

    deleteStoreOwnerTeam(storeOwnerTeam: StoreOwnerTeamDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._storeOwnerTeamsServiceProxy.delete(storeOwnerTeam.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._storeOwnerTeamsServiceProxy
            .getStoreOwnerTeamsToExcel(
                this.filterText,
                this.activeFilter,
                this.primaryFilter,
                this.orderEmailNotificationFilter,
                this.orderSmsNotificationFilter,
                this.storeNameFilter,
                this.userNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.activeFilter = -1;
        this.primaryFilter = -1;
        this.orderEmailNotificationFilter = -1;
        this.orderSmsNotificationFilter = -1;
        this.storeNameFilter = '';
        this.userNameFilter = '';

        this.getStoreOwnerTeams();
    }
}
