import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { RewardPointHistoriesServiceProxy, RewardPointHistoryDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRewardPointHistoryModalComponent } from './create-or-edit-rewardPointHistory-modal.component';

import { ViewRewardPointHistoryModalComponent } from './view-rewardPointHistory-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './rewardPointHistories.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class RewardPointHistoriesComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditRewardPointHistoryModal', { static: true }) createOrEditRewardPointHistoryModal: CreateOrEditRewardPointHistoryModalComponent;
    @ViewChild('viewRewardPointHistoryModal', { static: true }) viewRewardPointHistoryModal: ViewRewardPointHistoryModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    earnedOrRedeemedFilter = -1;
    maxDateFilter : DateTime;
		minDateFilter : DateTime;
    maxPurchaseAmountFilter : number;
		maxPurchaseAmountFilterEmpty : number;
		minPurchaseAmountFilter : number;
		minPurchaseAmountFilterEmpty : number;
    maxPointsEarnedFilter : number;
		maxPointsEarnedFilterEmpty : number;
		minPointsEarnedFilter : number;
		minPointsEarnedFilterEmpty : number;
    maxPointsBalanceFilter : number;
		maxPointsBalanceFilterEmpty : number;
		minPointsBalanceFilter : number;
		minPointsBalanceFilterEmpty : number;
    maxPointsDeductedFilter : number;
		maxPointsDeductedFilterEmpty : number;
		minPointsDeductedFilter : number;
		minPointsDeductedFilterEmpty : number;
        rewardPointTypeNameFilter = '';
        orderInvoiceNumberFilter = '';
        contactFullNameFilter = '';
        jobTitleFilter = '';






    constructor(
        injector: Injector,
        private _rewardPointHistoriesServiceProxy: RewardPointHistoriesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getRewardPointHistories(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._rewardPointHistoriesServiceProxy.getAll(
            this.filterText,
            this.earnedOrRedeemedFilter,
            this.maxDateFilter === undefined ? this.maxDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxDateFilter),
            this.minDateFilter === undefined ? this.minDateFilter : this._dateTimeService.getStartOfDayForDate(this.minDateFilter),
            this.maxPurchaseAmountFilter == null ? this.maxPurchaseAmountFilterEmpty: this.maxPurchaseAmountFilter,
            this.minPurchaseAmountFilter == null ? this.minPurchaseAmountFilterEmpty: this.minPurchaseAmountFilter,
            this.maxPointsEarnedFilter == null ? this.maxPointsEarnedFilterEmpty: this.maxPointsEarnedFilter,
            this.minPointsEarnedFilter == null ? this.minPointsEarnedFilterEmpty: this.minPointsEarnedFilter,
            this.maxPointsBalanceFilter == null ? this.maxPointsBalanceFilterEmpty: this.maxPointsBalanceFilter,
            this.minPointsBalanceFilter == null ? this.minPointsBalanceFilterEmpty: this.minPointsBalanceFilter,
            this.maxPointsDeductedFilter == null ? this.maxPointsDeductedFilterEmpty: this.maxPointsDeductedFilter,
            this.minPointsDeductedFilter == null ? this.minPointsDeductedFilterEmpty: this.minPointsDeductedFilter,
            this.rewardPointTypeNameFilter,
            this.orderInvoiceNumberFilter,
            this.contactFullNameFilter,
            this.jobTitleFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createRewardPointHistory(): void {
        this.createOrEditRewardPointHistoryModal.show();        
    }


    deleteRewardPointHistory(rewardPointHistory: RewardPointHistoryDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._rewardPointHistoriesServiceProxy.delete(rewardPointHistory.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._rewardPointHistoriesServiceProxy.getRewardPointHistoriesToExcel(
        this.filterText,
            this.earnedOrRedeemedFilter,
            this.maxDateFilter === undefined ? this.maxDateFilter : this._dateTimeService.getEndOfDayForDate(this.maxDateFilter),
            this.minDateFilter === undefined ? this.minDateFilter : this._dateTimeService.getStartOfDayForDate(this.minDateFilter),
            this.maxPurchaseAmountFilter == null ? this.maxPurchaseAmountFilterEmpty: this.maxPurchaseAmountFilter,
            this.minPurchaseAmountFilter == null ? this.minPurchaseAmountFilterEmpty: this.minPurchaseAmountFilter,
            this.maxPointsEarnedFilter == null ? this.maxPointsEarnedFilterEmpty: this.maxPointsEarnedFilter,
            this.minPointsEarnedFilter == null ? this.minPointsEarnedFilterEmpty: this.minPointsEarnedFilter,
            this.maxPointsBalanceFilter == null ? this.maxPointsBalanceFilterEmpty: this.maxPointsBalanceFilter,
            this.minPointsBalanceFilter == null ? this.minPointsBalanceFilterEmpty: this.minPointsBalanceFilter,
            this.maxPointsDeductedFilter == null ? this.maxPointsDeductedFilterEmpty: this.maxPointsDeductedFilter,
            this.minPointsDeductedFilter == null ? this.minPointsDeductedFilterEmpty: this.minPointsDeductedFilter,
            this.rewardPointTypeNameFilter,
            this.orderInvoiceNumberFilter,
            this.contactFullNameFilter,
            this.jobTitleFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.earnedOrRedeemedFilter = -1;
    this.maxDateFilter = undefined;
		this.minDateFilter = undefined;
    this.maxPurchaseAmountFilter = this.maxPurchaseAmountFilterEmpty;
		this.minPurchaseAmountFilter = this.maxPurchaseAmountFilterEmpty;
    this.maxPointsEarnedFilter = this.maxPointsEarnedFilterEmpty;
		this.minPointsEarnedFilter = this.maxPointsEarnedFilterEmpty;
    this.maxPointsBalanceFilter = this.maxPointsBalanceFilterEmpty;
		this.minPointsBalanceFilter = this.maxPointsBalanceFilterEmpty;
    this.maxPointsDeductedFilter = this.maxPointsDeductedFilterEmpty;
		this.minPointsDeductedFilter = this.maxPointsDeductedFilterEmpty;
		this.rewardPointTypeNameFilter = '';
							this.orderInvoiceNumberFilter = '';
							this.contactFullNameFilter = '';
							this.jobTitleFilter = '';
					
        this.getRewardPointHistories();
    }
}
