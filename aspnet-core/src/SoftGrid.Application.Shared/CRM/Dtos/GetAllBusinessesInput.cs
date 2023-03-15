using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllBusinessesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string TradeNameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string CustomIdFilter { get; set; }

        public string YearOfEstablishmentFilter { get; set; }

        public string LocationTitleFilter { get; set; }

        public string FullAddressFilter { get; set; }

        public string Address1Filter { get; set; }

        public string Address2Filter { get; set; }

        public string CityFilter { get; set; }

        public string ZipCodeFilter { get; set; }

        public decimal? MaxLatitudeFilter { get; set; }
        public decimal? MinLatitudeFilter { get; set; }

        public decimal? MaxLongitudeFilter { get; set; }
        public decimal? MinLongitudeFilter { get; set; }

        public string PhoneFilter { get; set; }

        public string FaxFilter { get; set; }

        public string EmailFilter { get; set; }

        public string WebsiteFilter { get; set; }

        public string EinTaxIdFilter { get; set; }

        public string IndustryFilter { get; set; }

        public string InternalRemarksFilter { get; set; }

        public int? VerifiedFilter { get; set; }

        public string FacebookFilter { get; set; }

        public string LinkedInFilter { get; set; }

        public string CountryNameFilter { get; set; }

        public string StateNameFilter { get; set; }

        public string CityNameFilter { get; set; }

        public string MediaLibraryNameFilter { get; set; }

    }
}