using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("Stores")]
    public class Store : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(StoreConsts.MaxNameLength, MinimumLength = StoreConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(StoreConsts.MaxStoreUrlLength, MinimumLength = StoreConsts.MinStoreUrlLength)]
        public virtual string StoreUrl { get; set; }

        public virtual string Description { get; set; }

        [StringLength(StoreConsts.MaxMetaTagLength, MinimumLength = StoreConsts.MinMetaTagLength)]
        public virtual string MetaTag { get; set; }

        public virtual string MetaDescription { get; set; }

        [StringLength(StoreConsts.MaxFullAddressLength, MinimumLength = StoreConsts.MinFullAddressLength)]
        public virtual string FullAddress { get; set; }

        [StringLength(StoreConsts.MaxAddressLength, MinimumLength = StoreConsts.MinAddressLength)]
        public virtual string Address { get; set; }

        [StringLength(StoreConsts.MaxCityLength, MinimumLength = StoreConsts.MinCityLength)]
        public virtual string City { get; set; }

        public virtual decimal? Latitude { get; set; }

        public virtual decimal? Longitude { get; set; }

        [StringLength(StoreConsts.MaxPhoneLength, MinimumLength = StoreConsts.MinPhoneLength)]
        public virtual string Phone { get; set; }

        [StringLength(StoreConsts.MaxMobileLength, MinimumLength = StoreConsts.MinMobileLength)]
        public virtual string Mobile { get; set; }

        [StringLength(StoreConsts.MaxEmailLength, MinimumLength = StoreConsts.MinEmailLength)]
        public virtual string Email { get; set; }

        public virtual bool IsPublished { get; set; }

        [StringLength(StoreConsts.MaxFacebookLength, MinimumLength = StoreConsts.MinFacebookLength)]
        public virtual string Facebook { get; set; }

        [StringLength(StoreConsts.MaxInstagramLength, MinimumLength = StoreConsts.MinInstagramLength)]
        public virtual string Instagram { get; set; }

        [StringLength(StoreConsts.MaxLinkedInLength, MinimumLength = StoreConsts.MinLinkedInLength)]
        public virtual string LinkedIn { get; set; }

        [StringLength(StoreConsts.MaxYoutubeLength, MinimumLength = StoreConsts.MinYoutubeLength)]
        public virtual string Youtube { get; set; }

        [StringLength(StoreConsts.MaxFaxLength, MinimumLength = StoreConsts.MinFaxLength)]
        public virtual string Fax { get; set; }

        [StringLength(StoreConsts.MaxZipCodeLength, MinimumLength = StoreConsts.MinZipCodeLength)]
        public virtual string ZipCode { get; set; }

        [StringLength(StoreConsts.MaxWebsiteLength, MinimumLength = StoreConsts.MinWebsiteLength)]
        public virtual string Website { get; set; }

        [StringLength(StoreConsts.MaxYearOfEstablishmentLength, MinimumLength = StoreConsts.MinYearOfEstablishmentLength)]
        public virtual string YearOfEstablishment { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual int? Score { get; set; }

        [StringLength(StoreConsts.MaxLegalNameLength, MinimumLength = StoreConsts.MinLegalNameLength)]
        public virtual string LegalName { get; set; }

        public virtual bool IsLocalOrOnlineStore { get; set; }

        public virtual bool IsVerified { get; set; }

        public virtual long? LogoMediaLibraryId { get; set; }

        [ForeignKey("LogoMediaLibraryId")]
        public MediaLibrary LogoMediaLibraryFk { get; set; }

        public virtual long? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual long? StateId { get; set; }

        [ForeignKey("StateId")]
        public State StateFk { get; set; }

        public virtual long? RatingLikeId { get; set; }

        [ForeignKey("RatingLikeId")]
        public RatingLike RatingLikeFk { get; set; }

        public virtual long? StoreCategoryId { get; set; }

        [ForeignKey("StoreCategoryId")]
        public MasterTag StoreCategoryFk { get; set; }

    }
}