using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string StoreUrl { get; set; }

        public string Description { get; set; }

        public string MetaTag { get; set; }

        public string MetaDescription { get; set; }

        public string FullAddress { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public bool IsPublished { get; set; }

        public string Facebook { get; set; }

        public string Instagram { get; set; }

        public string LinkedIn { get; set; }

        public string Youtube { get; set; }

        public string Fax { get; set; }

        public string ZipCode { get; set; }

        public string Website { get; set; }

        public string YearOfEstablishment { get; set; }

        public int? DisplaySequence { get; set; }

        public int? Score { get; set; }

        public string LegalName { get; set; }

        public bool IsLocalOrOnlineStore { get; set; }

        public bool IsVerified { get; set; }

        public long? LogoMediaLibraryId { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

        public long? RatingLikeId { get; set; }

        public long? StoreCategoryId { get; set; }

    }
}