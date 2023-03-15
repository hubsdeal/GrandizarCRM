using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditZipCodeDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ZipCodeConsts.MaxNameLength, MinimumLength = ZipCodeConsts.MinNameLength)]
        public string Name { get; set; }

        public string AreaCode { get; set; }

        public string AsianPopulation { get; set; }

        public string AverageHouseValue { get; set; }

        public string BlackPopulation { get; set; }

        public string CBSA { get; set; }

        public string CBSA_Div { get; set; }

        public string CBSA_Div_Name { get; set; }

        public string CBSA_Name { get; set; }

        public string CBSA_Type { get; set; }

        public string CSA { get; set; }

        public string CSAName { get; set; }

        public string CarrierRouteRateSortation { get; set; }

        public string City { get; set; }

        public string CityAliasCode { get; set; }

        public string CityAliasMixedCase { get; set; }

        public string CityAliasName { get; set; }

        public string CityDeliveryIndicator { get; set; }

        public string CityMixedCase { get; set; }

        public string CityStateKey { get; set; }

        public string CityType { get; set; }

        public string ClassificationCode { get; set; }

        public string County { get; set; }

        public string CountyANSI { get; set; }

        public string CountyFIPS { get; set; }

        public string CountyMixedCase { get; set; }

        public string DayLightSaving { get; set; }

        public string Division { get; set; }

        public string Elevation { get; set; }

        public string FacilityCode { get; set; }

        public string FemalePopulation { get; set; }

        public string FinanceNumber { get; set; }

        public string HawaiianPopulation { get; set; }

        public string HispanicPopulation { get; set; }

        public string HouseholdsPerZipCode { get; set; }

        public string IncomePerHousehold { get; set; }

        public string IndianPopulation { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string MSA { get; set; }

        public string MSA_Name { get; set; }

        public string MailingName { get; set; }

        public string MalePopulation { get; set; }

        public string MultiCounty { get; set; }

        public string OtherPopulation { get; set; }

        public string PMSA { get; set; }

        public string PMSA_Name { get; set; }

        public string PersonsPerHousehold { get; set; }

        public string Population { get; set; }

        public string PreferredLastLineKey { get; set; }

        public string PrimaryRecord { get; set; }

        public string Region { get; set; }

        public string State { get; set; }

        public string StateANSI { get; set; }

        public string StateFIPS { get; set; }

        public string StateFullName { get; set; }

        public string TimeZone { get; set; }

        public string UniqueZIPName { get; set; }

        public string WhitePopulation { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }

        public long? CountyId { get; set; }

    }
}