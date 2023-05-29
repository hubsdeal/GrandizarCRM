using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreDto : EntityDto<long?>
    {

        [Required]
        [StringLength(StoreConsts.MaxNameLength, MinimumLength = StoreConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(StoreConsts.MaxStoreUrlLength, MinimumLength = StoreConsts.MinStoreUrlLength)]
        public string StoreUrl { get; set; }

        public string Description { get; set; }

        [StringLength(StoreConsts.MaxMetaTagLength, MinimumLength = StoreConsts.MinMetaTagLength)]
        public string MetaTag { get; set; }

        public string MetaDescription { get; set; }

        [StringLength(StoreConsts.MaxFullAddressLength, MinimumLength = StoreConsts.MinFullAddressLength)]
        public string FullAddress { get; set; }

        [StringLength(StoreConsts.MaxAddressLength, MinimumLength = StoreConsts.MinAddressLength)]
        public string Address { get; set; }

        [StringLength(StoreConsts.MaxCityLength, MinimumLength = StoreConsts.MinCityLength)]
        public string City { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        [StringLength(StoreConsts.MaxPhoneLength, MinimumLength = StoreConsts.MinPhoneLength)]
        public string Phone { get; set; }

        [StringLength(StoreConsts.MaxMobileLength, MinimumLength = StoreConsts.MinMobileLength)]
        public string Mobile { get; set; }

        [StringLength(StoreConsts.MaxEmailLength, MinimumLength = StoreConsts.MinEmailLength)]
        public string Email { get; set; }

        public bool IsPublished { get; set; }

        [StringLength(StoreConsts.MaxFacebookLength, MinimumLength = StoreConsts.MinFacebookLength)]
        public string Facebook { get; set; }

        [StringLength(StoreConsts.MaxInstagramLength, MinimumLength = StoreConsts.MinInstagramLength)]
        public string Instagram { get; set; }

        [StringLength(StoreConsts.MaxLinkedInLength, MinimumLength = StoreConsts.MinLinkedInLength)]
        public string LinkedIn { get; set; }

        [StringLength(StoreConsts.MaxYoutubeLength, MinimumLength = StoreConsts.MinYoutubeLength)]
        public string Youtube { get; set; }

        [StringLength(StoreConsts.MaxFaxLength, MinimumLength = StoreConsts.MinFaxLength)]
        public string Fax { get; set; }

        [StringLength(StoreConsts.MaxZipCodeLength, MinimumLength = StoreConsts.MinZipCodeLength)]
        public string ZipCode { get; set; }

        [StringLength(StoreConsts.MaxWebsiteLength, MinimumLength = StoreConsts.MinWebsiteLength)]
        public string Website { get; set; }

        [StringLength(StoreConsts.MaxYearOfEstablishmentLength, MinimumLength = StoreConsts.MinYearOfEstablishmentLength)]
        public string YearOfEstablishment { get; set; }

        public int? DisplaySequence { get; set; }

        public int? Score { get; set; }

        [StringLength(StoreConsts.MaxLegalNameLength, MinimumLength = StoreConsts.MinLegalNameLength)]
        public string LegalName { get; set; }

        public bool IsLocalOrOnlineStore { get; set; }

        public bool IsVerified { get; set; }

        public long? LogoMediaLibraryId { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

        public long? RatingLikeId { get; set; }

        public long? StoreCategoryId { get; set; }
        public string FileToken { get; set; }

        public long? ProductCategoryId { get; set; }

        public long? PrimaryCategoryId { get; set; }

        public long? StoreTagSettingCategoryId { get; set; }

    }
}