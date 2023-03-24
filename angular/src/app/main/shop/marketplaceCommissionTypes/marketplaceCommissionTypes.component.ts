import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    MarketplaceCommissionTypesServiceProxy,
    MarketplaceCommissionTypeDto,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditMarketplaceCommissionTypeModalComponent } from './create-or-edit-marketplaceCommissionType-modal.component';

import { ViewMarketplaceCommissionTypeModalComponent } from './view-marketplaceCommissionType-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './marketplaceCommissionTypes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class MarketplaceCommissionTypesComponent extends AppComponentBase {
    @ViewChild('createOrEditMarketplaceCommissionTypeModal', { static: true })
    createOrEditMarketplaceCommissionTypeModal: CreateOrEditMarketplaceCommissionTypeModalComponent;
    @ViewChild('viewMarketplaceCommissionTypeModal', { static: true })
    viewMarketplaceCommissionTypeModal: ViewMarketplaceCommissionTypeModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    maxPercentageFilter: number;
    maxPercentageFilterEmpty: number;
    minPercentageFilter: number;
    minPercentageFilterEmpty: number;
    maxFixedAmountFilter: number;
    maxFixedAmountFilterEmpty: number;
    minFixedAmountFilter: number;
    minFixedAmountFilterEmpty: number;

    constructor(
        injector: Injector,
        private _marketplaceCommissionTypesServiceProxy: MarketplaceCommissionTypesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getMarketplaceCommissionTypes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._marketplaceCommissionTypesServiceProxy
            .getAll(
                this.filterText,
                this.nameFilter,
                this.maxPercentageFilter == null ? this.maxPercentageFilterEmpty : this.maxPercentageFilter,
                this.minPercentageFilter == null ? this.minPercentageFilterEmpty : this.minPercentageFilter,
                this.maxFixedAmountFilter == null ? this.maxFixedAmountFilterEmpty : this.maxFixedAmountFilter,
                this.minFixedAmountFilter == null ? this.minFixedAmountFilterEmpty : this.minFixedAmountFilter,
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

    createMarketplaceCommissionType(): void {
        this.createOrEditMarketplaceCommissionTypeModal.show();
    }

    deleteMarketplaceCommissionType(marketplaceCommissionType: MarketplaceCommissionTypeDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._marketplaceCommissionTypesServiceProxy.delete(marketplaceCommissionType.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._marketplaceCommissionTypesServiceProxy
            .getMarketplaceCommissionTypesToExcel(
                this.filterText,
                this.nameFilter,
                this.maxPercentageFilter == null ? this.maxPercentageFilterEmpty : this.maxPercentageFilter,
                this.minPercentageFilter == null ? this.minPercentageFilterEmpty : this.minPercentageFilter,
                this.maxFixedAmountFilter == null ? this.maxFixedAmountFilterEmpty : this.maxFixedAmountFilter,
                this.minFixedAmountFilter == null ? this.minFixedAmountFilterEmpty : this.minFixedAmountFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.nameFilter = '';
        this.maxPercentageFilter = this.maxPercentageFilterEmpty;
        this.minPercentageFilter = this.maxPercentageFilterEmpty;
        this.maxFixedAmountFilter = this.maxFixedAmountFilterEmpty;
        this.minFixedAmountFilter = this.maxFixedAmountFilterEmpty;

        this.getMarketplaceCommissionTypes();
    }
}
