using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.Shop;
using SoftGrid.LookupData;
using SoftGrid.CRM;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.JobManagement;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.JobManagement
{
    [Table("Jobs")]
    public class Job : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(JobConsts.MaxTitleLength, MinimumLength = JobConsts.MinTitleLength)]
        public virtual string Title { get; set; }

        public virtual bool FullTimeJobOrGigWorkProject { get; set; }

        public virtual bool RemoteWorkOrOnSiteWork { get; set; }

        public virtual bool SalaryBasedOrFixedPrice { get; set; }

        [StringLength(JobConsts.MaxSalaryOrStaffingRateLength, MinimumLength = JobConsts.MinSalaryOrStaffingRateLength)]
        public virtual string SalaryOrStaffingRate { get; set; }

        [StringLength(JobConsts.MaxReferralPointsLength, MinimumLength = JobConsts.MinReferralPointsLength)]
        public virtual string ReferralPoints { get; set; }

        public virtual bool Template { get; set; }

        public virtual int? NumberOfJobs { get; set; }

        [StringLength(JobConsts.MaxMinimumExperienceLength, MinimumLength = JobConsts.MinMinimumExperienceLength)]
        public virtual string MinimumExperience { get; set; }

        [StringLength(JobConsts.MaxMaximumExperienceLength, MinimumLength = JobConsts.MinMaximumExperienceLength)]
        public virtual string MaximumExperience { get; set; }

        public virtual string JobDescription { get; set; }

        [StringLength(JobConsts.MaxJobLocationFullAddressLength, MinimumLength = JobConsts.MinJobLocationFullAddressLength)]
        public virtual string JobLocationFullAddress { get; set; }

        [StringLength(JobConsts.MaxZipCodeLength, MinimumLength = JobConsts.MinZipCodeLength)]
        public virtual string ZipCode { get; set; }

        public virtual decimal? Latitude { get; set; }

        public virtual decimal? Longitude { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? HireByDate { get; set; }

        public virtual DateTime? PublishDate { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }

        public virtual string InternalJobDescription { get; set; }

        [StringLength(JobConsts.MaxCityLocationLength, MinimumLength = JobConsts.MinCityLocationLength)]
        public virtual string CityLocation { get; set; }

        public virtual bool Published { get; set; }

        public virtual string Url { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

        public virtual long? MasterTagId { get; set; }

        [ForeignKey("MasterTagId")]
        public MasterTag MasterTagFk { get; set; }

        public virtual long? ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategoryFk { get; set; }

        public virtual long? CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency CurrencyFk { get; set; }

        public virtual long? BusinessId { get; set; }

        [ForeignKey("BusinessId")]
        public Business BusinessFk { get; set; }

        public virtual long? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual long? StateId { get; set; }

        [ForeignKey("StateId")]
        public State StateFk { get; set; }

        public virtual long? CityId { get; set; }

        [ForeignKey("CityId")]
        public City CityFk { get; set; }

        public virtual long? JobStatusTypeId { get; set; }

        [ForeignKey("JobStatusTypeId")]
        public JobStatusType JobStatusTypeFk { get; set; }

        public virtual long? StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

    }
}