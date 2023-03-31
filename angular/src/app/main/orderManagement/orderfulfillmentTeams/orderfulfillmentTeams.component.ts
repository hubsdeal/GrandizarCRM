import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderfulfillmentTeamsServiceProxy, OrderfulfillmentTeamDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditOrderfulfillmentTeamModalComponent } from './create-or-edit-orderfulfillmentTeam-modal.component';

import { ViewOrderfulfillmentTeamModalComponent } from './view-orderfulfillmentTeam-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './orderfulfillmentTeams.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class OrderfulfillmentTeamsComponent extends AppComponentBase {
    @ViewChild('createOrEditOrderfulfillmentTeamModal', { static: true })
    createOrEditOrderfulfillmentTeamModal: CreateOrEditOrderfulfillmentTeamModalComponent;
    @ViewChild('viewOrderfulfillmentTeamModal', { static: true })
    viewOrderfulfillmentTeamModal: ViewOrderfulfillmentTeamModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    orderFullNameFilter = '';
    employeeNameFilter = '';
    contactFullNameFilter = '';
    userNameFilter = '';

    constructor(
        injector: Injector,
        private _orderfulfillmentTeamsServiceProxy: OrderfulfillmentTeamsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getOrderfulfillmentTeams(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._orderfulfillmentTeamsServiceProxy
            .getAll(
                this.filterText,
                this.orderFullNameFilter,
                this.employeeNameFilter,
                this.contactFullNameFilter,
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

    createOrderfulfillmentTeam(): void {
        this.createOrEditOrderfulfillmentTeamModal.show();
    }

    deleteOrderfulfillmentTeam(orderfulfillmentTeam: OrderfulfillmentTeamDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._orderfulfillmentTeamsServiceProxy.delete(orderfulfillmentTeam.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._orderfulfillmentTeamsServiceProxy
            .getOrderfulfillmentTeamsToExcel(
                this.filterText,
                this.orderFullNameFilter,
                this.employeeNameFilter,
                this.contactFullNameFilter,
                this.userNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.orderFullNameFilter = '';
        this.employeeNameFilter = '';
        this.contactFullNameFilter = '';
        this.userNameFilter = '';

        this.getOrderfulfillmentTeams();
    }
}
