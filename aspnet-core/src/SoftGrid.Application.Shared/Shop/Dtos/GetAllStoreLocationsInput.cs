using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreLocationsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string LocationNameFilter { get; set; }

        public string FullAddressFilter { get; set; }

        public decimal? MaxLatitudeFilter { get; set; }
        public decimal? MinLatitudeFilter { get; set; }

        public decimal? MaxLongitudeFilter { get; set; }
        public decimal? MinLongitudeFilter { get; set; }

        public string AddressFilter { get; set; }

        public string MobileFilter { get; set; }

        public string EmailFilter { get; set; }

        public string ZipCodeFilter { get; set; }

        public string CityNameFilter { get; set; }

        public string StateNameFilter { get; set; }

        public string CountryNameFilter { get; set; }

        public string StoreNameFilter { get; set; }

    }
}