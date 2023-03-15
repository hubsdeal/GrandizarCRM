using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("ZipCodes")]
    public class ZipCode : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ZipCodeConsts.MaxNameLength, MinimumLength = ZipCodeConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string AreaCode { get; set; }

        public virtual string AsianPopulation { get; set; }

        public virtual string AverageHouseValue { get; set; }

        public virtual string BlackPopulation { get; set; }

        public virtual string CBSA { get; set; }

        public virtual string CBSA_Div { get; set; }

        public virtual string CBSA_Div_Name { get; set; }

        public virtual string CBSA_Name { get; set; }

        public virtual string CBSA_Type { get; set; }

        public virtual string CSA { get; set; }

        public virtual string CSAName { get; set; }

        public virtual string CarrierRouteRateSortation { get; set; }

        public virtual string City { get; set; }

        public virtual string CityAliasCode { get; set; }

        public virtual string CityAliasMixedCase { get; set; }

        public virtual string CityAliasName { get; set; }

        public virtual string CityDeliveryIndicator { get; set; }

        public virtual string CityMixedCase { get; set; }

        public virtual string CityStateKey { get; set; }

        public virtual string CityType { get; set; }

        public virtual string ClassificationCode { get; set; }

        public virtual string County { get; set; }

        public virtual string CountyANSI { get; set; }

        public virtual string CountyFIPS { get; set; }

        public virtual string CountyMixedCase { get; set; }

        public virtual string DayLightSaving { get; set; }

        public virtual string Division { get; set; }

        public virtual string Elevation { get; set; }

        public virtual string FacilityCode { get; set; }

        public virtual string FemalePopulation { get; set; }

        public virtual string FinanceNumber { get; set; }

        public virtual string HawaiianPopulation { get; set; }

        public virtual string HispanicPopulation { get; set; }

        public virtual string HouseholdsPerZipCode { get; set; }

        public virtual string IncomePerHousehold { get; set; }

        public virtual string IndianPopulation { get; set; }

        public virtual string Latitude { get; set; }

        public virtual string Longitude { get; set; }

        public virtual string MSA { get; set; }

        public virtual string MSA_Name { get; set; }

        public virtual string MailingName { get; set; }

        public virtual string MalePopulation { get; set; }

        public virtual string MultiCounty { get; set; }

        public virtual string OtherPopulation { get; set; }

        public virtual string PMSA { get; set; }

        public virtual string PMSA_Name { get; set; }

        public virtual string PersonsPerHousehold { get; set; }

        public virtual string Population { get; set; }

        public virtual string PreferredLastLineKey { get; set; }

        public virtual string PrimaryRecord { get; set; }

        public virtual string Region { get; set; }

        public virtual string State { get; set; }

        public virtual string StateANSI { get; set; }

        public virtual string StateFIPS { get; set; }

        public virtual string StateFullName { get; set; }

        public virtual string TimeZone { get; set; }

        public virtual string UniqueZIPName { get; set; }

        public virtual string WhitePopulation { get; set; }

        public virtual long? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual long? StateId { get; set; }

        [ForeignKey("StateId")]
        public State StateFk { get; set; }

        public virtual long? CityId { get; set; }

        [ForeignKey("CityId")]
        public City CityFk { get; set; }

        public virtual long? CountyId { get; set; }

        [ForeignKey("CountyId")]
        public County CountyFk { get; set; }

    }
}