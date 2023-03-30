using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Territory.Dtos
{
    public class GetAllHubZipCodeMapsForExcelInput
    {
        public string Filter { get; set; }

        public string CityNameFilter { get; set; }

        public string ZipCodeFilter { get; set; }

        public string HubNameFilter { get; set; }

        //public string CityNameFilter { get; set; }

        public string ZipCodeNameFilter { get; set; }

    }
}