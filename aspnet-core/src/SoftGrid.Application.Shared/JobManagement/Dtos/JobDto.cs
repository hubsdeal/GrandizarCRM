using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.JobManagement.Dtos
{
    public class JobDto : EntityDto<long>
    {
        public string Title { get; set; }

        public bool FullTimeJobOrGigWorkProject { get; set; }

        public bool RemoteWorkOrOnSiteWork { get; set; }

        public bool SalaryBasedOrFixedPrice { get; set; }

        public string SalaryOrStaffingRate { get; set; }

        public string ReferralPoints { get; set; }

        public bool Template { get; set; }

        public int? NumberOfJobs { get; set; }

        public string MinimumExperience { get; set; }

        public string MaximumExperience { get; set; }

        public string JobDescription { get; set; }

        public string JobLocationFullAddress { get; set; }

        public string ZipCode { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? HireByDate { get; set; }

        public DateTime? PublishDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string InternalJobDescription { get; set; }

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