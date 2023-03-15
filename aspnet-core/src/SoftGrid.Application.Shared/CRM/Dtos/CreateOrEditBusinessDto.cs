using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditBusinessDto : EntityDto<long?>
    {

        [Required]
        [StringLength(BusinessConsts.MaxNameLength, MinimumLength = BusinessConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(BusinessConsts.MaxTradeNameLength, MinimumLength = BusinessConsts.MinTradeNameLength)]
        public string TradeName { get; set; }

        public string Description { get; set; }

        [StringLength(BusinessConsts.MaxCustomIdLength, MinimumLength = BusinessConsts.MinCustomIdLength)]
        public string CustomId { get; set; }

        [StringLength(BusinessConsts.MaxYearOfEstablishmentLength, MinimumLength = BusinessConsts.MinYearOfEstablishmentLength)]
        public string YearOfEstablishment { get; set; }

        [StringLength(BusinessConsts.MaxLocationTitleLength, MinimumLength = BusinessConsts.MinLocationTitleLength)]
        public string LocationTitle { get; set; }

        [StringLength(BusinessConsts.MaxFullAddressLength, MinimumLength = BusinessConsts.MinFullAddressLength)]
        public string FullAddress { get; set; }

        [StringLength(BusinessConsts.MaxAddress1Length, MinimumLength = BusinessConsts.MinAddress1Length)]
        public string Address1 { get; set; }

        [StringLength(BusinessConsts.MaxAddress2Length, MinimumLength = BusinessConsts.MinAddress2Length)]
        public string Address2 { get; set; }

        [StringLength(BusinessConsts.MaxCityLength, MinimumLength = BusinessConsts.MinCityLength)]
        public string City { get; set; }

        [StringLength(BusinessConsts.MaxZipCodeLength, MinimumLength = BusinessConsts.MinZipCodeLength)]
        public string ZipCode { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        [StringLength(BusinessConsts.MaxPhoneLength, MinimumLength = BusinessConsts.MinPhoneLength)]
        public string Phone { get; set; }

        [StringLength(BusinessConsts.MaxFaxLength, MinimumLength = BusinessConsts.MinFaxLength)]
        public string Fax { get; set; }

        [StringLength(BusinessConsts.MaxEmailLength, MinimumLength = BusinessConsts.MinEmailLength)]
        public string Email { get; set; }

        [StringLength(BusinessConsts.MaxWebsiteLength, MinimumLength = BusinessConsts.MinWebsiteLength)]
        public string Website { get; set; }

        [StringLength(BusinessConsts.MaxEinTaxIdLength, MinimumLength = BusinessConsts.MinEinTaxIdLength)]
        public string EinTaxId { get; set; }

        [StringLength(BusinessConsts.MaxIndustryLength, MinimumLength = BusinessConsts.MinIndustryLength)]
        public string Industry { get; set; }

        public string InternalRemarks { get; set; }

        public bool Verified { get; set; }

        [StringLength(BusinessConsts.MaxFacebookLength, MinimumLength = BusinessConsts.MinFacebookLength)]
        public string Facebook { get; set; }

        [StringLength(BusinessConsts.MaxLinkedInLength, MinimumLength = BusinessConsts.MinLinkedInLength)]
        public string LinkedIn { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }

        public long? LogoMediaLibraryId { get; set; }

    }
}