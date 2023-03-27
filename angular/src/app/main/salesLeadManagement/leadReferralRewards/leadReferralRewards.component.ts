import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LeadReferralRewardsServiceProxy, LeadReferralRewardDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditLeadReferralRewardModalComponent } from './create-or-edit-leadReferralReward-modal.component';

import { ViewLeadReferralRewardModalComponent } from './view-leadReferralReward-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './leadReferralRewards.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class LeadReferralRewardsComponent extends AppComponentBase {
    @ViewChild('createOrEditLeadReferralRewardModal', { static: true })
    createOrEditLeadReferralRewardModal: CreateOrEditLeadReferralRewardModalComponent;
    @ViewChild('viewLeadReferralRewardModal', { static: true })
    viewLeadReferralRewardModal: ViewLeadReferralRewardModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    firstNameFilter = '';
    lastNameFilter = '';
    phoneFilter = '';
    emailFilter = '';
    rewardTypeFilter = '';
    maxRewardAmountFilter: number;
    maxRewardAmountFilterEmpty: number;
    minRewardAmountFilter: number;
    minRewardAmountFilterEmpty: number;
    rewardStatusFilter = -1;
    leadTitleFilter = '';
    contactFullNameFilter = '';

    constructor(
        injector: Injector,
        private _leadReferralRewardsServiceProxy: LeadReferralRewardsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getLeadReferralRewards(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._leadReferralRewardsServiceProxy
            .getAll(
                this.filterText,
                this.firstNameFilter,
                this.lastNameFilter,
                this.phoneFilter,
                this.emailFilter,
                this.rewardTypeFilter,
                this.maxRewardAmountFilter == null ? this.maxRewardAmountFilterEmpty : this.maxRewardAmountFilter,
                this.minRewardAmountFilter == null ? this.minRewardAmountFilterEmpty : this.minRewardAmountFilter,
                this.rewardStatusFilter,
                this.leadTitleFilter,
                this.contactFullNameFilter,
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

    createLeadReferralReward(): void {
        this.createOrEditLeadReferralRewardModal.show();
    }

    deleteLeadReferralReward(leadReferralReward: LeadReferralRewardDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._leadReferralRewardsServiceProxy.delete(leadReferralReward.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._leadReferralRewardsServiceProxy
            .getLeadReferralRewardsToExcel(
                this.filterText,
                this.firstNameFilter,
                this.lastNameFilter,
                this.phoneFilter,
                this.emailFilter,
                this.rewardTypeFilter,
                this.maxRewardAmountFilter == null ? this.maxRewardAmountFilterEmpty : this.maxRewardAmountFilter,
                this.minRewardAmountFilter == null ? this.minRewardAmountFilterEmpty : this.minRewardAmountFilter,
                this.rewardStatusFilter,
                this.leadTitleFilter,
                this.contactFullNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.firstNameFilter = '';
        this.lastNameFilter = '';
        this.phoneFilter = '';
        this.emailFilter = '';
        this.rewardTypeFilter = '';
        this.maxRewardAmountFilter = this.maxRewardAmountFilterEmpty;
        this.minRewardAmountFilter = this.maxRewardAmountFilterEmpty;
        this.rewardStatusFilter = -1;
        this.leadTitleFilter = '';
        this.contactFullNameFilter = '';

        this.getLeadReferralRewards();
    }
}
