import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { JobReferralFeeDetailsServiceProxy, JobReferralFeeDetailDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditJobReferralFeeDetailModalComponent } from './create-or-edit-jobReferralFeeDetail-modal.component';

import { ViewJobReferralFeeDetailModalComponent } from './view-jobReferralFeeDetail-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './jobReferralFeeDetails.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class JobReferralFeeDetailsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditJobReferralFeeDetailModal', { static: true }) createOrEditJobReferralFeeDetailModal: CreateOrEditJobReferralFeeDetailModalComponent;
    @ViewChild('viewJobReferralFeeDetailModal', { static: true }) viewJobReferralFeeDetailModal: ViewJobReferralFeeDetailModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxTotalReferralAmountFilter : number;
		maxTotalReferralAmountFilterEmpty : number;
		minTotalReferralAmountFilter : number;
		minTotalReferralAmountFilterEmpty : number;
    maxPlatformPercentageFilter : number;
		maxPlatformPercentageFilterEmpty : number;
		minPlatformPercentageFilter : number;
		minPlatformPercentageFilterEmpty : number;
    maxPlatformAmountFilter : number;
		maxPlatformAmountFilterEmpty : number;
		minPlatformAmountFilter : number;
		minPlatformAmountFilterEmpty : number;
    maxApplicantAmountFilter : number;
		maxApplicantAmountFilterEmpty : number;
		minApplicantAmountFilter : number;
		minApplicantAmountFilterEmpty : number;
    maxApplicantAmountPercentageFilter : number;
		maxApplicantAmountPercentageFilterEmpty : number;
		minApplicantAmountPercentageFilter : number;
		minApplicantAmountPercentageFilterEmpty : number;
    applicantPaidFilter = -1;
    maxReferralContactPercentageFilter : number;
		maxReferralContactPercentageFilterEmpty : number;
		minReferralContactPercentageFilter : number;
		minReferralContactPercentageFilterEmpty : number;
    maxReferrerAmountFilter : number;
		maxReferrerAmountFilterEmpty : number;
		minReferrerAmountFilter : number;
		minReferrerAmountFilterEmpty : number;
    referrerPaidFilter = -1;
    maxHubPartnerPercentageFilter : number;
		maxHubPartnerPercentageFilterEmpty : number;
		minHubPartnerPercentageFilter : number;
		minHubPartnerPercentageFilterEmpty : number;
    maxHubPartnerAmountFilter : number;
		maxHubPartnerAmountFilterEmpty : number;
		minHubPartnerAmountFilter : number;
		minHubPartnerAmountFilterEmpty : number;
    hubPartnerPaidFilter = -1;
    maxEmployeePercentageFilter : number;
		maxEmployeePercentageFilterEmpty : number;
		minEmployeePercentageFilter : number;
		minEmployeePercentageFilterEmpty : number;
    maxEmployeeAmountFilter : number;
		maxEmployeeAmountFilterEmpty : number;
		minEmployeeAmountFilter : number;
		minEmployeeAmountFilterEmpty : number;
    employeePaidFilter = -1;
    remarksFilter = '';
        jobTitleFilter = '';
        storeNameFilter = '';
        businessNameFilter = '';
        orderInvoiceNumberFilter = '';
        jobReferralFeeSplitPolicyTitleNameFilter = '';
        currencyNameFilter = '';
        contactFullNameFilter = '';
        contactFullName2Filter = '';
        employeeNameFilter = '';
        employeeName2Filter = '';






    constructor(
        injector: Injector,
        private _jobReferralFeeDetailsServiceProxy: JobReferralFeeDetailsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getJobReferralFeeDetails(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records &&
                this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._jobReferralFeeDetailsServiceProxy.getAll(
            this.filterText,
            this.maxTotalReferralAmountFilter == null ? this.maxTotalReferralAmountFilterEmpty: this.maxTotalReferralAmountFilter,
            this.minTotalReferralAmountFilter == null ? this.minTotalReferralAmountFilterEmpty: this.minTotalReferralAmountFilter,
            this.maxPlatformPercentageFilter == null ? this.maxPlatformPercentageFilterEmpty: this.maxPlatformPercentageFilter,
            this.minPlatformPercentageFilter == null ? this.minPlatformPercentageFilterEmpty: this.minPlatformPercentageFilter,
            this.maxPlatformAmountFilter == null ? this.maxPlatformAmountFilterEmpty: this.maxPlatformAmountFilter,
            this.minPlatformAmountFilter == null ? this.minPlatformAmountFilterEmpty: this.minPlatformAmountFilter,
            this.maxApplicantAmountFilter == null ? this.maxApplicantAmountFilterEmpty: this.maxApplicantAmountFilter,
            this.minApplicantAmountFilter == null ? this.minApplicantAmountFilterEmpty: this.minApplicantAmountFilter,
            this.maxApplicantAmountPercentageFilter == null ? this.maxApplicantAmountPercentageFilterEmpty: this.maxApplicantAmountPercentageFilter,
            this.minApplicantAmountPercentageFilter == null ? this.minApplicantAmountPercentageFilterEmpty: this.minApplicantAmountPercentageFilter,
            this.applicantPaidFilter,
            this.maxReferralContactPercentageFilter == null ? this.maxReferralContactPercentageFilterEmpty: this.maxReferralContactPercentageFilter,
            this.minReferralContactPercentageFilter == null ? this.minReferralContactPercentageFilterEmpty: this.minReferralContactPercentageFilter,
            this.maxReferrerAmountFilter == null ? this.maxReferrerAmountFilterEmpty: this.maxReferrerAmountFilter,
            this.minReferrerAmountFilter == null ? this.minReferrerAmountFilterEmpty: this.minReferrerAmountFilter,
            this.referrerPaidFilter,
            this.maxHubPartnerPercentageFilter == null ? this.maxHubPartnerPercentageFilterEmpty: this.maxHubPartnerPercentageFilter,
            this.minHubPartnerPercentageFilter == null ? this.minHubPartnerPercentageFilterEmpty: this.minHubPartnerPercentageFilter,
            this.maxHubPartnerAmountFilter == null ? this.maxHubPartnerAmountFilterEmpty: this.maxHubPartnerAmountFilter,
            this.minHubPartnerAmountFilter == null ? this.minHubPartnerAmountFilterEmpty: this.minHubPartnerAmountFilter,
            this.hubPartnerPaidFilter,
            this.maxEmployeePercentageFilter == null ? this.maxEmployeePercentageFilterEmpty: this.maxEmployeePercentageFilter,
            this.minEmployeePercentageFilter == null ? this.minEmployeePercentageFilterEmpty: this.minEmployeePercentageFilter,
            this.maxEmployeeAmountFilter == null ? this.maxEmployeeAmountFilterEmpty: this.maxEmployeeAmountFilter,
            this.minEmployeeAmountFilter == null ? this.minEmployeeAmountFilterEmpty: this.minEmployeeAmountFilter,
            this.employeePaidFilter,
            this.remarksFilter,
            this.jobTitleFilter,
            this.storeNameFilter,
            this.businessNameFilter,
            this.orderInvoiceNumberFilter,
            this.jobReferralFeeSplitPolicyTitleNameFilter,
            this.currencyNameFilter,
            this.contactFullNameFilter,
            this.contactFullName2Filter,
            this.employeeNameFilter,
            this.employeeName2Filter,
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

    createJobReferralFeeDetail(): void {
        this.createOrEditJobReferralFeeDetailModal.show();        
    }


    deleteJobReferralFeeDetail(jobReferralFeeDetail: JobReferralFeeDetailDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._jobReferralFeeDetailsServiceProxy.delete(jobReferralFeeDetail.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._jobReferralFeeDetailsServiceProxy.getJobReferralFeeDetailsToExcel(
        this.filterText,
            this.maxTotalReferralAmountFilter == null ? this.maxTotalReferralAmountFilterEmpty: this.maxTotalReferralAmountFilter,
            this.minTotalReferralAmountFilter == null ? this.minTotalReferralAmountFilterEmpty: this.minTotalReferralAmountFilter,
            this.maxPlatformPercentageFilter == null ? this.maxPlatformPercentageFilterEmpty: this.maxPlatformPercentageFilter,
            this.minPlatformPercentageFilter == null ? this.minPlatformPercentageFilterEmpty: this.minPlatformPercentageFilter,
            this.maxPlatformAmountFilter == null ? this.maxPlatformAmountFilterEmpty: this.maxPlatformAmountFilter,
            this.minPlatformAmountFilter == null ? this.minPlatformAmountFilterEmpty: this.minPlatformAmountFilter,
            this.maxApplicantAmountFilter == null ? this.maxApplicantAmountFilterEmpty: this.maxApplicantAmountFilter,
            this.minApplicantAmountFilter == null ? this.minApplicantAmountFilterEmpty: this.minApplicantAmountFilter,
            this.maxApplicantAmountPercentageFilter == null ? this.maxApplicantAmountPercentageFilterEmpty: this.maxApplicantAmountPercentageFilter,
            this.minApplicantAmountPercentageFilter == null ? this.minApplicantAmountPercentageFilterEmpty: this.minApplicantAmountPercentageFilter,
            this.applicantPaidFilter,
            this.maxReferralContactPercentageFilter == null ? this.maxReferralContactPercentageFilterEmpty: this.maxReferralContactPercentageFilter,
            this.minReferralContactPercentageFilter == null ? this.minReferralContactPercentageFilterEmpty: this.minReferralContactPercentageFilter,
            this.maxReferrerAmountFilter == null ? this.maxReferrerAmountFilterEmpty: this.maxReferrerAmountFilter,
            this.minReferrerAmountFilter == null ? this.minReferrerAmountFilterEmpty: this.minReferrerAmountFilter,
            this.referrerPaidFilter,
            this.maxHubPartnerPercentageFilter == null ? this.maxHubPartnerPercentageFilterEmpty: this.maxHubPartnerPercentageFilter,
            this.minHubPartnerPercentageFilter == null ? this.minHubPartnerPercentageFilterEmpty: this.minHubPartnerPercentageFilter,
            this.maxHubPartnerAmountFilter == null ? this.maxHubPartnerAmountFilterEmpty: this.maxHubPartnerAmountFilter,
            this.minHubPartnerAmountFilter == null ? this.minHubPartnerAmountFilterEmpty: this.minHubPartnerAmountFilter,
            this.hubPartnerPaidFilter,
            this.maxEmployeePercentageFilter == null ? this.maxEmployeePercentageFilterEmpty: this.maxEmployeePercentageFilter,
            this.minEmployeePercentageFilter == null ? this.minEmployeePercentageFilterEmpty: this.minEmployeePercentageFilter,
            this.maxEmployeeAmountFilter == null ? this.maxEmployeeAmountFilterEmpty: this.maxEmployeeAmountFilter,
            this.minEmployeeAmountFilter == null ? this.minEmployeeAmountFilterEmpty: this.minEmployeeAmountFilter,
            this.employeePaidFilter,
            this.remarksFilter,
            this.jobTitleFilter,
            this.storeNameFilter,
            this.businessNameFilter,
            this.orderInvoiceNumberFilter,
            this.jobReferralFeeSplitPolicyTitleNameFilter,
            this.currencyNameFilter,
            this.contactFullNameFilter,
            this.contactFullName2Filter,
            this.employeeNameFilter,
            this.employeeName2Filter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    

    resetFilters(): void {
        this.filterText = '';
            this.maxTotalReferralAmountFilter = this.maxTotalReferralAmountFilterEmpty;
		this.minTotalReferralAmountFilter = this.maxTotalReferralAmountFilterEmpty;
    this.maxPlatformPercentageFilter = this.maxPlatformPercentageFilterEmpty;
		this.minPlatformPercentageFilter = this.maxPlatformPercentageFilterEmpty;
    this.maxPlatformAmountFilter = this.maxPlatformAmountFilterEmpty;
		this.minPlatformAmountFilter = this.maxPlatformAmountFilterEmpty;
    this.maxApplicantAmountFilter = this.maxApplicantAmountFilterEmpty;
		this.minApplicantAmountFilter = this.maxApplicantAmountFilterEmpty;
    this.maxApplicantAmountPercentageFilter = this.maxApplicantAmountPercentageFilterEmpty;
		this.minApplicantAmountPercentageFilter = this.maxApplicantAmountPercentageFilterEmpty;
    this.applicantPaidFilter = -1;
    this.maxReferralContactPercentageFilter = this.maxReferralContactPercentageFilterEmpty;
		this.minReferralContactPercentageFilter = this.maxReferralContactPercentageFilterEmpty;
    this.maxReferrerAmountFilter = this.maxReferrerAmountFilterEmpty;
		this.minReferrerAmountFilter = this.maxReferrerAmountFilterEmpty;
    this.referrerPaidFilter = -1;
    this.maxHubPartnerPercentageFilter = this.maxHubPartnerPercentageFilterEmpty;
		this.minHubPartnerPercentageFilter = this.maxHubPartnerPercentageFilterEmpty;
    this.maxHubPartnerAmountFilter = this.maxHubPartnerAmountFilterEmpty;
		this.minHubPartnerAmountFilter = this.maxHubPartnerAmountFilterEmpty;
    this.hubPartnerPaidFilter = -1;
    this.maxEmployeePercentageFilter = this.maxEmployeePercentageFilterEmpty;
		this.minEmployeePercentageFilter = this.maxEmployeePercentageFilterEmpty;
    this.maxEmployeeAmountFilter = this.maxEmployeeAmountFilterEmpty;
		this.minEmployeeAmountFilter = this.maxEmployeeAmountFilterEmpty;
    this.employeePaidFilter = -1;
    this.remarksFilter = '';
		this.jobTitleFilter = '';
							this.storeNameFilter = '';
							this.businessNameFilter = '';
							this.orderInvoiceNumberFilter = '';
							this.jobReferralFeeSplitPolicyTitleNameFilter = '';
							this.currencyNameFilter = '';
							this.contactFullNameFilter = '';
							this.contactFullName2Filter = '';
							this.employeeNameFilter = '';
							this.employeeName2Filter = '';
					
        this.getJobReferralFeeDetails();
    }
}
