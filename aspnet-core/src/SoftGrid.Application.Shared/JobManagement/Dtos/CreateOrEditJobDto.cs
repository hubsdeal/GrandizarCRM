using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.JobManagement.Dtos
{
    public class CreateOrEditJobDto : EntityDto<long?>
    {

        [Required]
        [StringLength(JobConsts.MaxTitleLength, MinimumLength = JobConsts.MinTitleLength)]
        public string Title { get; set; }

        public bool FullTimeJobOrGigWorkProject { get; set; }

        public bool RemoteWorkOrOnSiteWork { get; set; }

        public bool SalaryBasedOrFixedPrice { get; set; }

        [StringLength(JobConsts.MaxSalaryOrStaffingRateLength, MinimumLength = JobConsts.MinSalaryOrStaffingRateLength)]
        public string SalaryOrStaffingRate { get; set; }

        [StringLength(JobConsts.MaxReferralPointsLength, MinimumLength = JobConsts.MinReferralPointsLength)]
        public string ReferralPoints { get; set; }

        public bool Template { get; set; }

        public int? NumberOfJobs { get; set; }

        [StringLength(JobConsts.MaxMinimumExperienceLength, MinimumLength = JobConsts.MinMinimumExperienceLength)]
        public string MinimumExperience { get; set; }

        [StringLength(JobConsts.MaxMaximumExperienceLength, MinimumLength = JobConsts.MinMaximumExperienceLength)]
        public string MaximumExperience { get; set; }

        public string JobDescription { get; set; }

        [StringLength(JobConsts.MaxJobLocationFullAddressLength, MinimumLength = JobConsts.MinJobLocationFullAddressLength)]
        public string JobLocationFullAddress { get; set; }

        [StringLength(JobConsts.MaxZipCodeLength, MinimumLength = JobConsts.MinZipCodeLength)]
        public string ZipCode { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? HireByDate { get; set; }

        public DateTime? PublishDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string InternalJobDescription { get; set; }

        [StringLength(JobConsts.MaxCityLocationLength, MinimumLength = JobConsts.MinCityLocationLength)]
        public string CityLocation { get; set; }

        public bool Published { get; set; }

        public string Url { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

        public long? ProductCategoryId { get; set; }

        public long? CurrencyId { get; set; }

        public long? BusinessId { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }

        public long? JobStatusTypeId { get; set; }

        public long? StoreId { get; set; }

        public bool? ProjectOrJob { get; set; }

    }
}