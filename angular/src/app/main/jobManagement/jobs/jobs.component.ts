import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JobsServiceProxy, JobDto, JobStatusCountDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditJobModalComponent } from './create-or-edit-job-modal.component';

import { ViewJobModalComponent } from './view-job-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import * as moment from 'moment';
import { Subscription } from 'rxjs';

@Component({
    templateUrl: './jobs.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class JobsComponent extends AppComponentBase {
    @ViewChild('createOrEditJobModal', { static: true }) createOrEditJobModal: CreateOrEditJobModalComponent;
    @ViewChild('viewJobModal', { static: true }) viewJobModal: ViewJobModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    creationTime = 100;
    jobStatusId: number;
    jobTypeId: number;
    organizationId: number;
    jobCategoryId: number;
    assignTeamId: number;
    marketPlaceId: number;
    dateRange: Date[] = []
    selectedAll: boolean = false;
    jobStatusOptions: any;
    organizationOptions: any;
    jobCategoryOptions: any;
    jobTypeOptions: any;
    teamOptions: any;
    assignEmployeeOptions: any;
    IndustryFilter: any;
    postedptions: any = [
        {
            id: 0,
            displayName: 'Today',
        },
        {
            id: 1,
            displayName: '1 Day',
        },
        {
            id: 7,
            displayName: '7 Day',
        },
        {
            id: 30,
            displayName: '30 Days',
        },
        {
            id: 90,
            displayName: '3 Months',
        },
        {
            id: 100,
            displayName: 'All',
        },
    ];
    saving = false;
    filterText = '';
    jobTitleFilter = '';
    maxNumberOfJobsFilter: number;
    maxNumberOfJobsFilterEmpty: number;
    minNumberOfJobsFilter: number;
    minNumberOfJobsFilterEmpty: number;
    minimumExperienceFilter = '';
    maximumExperienceFilter = '';
    salaryDependsOnExperienceFilter = -1;
    maxSalaryOrStaffingRateFilter: number;
    maxSalaryOrStaffingRateFilterEmpty: number;
    minSalaryOrStaffingRateFilter: number;
    minSalaryOrStaffingRateFilterEmpty: number;
    jobDescriptionFilter = '';
    jobLocationFullAddressFilter = '';
    zipCodeFilter = '';
    maxLatitudeFilter: number;
    maxLatitudeFilterEmpty: number;
    minLatitudeFilter: number;
    minLatitudeFilterEmpty: number;
    maxLongitudeFilter: number;
    maxLongitudeFilterEmpty: number;
    minLongitudeFilter: number;
    minLongitudeFilterEmpty: number;
    maxJobStartDateTimeFilter: moment.Moment;
    minJobStartDateTimeFilter: moment.Moment;
    maxHireByDateFilter: moment.Moment;
    minHireByDateFilter: moment.Moment;
    internalHireFilter = -1;
    maxPublishDateFilter: moment.Moment;
    minPublishDateFilter: moment.Moment;
    maxExpirationDateFilter: moment.Moment;
    minExpirationDateFilter: moment.Moment;
    internalJobDescriptionFilter = '';
    remoteWorkFilter = -1;
    cityLocationFilter = '';
    maxReferralPointsFilter: number;
    maxReferralPointsFilterEmpty: number;
    minReferralPointsFilter: number;
    minReferralPointsFilterEmpty: number;
    templateFilter = -1;
    masterTagCategoryNameFilter = '';
    masterTagNameFilter = '';
    currencyNameFilter = '';
    companyNameFilter = '';
    countryNameFilter = '';
    stateNameFilter = '';
    cityNameFilter = '';
    jobStatusTypeNameFilter = '';
    isRemoteWork = false;
    @Input() companyIdFilter: number;

    countryId: number;
    stateId: number;
    cityId: number;
    assignEmployeeId: number;

    @Input() creatorEmployeeId: number = null;
    jobStatusCountList: JobStatusCountDto[] = [];
    subscription: Subscription;
    constructor(
        injector: Injector,
        private _jobsServiceProxy: JobsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
    }

    getJobs(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._jobsServiceProxy
        this._jobsServiceProxy.getAllJobList(
            this.filterText,
            this.jobTitleFilter,
            this.cityLocationFilter == null ? undefined : this.cityLocationFilter,
            this.jobStatusId == null ? undefined : this.jobStatusId,
            this.jobCategoryId == null ? undefined : this.jobCategoryId,
            this.jobTypeId == null ? undefined : this.jobTypeId,
            this.assignEmployeeId == null ? undefined : this.assignEmployeeId,
            this.companyIdFilter == null ? undefined : this.companyIdFilter,
            undefined,
            undefined,
            this.creationTime == null ? undefined : this.creationTime,
            undefined,
            this.remoteWorkFilter,
            0,
            this.creatorEmployeeId == null ? undefined : this.creatorEmployeeId,
            undefined,
            undefined,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.jobs;
            this.jobStatusCountList = result.jobStatusCounts;
            console.log(this.jobStatusCountList);
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    onChangesSelectAll() {
        for (var i = 0; i < this.primengTableHelper.records.length; i++) {
          this.primengTableHelper.records[i].selected = this.selectedAll;
        }
      }
    
      checkIfAllSelected() {
        this.selectedAll = this.primengTableHelper.records.every(function (item: any) {
          return item.selected == true;
        });
      }
    
    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createJob(): void {
        this.createOrEditJobModal.show();
    }

    deleteJob(job: JobDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._jobsServiceProxy.delete(job.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    // exportToExcel(): void {
    //     this._jobsServiceProxy
    //         .getJobsToExcel(
    //             this.filterText,
    //             this.titleFilter,
    //             this.fullTimeJobOrGigWorkProjectFilter,
    //             this.remoteWorkOrOnSiteWorkFilter,
    //             this.salaryBasedOrFixedPriceFilter,
    //             this.salaryOrStaffingRateFilter,
    //             this.referralPointsFilter,
    //             this.templateFilter,
    //             this.maxNumberOfJobsFilter == null ? this.maxNumberOfJobsFilterEmpty : this.maxNumberOfJobsFilter,
    //             this.minNumberOfJobsFilter == null ? this.minNumberOfJobsFilterEmpty : this.minNumberOfJobsFilter,
    //             this.minimumExperienceFilter,
    //             this.maximumExperienceFilter,
    //             this.jobDescriptionFilter,
    //             this.jobLocationFullAddressFilter,
    //             this.zipCodeFilter,
    //             this.maxLatitudeFilter == null ? this.maxLatitudeFilterEmpty : this.maxLatitudeFilter,
    //             this.minLatitudeFilter == null ? this.minLatitudeFilterEmpty : this.minLatitudeFilter,
    //             this.maxLongitudeFilter == null ? this.maxLongitudeFilterEmpty : this.maxLongitudeFilter,
    //             this.minLongitudeFilter == null ? this.minLongitudeFilterEmpty : this.minLongitudeFilter,
    //             this.maxStartDateFilter === undefined
    //                 ? this.maxStartDateFilter
    //                 : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
    //             this.minStartDateFilter === undefined
    //                 ? this.minStartDateFilter
    //                 : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
    //             this.maxHireByDateFilter === undefined
    //                 ? this.maxHireByDateFilter
    //                 : this._dateTimeService.getEndOfDayForDate(this.maxHireByDateFilter),
    //             this.minHireByDateFilter === undefined
    //                 ? this.minHireByDateFilter
    //                 : this._dateTimeService.getStartOfDayForDate(this.minHireByDateFilter),
    //             this.maxPublishDateFilter === undefined
    //                 ? this.maxPublishDateFilter
    //                 : this._dateTimeService.getEndOfDayForDate(this.maxPublishDateFilter),
    //             this.minPublishDateFilter === undefined
    //                 ? this.minPublishDateFilter
    //                 : this._dateTimeService.getStartOfDayForDate(this.minPublishDateFilter),
    //             this.maxExpirationDateFilter === undefined
    //                 ? this.maxExpirationDateFilter
    //                 : this._dateTimeService.getEndOfDayForDate(this.maxExpirationDateFilter),
    //             this.minExpirationDateFilter === undefined
    //                 ? this.minExpirationDateFilter
    //                 : this._dateTimeService.getStartOfDayForDate(this.minExpirationDateFilter),
    //             this.internalJobDescriptionFilter,
    //             this.cityLocationFilter,
    //             this.publishedFilter,
    //             this.urlFilter,
    //             this.masterTagCategoryNameFilter,
    //             this.masterTagNameFilter,
    //             this.productCategoryNameFilter,
    //             this.currencyNameFilter,
    //             this.businessNameFilter,
    //             this.countryNameFilter,
    //             this.stateNameFilter,
    //             this.cityNameFilter,
    //             this.jobStatusTypeNameFilter,
    //             this.storeNameFilter
    //         )
    //         .subscribe((result) => {
    //             this._fileDownloadService.downloadTempFile(result);
    //         });
    // }

    resetFilters(): void {
        // this.filterText = '';
        // this.titleFilter = '';
        // this.fullTimeJobOrGigWorkProjectFilter = -1;
        // this.remoteWorkOrOnSiteWorkFilter = -1;
        // this.salaryBasedOrFixedPriceFilter = -1;
        // this.salaryOrStaffingRateFilter = '';
        // this.referralPointsFilter = '';
        // this.templateFilter = -1;
        // this.maxNumberOfJobsFilter = this.maxNumberOfJobsFilterEmpty;
        // this.minNumberOfJobsFilter = this.maxNumberOfJobsFilterEmpty;
        // this.minimumExperienceFilter = '';
        // this.maximumExperienceFilter = '';
        // this.jobDescriptionFilter = '';
        // this.jobLocationFullAddressFilter = '';
        // this.zipCodeFilter = '';
        // this.maxLatitudeFilter = this.maxLatitudeFilterEmpty;
        // this.minLatitudeFilter = this.maxLatitudeFilterEmpty;
        // this.maxLongitudeFilter = this.maxLongitudeFilterEmpty;
        // this.minLongitudeFilter = this.maxLongitudeFilterEmpty;
        // this.maxStartDateFilter = undefined;
        // this.minStartDateFilter = undefined;
        // this.maxHireByDateFilter = undefined;
        // this.minHireByDateFilter = undefined;
        // this.maxPublishDateFilter = undefined;
        // this.minPublishDateFilter = undefined;
        // this.maxExpirationDateFilter = undefined;
        // this.minExpirationDateFilter = undefined;
        // this.internalJobDescriptionFilter = '';
        // this.cityLocationFilter = '';
        // this.publishedFilter = -1;
        // this.urlFilter = '';
        // this.masterTagCategoryNameFilter = '';
        // this.masterTagNameFilter = '';
        // this.productCategoryNameFilter = '';
        // this.currencyNameFilter = '';
        // this.businessNameFilter = '';
        // this.countryNameFilter = '';
        // this.stateNameFilter = '';
        // this.cityNameFilter = '';
        // this.jobStatusTypeNameFilter = '';
        // this.storeNameFilter = '';

        this.getJobs();
    }
}
