import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { RewardPointPriceSettingsServiceProxy, RewardPointPriceSettingDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRewardPointPriceSettingModalComponent } from './create-or-edit-rewardPointPriceSetting-modal.component';

import { ViewRewardPointPriceSettingModalComponent } from './view-rewardPointPriceSetting-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './rewardPointPriceSettings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class RewardPointPriceSettingsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditRewardPointPriceSettingModal', { static: true }) createOrEditRewardPointPriceSettingModal: CreateOrEditRewardPointPriceSettingModalComponent;
    @ViewChild('viewRewardPointPriceSettingModal', { static: true }) viewRewardPointPriceSettingModal: ViewRewardPointPriceSettingModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxNumberOfPointsFilter : number;
		maxNumberOfPointsFilterEmpty : number;
		minNumberOfPointsFilter : number;
		minNumberOfPointsFilterEmpty : number;
    maxCreditAmountFilter : number;
		maxCreditAmountFilterEmpty : number;
		minCreditAmountFilter : number;
		minCreditAmountFilterEmpty : number;
    activeFilter = -1;
        currencyNameFilter = '';






    constructor(
        injector: Injector,
        private _rewardPointPriceSettingsServiceProxy: RewardPointPriceSettingsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getRewardPointPriceSettings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._rewardPointPriceSettingsServiceProxy.getAll(
            this.filterText,
            this.maxNumberOfPointsFilter == null ? this.maxNumberOfPointsFilterEmpty: this.maxNumberOfPointsFilter,
            this.minNumberOfPointsFilter == null ? this.minNumberOfPointsFilterEmpty: this.minNumberOfPointsFilter,
            this.maxCreditAmountFilter == null ? this.maxCreditAmountFilterEmpty: this.maxCreditAmountFilter,
            this.minCreditAmountFilter == null ? this.minCreditAmountFilterEmpty: this.minCreditAmountFilter,
            this.activeFilter,
            this.currencyNameFilter,
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

    createRewardPointPriceSetting(): void {
        this.createOrEditRewardPointPriceSettingModal.show();        
    }


    deleteRewardPointPriceSetting(rewardPointPriceSetting: RewardPointPriceSettingDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._rewardPointPriceSettingsServiceProxy.delete(rewardPointPriceSetting.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._rewardPointPriceSettingsServiceProxy.getRewardPointPriceSettingsToExcel(
        this.filterText,
            this.maxNumberOfPointsFilter == null ? this.maxNumberOfPointsFilterEmpty: this.maxNumberOfPointsFilter,
            this.minNumberOfPointsFilter == null ? this.minNumberOfPointsFilterEmpty: this.minNumberOfPointsFilter,
            this.maxCreditAmountFilter == null ? this.maxCreditAmountFilterEmpty: this.maxCreditAmountFilter,
            this.minCreditAmountFilter == null ? this.minCreditAmountFilterEmpty: this.minCreditAmountFilter,
            this.activeFilter,
            this.currencyNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.maxNumberOfPointsFilter = this.maxNumberOfPointsFilterEmpty;
		this.minNumberOfPointsFilter = this.maxNumberOfPointsFilterEmpty;
    this.maxCreditAmountFilter = this.maxCreditAmountFilterEmpty;
		this.minCreditAmountFilter = this.maxCreditAmountFilterEmpty;
    this.activeFilter = -1;
		this.currencyNameFilter = '';
					
        this.getRewardPointPriceSettings();
    }
}
