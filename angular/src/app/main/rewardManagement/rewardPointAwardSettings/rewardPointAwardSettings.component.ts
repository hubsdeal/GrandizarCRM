import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { RewardPointAwardSettingsServiceProxy, RewardPointAwardSettingDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRewardPointAwardSettingModalComponent } from './create-or-edit-rewardPointAwardSetting-modal.component';

import { ViewRewardPointAwardSettingModalComponent } from './view-rewardPointAwardSetting-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './rewardPointAwardSettings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class RewardPointAwardSettingsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditRewardPointAwardSettingModal', { static: true }) createOrEditRewardPointAwardSettingModal: CreateOrEditRewardPointAwardSettingModalComponent;
    @ViewChild('viewRewardPointAwardSettingModal', { static: true }) viewRewardPointAwardSettingModal: ViewRewardPointAwardSettingModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxBaseAmountFilter : number;
		maxBaseAmountFilterEmpty : number;
		minBaseAmountFilter : number;
		minBaseAmountFilterEmpty : number;
    maxRewardPointFilter : number;
		maxRewardPointFilterEmpty : number;
		minRewardPointFilter : number;
		minRewardPointFilterEmpty : number;
    maxPremiumMemberBonusPercentageFilter : number;
		maxPremiumMemberBonusPercentageFilterEmpty : number;
		minPremiumMemberBonusPercentageFilter : number;
		minPremiumMemberBonusPercentageFilterEmpty : number;
    deductPointForReturnFilter = -1;
    maxAddPointsAfterDaysFilter : number;
		maxAddPointsAfterDaysFilterEmpty : number;
		minAddPointsAfterDaysFilter : number;
		minAddPointsAfterDaysFilterEmpty : number;
        rewardPointTypeNameFilter = '';
        storeNameFilter = '';
        productNameFilter = '';
        membershipTypeNameFilter = '';






    constructor(
        injector: Injector,
        private _rewardPointAwardSettingsServiceProxy: RewardPointAwardSettingsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getRewardPointAwardSettings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._rewardPointAwardSettingsServiceProxy.getAll(
            this.filterText,
            this.maxBaseAmountFilter == null ? this.maxBaseAmountFilterEmpty: this.maxBaseAmountFilter,
            this.minBaseAmountFilter == null ? this.minBaseAmountFilterEmpty: this.minBaseAmountFilter,
            this.maxRewardPointFilter == null ? this.maxRewardPointFilterEmpty: this.maxRewardPointFilter,
            this.minRewardPointFilter == null ? this.minRewardPointFilterEmpty: this.minRewardPointFilter,
            this.maxPremiumMemberBonusPercentageFilter == null ? this.maxPremiumMemberBonusPercentageFilterEmpty: this.maxPremiumMemberBonusPercentageFilter,
            this.minPremiumMemberBonusPercentageFilter == null ? this.minPremiumMemberBonusPercentageFilterEmpty: this.minPremiumMemberBonusPercentageFilter,
            this.deductPointForReturnFilter,
            this.maxAddPointsAfterDaysFilter == null ? this.maxAddPointsAfterDaysFilterEmpty: this.maxAddPointsAfterDaysFilter,
            this.minAddPointsAfterDaysFilter == null ? this.minAddPointsAfterDaysFilterEmpty: this.minAddPointsAfterDaysFilter,
            this.rewardPointTypeNameFilter,
            this.storeNameFilter,
            this.productNameFilter,
            this.membershipTypeNameFilter,
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

    createRewardPointAwardSetting(): void {
        this.createOrEditRewardPointAwardSettingModal.show();        
    }


    deleteRewardPointAwardSetting(rewardPointAwardSetting: RewardPointAwardSettingDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._rewardPointAwardSettingsServiceProxy.delete(rewardPointAwardSetting.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._rewardPointAwardSettingsServiceProxy.getRewardPointAwardSettingsToExcel(
        this.filterText,
            this.maxBaseAmountFilter == null ? this.maxBaseAmountFilterEmpty: this.maxBaseAmountFilter,
            this.minBaseAmountFilter == null ? this.minBaseAmountFilterEmpty: this.minBaseAmountFilter,
            this.maxRewardPointFilter == null ? this.maxRewardPointFilterEmpty: this.maxRewardPointFilter,
            this.minRewardPointFilter == null ? this.minRewardPointFilterEmpty: this.minRewardPointFilter,
            this.maxPremiumMemberBonusPercentageFilter == null ? this.maxPremiumMemberBonusPercentageFilterEmpty: this.maxPremiumMemberBonusPercentageFilter,
            this.minPremiumMemberBonusPercentageFilter == null ? this.minPremiumMemberBonusPercentageFilterEmpty: this.minPremiumMemberBonusPercentageFilter,
            this.deductPointForReturnFilter,
            this.maxAddPointsAfterDaysFilter == null ? this.maxAddPointsAfterDaysFilterEmpty: this.maxAddPointsAfterDaysFilter,
            this.minAddPointsAfterDaysFilter == null ? this.minAddPointsAfterDaysFilterEmpty: this.minAddPointsAfterDaysFilter,
            this.rewardPointTypeNameFilter,
            this.storeNameFilter,
            this.productNameFilter,
            this.membershipTypeNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.maxBaseAmountFilter = this.maxBaseAmountFilterEmpty;
		this.minBaseAmountFilter = this.maxBaseAmountFilterEmpty;
    this.maxRewardPointFilter = this.maxRewardPointFilterEmpty;
		this.minRewardPointFilter = this.maxRewardPointFilterEmpty;
    this.maxPremiumMemberBonusPercentageFilter = this.maxPremiumMemberBonusPercentageFilterEmpty;
		this.minPremiumMemberBonusPercentageFilter = this.maxPremiumMemberBonusPercentageFilterEmpty;
    this.deductPointForReturnFilter = -1;
    this.maxAddPointsAfterDaysFilter = this.maxAddPointsAfterDaysFilterEmpty;
		this.minAddPointsAfterDaysFilter = this.maxAddPointsAfterDaysFilterEmpty;
		this.rewardPointTypeNameFilter = '';
							this.storeNameFilter = '';
							this.productNameFilter = '';
							this.membershipTypeNameFilter = '';
					
        this.getRewardPointAwardSettings();
    }
}
