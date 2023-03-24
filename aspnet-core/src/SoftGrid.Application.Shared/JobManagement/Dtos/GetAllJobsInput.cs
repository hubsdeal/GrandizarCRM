using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.JobManagement.Dtos
{
    public class GetAllJobsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TitleFilter { get; set; }

        public int? FullTimeJobOrGigWorkProjectFilter { get; set; }

        public int? RemoteWorkOrOnSiteWorkFilter { get; set; }

        public int? SalaryBasedOrFixedPriceFilter { get; set; }

        public string SalaryOrStaffingRateFilter { get; set; }

        public string ReferralPointsFilter { get; set; }

        public int? TemplateFilter { get; set; }

        public int? MaxNumberOfJobsFilter { get; set; }
        public int? MinNumberOfJobsFilter { get; set; }

        public string MinimumExperienceFilter { get; set; }

        public string MaximumExperienceFilter { get; set; }

        public string JobDescriptionFilter { get; set; }

        public string JobLocationFullAddressFilter { get; set; }

        public string ZipCodeFilter { get; set; }

        public decimal? MaxLatitudeFilter { get; set; }
        public decimal? MinLatitudeFilter { get; set; }

        public decimal? MaxLongitudeFilter { get; set; }
        public decimal? MinLongitudeFilter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxHireByDateFilter { get; set; }
        public DateTime? MinHireByDateFilter { get; set; }

        public DateTime? MaxPublishDateFilter { get; set; }
        public DateTime? MinPublishDateFilter { get; set; }

        public DateTime? MaxExpirationDateFilter { get; set; }
        public DateTime? MinExpirationDateFilter { get; set; }

        public string InternalJobDescriptionFilter { get; set; }

        public string CityLocationFilter { get; set; }

        public int? PublishedFilter { get; set; }

        public string UrlFilter { get; set; }

        public string MasterTagCategoryNameFilter { get; set; }

        public string MasterTagNameFilter { get; set; }

        public string ProductCategoryNameFilter { get; set; }

        public string CurrencyNameFilter { get; set; }

        public string BusinessNameFilter { get; set; }

        public string CountryNameFilter { get; set; }

        public string StateNameFilter { get; set; }

        public string CityNameFilter { get; set; }

        public string JobStatusTypeNameFilter { get; set; }

        public string StoreNameFilter { get; set; }

    }
}