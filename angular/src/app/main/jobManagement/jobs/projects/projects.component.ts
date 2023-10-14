import { Component, Injector, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { JobsServiceProxy, TokenAuthServiceProxy, JobDto } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { NotifyService } from 'abp-ng2-module';
import { DateTime } from 'luxon';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { CreateOrEditJobModalComponent } from '../create-or-edit-job-modal.component';
import { ViewJobModalComponent } from '../view-job-modal.component';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})
export class ProjectsComponent extends AppComponentBase {
  @ViewChild('createOrEditJobModal', { static: true }) createOrEditJobModal: CreateOrEditJobModalComponent;
  @ViewChild('viewJobModal', { static: true }) viewJobModal: ViewJobModalComponent;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  titleFilter = '';
  fullTimeJobOrGigWorkProjectFilter = -1;
  remoteWorkOrOnSiteWorkFilter = -1;
  salaryBasedOrFixedPriceFilter = -1;
  salaryOrStaffingRateFilter = '';
  referralPointsFilter = '';
  templateFilter = -1;
  maxNumberOfJobsFilter: number;
  maxNumberOfJobsFilterEmpty: number;
  minNumberOfJobsFilter: number;
  minNumberOfJobsFilterEmpty: number;
  minimumExperienceFilter = '';
  maximumExperienceFilter = '';
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
  maxStartDateFilter: DateTime;
  minStartDateFilter: DateTime;
  maxHireByDateFilter: DateTime;
  minHireByDateFilter: DateTime;
  maxPublishDateFilter: DateTime;
  minPublishDateFilter: DateTime;
  maxExpirationDateFilter: DateTime;
  minExpirationDateFilter: DateTime;
  internalJobDescriptionFilter = '';
  cityLocationFilter = '';
  publishedFilter = -1;
  urlFilter = '';
  masterTagCategoryNameFilter = '';
  masterTagNameFilter = '';
  productCategoryNameFilter = '';
  currencyNameFilter = '';
  businessNameFilter = '';
  countryNameFilter = '';
  stateNameFilter = '';
  cityNameFilter = '';
  jobStatusTypeNameFilter = '';
  storeNameFilter = '';

  constructor(
      injector: Injector,
      private _jobsServiceProxy: JobsServiceProxy,
      private _notifyService: NotifyService,
      private _tokenAuth: TokenAuthServiceProxy,
      private _activatedRoute: ActivatedRoute,
      private _fileDownloadService: FileDownloadService,
      private _dateTimeService: DateTimeService
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
          .getAll(
              this.filterText,
              this.titleFilter,
              this.fullTimeJobOrGigWorkProjectFilter,
              this.remoteWorkOrOnSiteWorkFilter,
              this.salaryBasedOrFixedPriceFilter,
              this.salaryOrStaffingRateFilter,
              this.referralPointsFilter,
              this.templateFilter,
              this.maxNumberOfJobsFilter == null ? this.maxNumberOfJobsFilterEmpty : this.maxNumberOfJobsFilter,
              this.minNumberOfJobsFilter == null ? this.minNumberOfJobsFilterEmpty : this.minNumberOfJobsFilter,
              this.minimumExperienceFilter,
              this.maximumExperienceFilter,
              this.jobDescriptionFilter,
              this.jobLocationFullAddressFilter,
              this.zipCodeFilter,
              this.maxLatitudeFilter == null ? this.maxLatitudeFilterEmpty : this.maxLatitudeFilter,
              this.minLatitudeFilter == null ? this.minLatitudeFilterEmpty : this.minLatitudeFilter,
              this.maxLongitudeFilter == null ? this.maxLongitudeFilterEmpty : this.maxLongitudeFilter,
              this.minLongitudeFilter == null ? this.minLongitudeFilterEmpty : this.minLongitudeFilter,
              this.maxStartDateFilter === undefined
                  ? this.maxStartDateFilter
                  : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
              this.minStartDateFilter === undefined
                  ? this.minStartDateFilter
                  : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
              this.maxHireByDateFilter === undefined
                  ? this.maxHireByDateFilter
                  : this._dateTimeService.getEndOfDayForDate(this.maxHireByDateFilter),
              this.minHireByDateFilter === undefined
                  ? this.minHireByDateFilter
                  : this._dateTimeService.getStartOfDayForDate(this.minHireByDateFilter),
              this.maxPublishDateFilter === undefined
                  ? this.maxPublishDateFilter
                  : this._dateTimeService.getEndOfDayForDate(this.maxPublishDateFilter),
              this.minPublishDateFilter === undefined
                  ? this.minPublishDateFilter
                  : this._dateTimeService.getStartOfDayForDate(this.minPublishDateFilter),
              this.maxExpirationDateFilter === undefined
                  ? this.maxExpirationDateFilter
                  : this._dateTimeService.getEndOfDayForDate(this.maxExpirationDateFilter),
              this.minExpirationDateFilter === undefined
                  ? this.minExpirationDateFilter
                  : this._dateTimeService.getStartOfDayForDate(this.minExpirationDateFilter),
              this.internalJobDescriptionFilter,
              this.cityLocationFilter,
              this.publishedFilter,
              this.urlFilter,
              this.masterTagCategoryNameFilter,
              this.masterTagNameFilter,
              this.productCategoryNameFilter,
              this.currencyNameFilter,
              this.businessNameFilter,
              this.countryNameFilter,
              this.stateNameFilter,
              this.cityNameFilter,
              this.jobStatusTypeNameFilter,
              this.storeNameFilter,
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

  exportToExcel(): void {
      this._jobsServiceProxy
          .getJobsToExcel(
              this.filterText,
              this.titleFilter,
              this.fullTimeJobOrGigWorkProjectFilter,
              this.remoteWorkOrOnSiteWorkFilter,
              this.salaryBasedOrFixedPriceFilter,
              this.salaryOrStaffingRateFilter,
              this.referralPointsFilter,
              this.templateFilter,
              this.maxNumberOfJobsFilter == null ? this.maxNumberOfJobsFilterEmpty : this.maxNumberOfJobsFilter,
              this.minNumberOfJobsFilter == null ? this.minNumberOfJobsFilterEmpty : this.minNumberOfJobsFilter,
              this.minimumExperienceFilter,
              this.maximumExperienceFilter,
              this.jobDescriptionFilter,
              this.jobLocationFullAddressFilter,
              this.zipCodeFilter,
              this.maxLatitudeFilter == null ? this.maxLatitudeFilterEmpty : this.maxLatitudeFilter,
              this.minLatitudeFilter == null ? this.minLatitudeFilterEmpty : this.minLatitudeFilter,
              this.maxLongitudeFilter == null ? this.maxLongitudeFilterEmpty : this.maxLongitudeFilter,
              this.minLongitudeFilter == null ? this.minLongitudeFilterEmpty : this.minLongitudeFilter,
              this.maxStartDateFilter === undefined
                  ? this.maxStartDateFilter
                  : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
              this.minStartDateFilter === undefined
                  ? this.minStartDateFilter
                  : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
              this.maxHireByDateFilter === undefined
                  ? this.maxHireByDateFilter
                  : this._dateTimeService.getEndOfDayForDate(this.maxHireByDateFilter),
              this.minHireByDateFilter === undefined
                  ? this.minHireByDateFilter
                  : this._dateTimeService.getStartOfDayForDate(this.minHireByDateFilter),
              this.maxPublishDateFilter === undefined
                  ? this.maxPublishDateFilter
                  : this._dateTimeService.getEndOfDayForDate(this.maxPublishDateFilter),
              this.minPublishDateFilter === undefined
                  ? this.minPublishDateFilter
                  : this._dateTimeService.getStartOfDayForDate(this.minPublishDateFilter),
              this.maxExpirationDateFilter === undefined
                  ? this.maxExpirationDateFilter
                  : this._dateTimeService.getEndOfDayForDate(this.maxExpirationDateFilter),
              this.minExpirationDateFilter === undefined
                  ? this.minExpirationDateFilter
                  : this._dateTimeService.getStartOfDayForDate(this.minExpirationDateFilter),
              this.internalJobDescriptionFilter,
              this.cityLocationFilter,
              this.publishedFilter,
              this.urlFilter,
              this.masterTagCategoryNameFilter,
              this.masterTagNameFilter,
              this.productCategoryNameFilter,
              this.currencyNameFilter,
              this.businessNameFilter,
              this.countryNameFilter,
              this.stateNameFilter,
              this.cityNameFilter,
              this.jobStatusTypeNameFilter,
              this.storeNameFilter
          )
          .subscribe((result) => {
              this._fileDownloadService.downloadTempFile(result);
          });
  }

  resetFilters(): void {
      this.filterText = '';
      this.titleFilter = '';
      this.fullTimeJobOrGigWorkProjectFilter = -1;
      this.remoteWorkOrOnSiteWorkFilter = -1;
      this.salaryBasedOrFixedPriceFilter = -1;
      this.salaryOrStaffingRateFilter = '';
      this.referralPointsFilter = '';
      this.templateFilter = -1;
      this.maxNumberOfJobsFilter = this.maxNumberOfJobsFilterEmpty;
      this.minNumberOfJobsFilter = this.maxNumberOfJobsFilterEmpty;
      this.minimumExperienceFilter = '';
      this.maximumExperienceFilter = '';
      this.jobDescriptionFilter = '';
      this.jobLocationFullAddressFilter = '';
      this.zipCodeFilter = '';
      this.maxLatitudeFilter = this.maxLatitudeFilterEmpty;
      this.minLatitudeFilter = this.maxLatitudeFilterEmpty;
      this.maxLongitudeFilter = this.maxLongitudeFilterEmpty;
      this.minLongitudeFilter = this.maxLongitudeFilterEmpty;
      this.maxStartDateFilter = undefined;
      this.minStartDateFilter = undefined;
      this.maxHireByDateFilter = undefined;
      this.minHireByDateFilter = undefined;
      this.maxPublishDateFilter = undefined;
      this.minPublishDateFilter = undefined;
      this.maxExpirationDateFilter = undefined;
      this.minExpirationDateFilter = undefined;
      this.internalJobDescriptionFilter = '';
      this.cityLocationFilter = '';
      this.publishedFilter = -1;
      this.urlFilter = '';
      this.masterTagCategoryNameFilter = '';
      this.masterTagNameFilter = '';
      this.productCategoryNameFilter = '';
      this.currencyNameFilter = '';
      this.businessNameFilter = '';
      this.countryNameFilter = '';
      this.stateNameFilter = '';
      this.cityNameFilter = '';
      this.jobStatusTypeNameFilter = '';
      this.storeNameFilter = '';

      this.getJobs();
  }
}
