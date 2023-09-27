import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { JobReferralFeeSplitPoliciesServiceProxy, JobReferralFeeSplitPolicyDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditJobReferralFeeSplitPolicyModalComponent } from './create-or-edit-jobReferralFeeSplitPolicy-modal.component';

import { ViewJobReferralFeeSplitPolicyModalComponent } from './view-jobReferralFeeSplitPolicy-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './jobReferralFeeSplitPolicies.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class JobReferralFeeSplitPoliciesComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditJobReferralFeeSplitPolicyModal', { static: true }) createOrEditJobReferralFeeSplitPolicyModal: CreateOrEditJobReferralFeeSplitPolicyModalComponent;
    @ViewChild('viewJobReferralFeeSplitPolicyModal', { static: true }) viewJobReferralFeeSplitPolicyModal: ViewJobReferralFeeSplitPolicyModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    titleNameFilter = '';
    maxApplicantAmountPercentageFilter : number;
		maxApplicantAmountPercentageFilterEmpty : number;
		minApplicantAmountPercentageFilter : number;
		minApplicantAmountPercentageFilterEmpty : number;
    maxReferrerPercentageFilter : number;
		maxReferrerPercentageFilterEmpty : number;
		minReferrerPercentageFilter : number;
		minReferrerPercentageFilterEmpty : number;
    maxHubPartnerPercentageFilter : number;
		maxHubPartnerPercentageFilterEmpty : number;
		minHubPartnerPercentageFilter : number;
		minHubPartnerPercentageFilterEmpty : number;
    maxEmployeePercentageFilter : number;
		maxEmployeePercentageFilterEmpty : number;
		minEmployeePercentageFilter : number;
		minEmployeePercentageFilterEmpty : number;
    maxPlatformPercentageFilter : number;
		maxPlatformPercentageFilterEmpty : number;
		minPlatformPercentageFilter : number;
		minPlatformPercentageFilterEmpty : number;
    isActiveFilter = -1;






    constructor(
        injector: Injector,
        private _jobReferralFeeSplitPoliciesServiceProxy: JobReferralFeeSplitPoliciesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getJobReferralFeeSplitPolicies(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._jobReferralFeeSplitPoliciesServiceProxy.getAll(
            this.filterText,
            this.titleNameFilter,
            this.maxApplicantAmountPercentageFilter == null ? this.maxApplicantAmountPercentageFilterEmpty: this.maxApplicantAmountPercentageFilter,
            this.minApplicantAmountPercentageFilter == null ? this.minApplicantAmountPercentageFilterEmpty: this.minApplicantAmountPercentageFilter,
            this.maxReferrerPercentageFilter == null ? this.maxReferrerPercentageFilterEmpty: this.maxReferrerPercentageFilter,
            this.minReferrerPercentageFilter == null ? this.minReferrerPercentageFilterEmpty: this.minReferrerPercentageFilter,
            this.maxHubPartnerPercentageFilter == null ? this.maxHubPartnerPercentageFilterEmpty: this.maxHubPartnerPercentageFilter,
            this.minHubPartnerPercentageFilter == null ? this.minHubPartnerPercentageFilterEmpty: this.minHubPartnerPercentageFilter,
            this.maxEmployeePercentageFilter == null ? this.maxEmployeePercentageFilterEmpty: this.maxEmployeePercentageFilter,
            this.minEmployeePercentageFilter == null ? this.minEmployeePercentageFilterEmpty: this.minEmployeePercentageFilter,
            this.maxPlatformPercentageFilter == null ? this.maxPlatformPercentageFilterEmpty: this.maxPlatformPercentageFilter,
            this.minPlatformPercentageFilter == null ? this.minPlatformPercentageFilterEmpty: this.minPlatformPercentageFilter,
            this.isActiveFilter,
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

    createJobReferralFeeSplitPolicy(): void {
        this.createOrEditJobReferralFeeSplitPolicyModal.show();        
    }


    deleteJobReferralFeeSplitPolicy(jobReferralFeeSplitPolicy: JobReferralFeeSplitPolicyDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._jobReferralFeeSplitPoliciesServiceProxy.delete(jobReferralFeeSplitPolicy.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._jobReferralFeeSplitPoliciesServiceProxy.getJobReferralFeeSplitPoliciesToExcel(
        this.filterText,
            this.titleNameFilter,
            this.maxApplicantAmountPercentageFilter == null ? this.maxApplicantAmountPercentageFilterEmpty: this.maxApplicantAmountPercentageFilter,
            this.minApplicantAmountPercentageFilter == null ? this.minApplicantAmountPercentageFilterEmpty: this.minApplicantAmountPercentageFilter,
            this.maxReferrerPercentageFilter == null ? this.maxReferrerPercentageFilterEmpty: this.maxReferrerPercentageFilter,
            this.minReferrerPercentageFilter == null ? this.minReferrerPercentageFilterEmpty: this.minReferrerPercentageFilter,
            this.maxHubPartnerPercentageFilter == null ? this.maxHubPartnerPercentageFilterEmpty: this.maxHubPartnerPercentageFilter,
            this.minHubPartnerPercentageFilter == null ? this.minHubPartnerPercentageFilterEmpty: this.minHubPartnerPercentageFilter,
            this.maxEmployeePercentageFilter == null ? this.maxEmployeePercentageFilterEmpty: this.maxEmployeePercentageFilter,
            this.minEmployeePercentageFilter == null ? this.minEmployeePercentageFilterEmpty: this.minEmployeePercentageFilter,
            this.maxPlatformPercentageFilter == null ? this.maxPlatformPercentageFilterEmpty: this.maxPlatformPercentageFilter,
            this.minPlatformPercentageFilter == null ? this.minPlatformPercentageFilterEmpty: this.minPlatformPercentageFilter,
            this.isActiveFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.titleNameFilter = '';
    this.maxApplicantAmountPercentageFilter = this.maxApplicantAmountPercentageFilterEmpty;
		this.minApplicantAmountPercentageFilter = this.maxApplicantAmountPercentageFilterEmpty;
    this.maxReferrerPercentageFilter = this.maxReferrerPercentageFilterEmpty;
		this.minReferrerPercentageFilter = this.maxReferrerPercentageFilterEmpty;
    this.maxHubPartnerPercentageFilter = this.maxHubPartnerPercentageFilterEmpty;
		this.minHubPartnerPercentageFilter = this.maxHubPartnerPercentageFilterEmpty;
    this.maxEmployeePercentageFilter = this.maxEmployeePercentageFilterEmpty;
		this.minEmployeePercentageFilter = this.maxEmployeePercentageFilterEmpty;
    this.maxPlatformPercentageFilter = this.maxPlatformPercentageFilterEmpty;
		this.minPlatformPercentageFilter = this.maxPlatformPercentageFilterEmpty;
    this.isActiveFilter = -1;

        this.getJobReferralFeeSplitPolicies();
    }
}
