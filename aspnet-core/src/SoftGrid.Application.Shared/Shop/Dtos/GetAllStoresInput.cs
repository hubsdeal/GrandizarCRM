using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoresInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string StoreUrlFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string MetaTagFilter { get; set; }

        public string MetaDescriptionFilter { get; set; }

        public string FullAddressFilter { get; set; }

        public string AddressFilter { get; set; }

        public string CityFilter { get; set; }

        public decimal? MaxLatitudeFilter { get; set; }
        public decimal? MinLatitudeFilter { get; set; }

        public decimal? MaxLongitudeFilter { get; set; }
        public decimal? MinLongitudeFilter { get; set; }

        public string PhoneFilter { get; set; }

        public string MobileFilter { get; set; }

        public string EmailFilter { get; set; }

        public int? IsPublishedFilter { get; set; }

        public string FacebookFilter { get; set; }

        public string InstagramFilter { get; set; }

        public string LinkedInFilter { get; set; }

        public string YoutubeFilter { get; set; }

        public string FaxFilter { get; set; }

        public string ZipCodeFilter { get; set; }

        public string WebsiteFilter { get; set; }

        public string YearOfEstablishmentFilter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public int? MaxScoreFilter { get; set; }
        public int? MinScoreFilter { get; set; }

        public string LegalNameFilter { get; set; }

        public int? IsLocalOrOnlineStoreFilter { get; set; }

        public int? IsVerifiedFilter { get; set; }

        public string MediaLibraryNameFilter { get; set; }

        public string CountryNameFilter { get; set; }

        public string StateNameFilter { get; set; }

        public string RatingLikeNameFilter { get; set; }

        public string MasterTagNameFilter { get; set; }

    }
}