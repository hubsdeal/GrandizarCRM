using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.CRM
{
    [Table("Businesses")]
    public class Business : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(BusinessConsts.MaxNameLength, MinimumLength = BusinessConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(BusinessConsts.MaxTradeNameLength, MinimumLength = BusinessConsts.MinTradeNameLength)]
        public virtual string TradeName { get; set; }

        public virtual string Description { get; set; }

        [StringLength(BusinessConsts.MaxCustomIdLength, MinimumLength = BusinessConsts.MinCustomIdLength)]
        public virtual string CustomId { get; set; }

        [StringLength(BusinessConsts.MaxYearOfEstablishmentLength, MinimumLength = BusinessConsts.MinYearOfEstablishmentLength)]
        public virtual string YearOfEstablishment { get; set; }

        [StringLength(BusinessConsts.MaxLocationTitleLength, MinimumLength = BusinessConsts.MinLocationTitleLength)]
        public virtual string LocationTitle { get; set; }

        [StringLength(BusinessConsts.MaxFullAddressLength, MinimumLength = BusinessConsts.MinFullAddressLength)]
        public virtual string FullAddress { get; set; }

        [StringLength(BusinessConsts.MaxAddress1Length, MinimumLength = BusinessConsts.MinAddress1Length)]
        public virtual string Address1 { get; set; }

        [StringLength(BusinessConsts.MaxAddress2Length, MinimumLength = BusinessConsts.MinAddress2Length)]
        public virtual string Address2 { get; set; }

        [StringLength(BusinessConsts.MaxCityLength, MinimumLength = BusinessConsts.MinCityLength)]
        public virtual string City { get; set; }

        [StringLength(BusinessConsts.MaxZipCodeLength, MinimumLength = BusinessConsts.MinZipCodeLength)]
        public virtual string ZipCode { get; set; }

        public virtual decimal? Latitude { get; set; }

        public virtual decimal? Longitude { get; set; }

        [StringLength(BusinessConsts.MaxPhoneLength, MinimumLength = BusinessConsts.MinPhoneLength)]
        public virtual string Phone { get; set; }

        [StringLength(BusinessConsts.MaxFaxLength, MinimumLength = BusinessConsts.MinFaxLength)]
        public virtual string Fax { get; set; }

        [StringLength(BusinessConsts.MaxEmailLength, MinimumLength = BusinessConsts.MinEmailLength)]
        public virtual string Email { get; set; }

        [StringLength(BusinessConsts.MaxWebsiteLength, MinimumLength = BusinessConsts.MinWebsiteLength)]
        public virtual string Website { get; set; }

        [StringLength(BusinessConsts.MaxEinTaxIdLength, MinimumLength = BusinessConsts.MinEinTaxIdLength)]
        public virtual string EinTaxId { get; set; }

        [StringLength(BusinessConsts.MaxIndustryLength, MinimumLength = BusinessConsts.MinIndustryLength)]
        public virtual string Industry { get; set; }

        public virtual string InternalRemarks { get; set; }

        public virtual bool Verified { get; set; }

        [StringLength(BusinessConsts.MaxFacebookLength, MinimumLength = BusinessConsts.MinFacebookLength)]
        public virtual string Facebook { get; set; }

        [StringLength(BusinessConsts.MaxLinkedInLength, MinimumLength = BusinessConsts.MinLinkedInLength)]
        public virtual string LinkedIn { get; set; }

        public virtual long? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual long? StateId { get; set; }

        [ForeignKey("StateId")]
        public State StateFk { get; set; }

        public virtual long? CityId { get; set; }

        [ForeignKey("CityId")]
        public City CityFk { get; set; }

        public virtual long? LogoMediaLibraryId { get; set; }

        [ForeignKey("LogoMediaLibraryId")]
        public MediaLibrary LogoMediaLibraryFk { get; set; }

    }
}