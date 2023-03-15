using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Territory.Dtos
{
    public class GetAllHubsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? MaxEstimatedPopulationFilter { get; set; }
        public int? MinEstimatedPopulationFilter { get; set; }

        public int? HasParentHubFilter { get; set; }

        public long? MaxParentHubIdFilter { get; set; }
        public long? MinParentHubIdFilter { get; set; }

        public decimal? MaxLatitudeFilter { get; set; }
        public decimal? MinLatitudeFilter { get; set; }

        public decimal? MaxLongitudeFilter { get; set; }
        public decimal? MinLongitudeFilter { get; set; }

        public int? LiveFilter { get; set; }

        public string UrlFilter { get; set; }

        public string OfficeFullAddressFilter { get; set; }

        public int? PartnerOrOwnedFilter { get; set; }

        public Guid? PictureIdFilter { get; set; }

        public string PhoneFilter { get; set; }

        public string YearlyRevenueFilter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public string CountryNameFilter { get; set; }

        public string StateNameFilter { get; set; }

        public string CityNameFilter { get; set; }

        public string CountyNameFilter { get; set; }

        public string HubTypeNameFilter { get; set; }

        public string CurrencyNameFilter { get; set; }

    }
}