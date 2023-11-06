import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductAccountTeamsServiceProxy, ProductAccountTeamDto, GetProductAccountTeamForViewDto } from '@shared/service-proxies/service-proxies';
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
export class ProductAccountTeamsComponent extends AppComponentBase implements OnInit {
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

    @Input() productId: number;

    totalRecordsCount: number;
    allEmployee: GetProductAccountTeamForViewDto[] = [];

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

    ngOnInit(): void {
        this.getAllProductAccountTeams(this.productId);
    }


    getAllProductAccountTeams(id:number) {
        this._productAccountTeamsServiceProxy.getAllByProductId(this.productId).subscribe(result => {
            this.totalRecordsCount = result.totalCount;
            this.allEmployee = result.items;
        });
    }

    reloadPage(): void {
        this.getAllProductAccountTeams(this.productId)
    }

    createProductAccountTeam(): void {
        this.createOrEditProductAccountTeamModal.productId = this.productId;
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

    }
}
