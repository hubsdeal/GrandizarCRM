using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class GetHubForEditOutput
    {
        public CreateOrEditHubDto Hub { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string CityName { get; set; }

        public string CountyName { get; set; }

        public string HubTypeName { get; set; }

        public string CurrencyName { get; set; }

        public string MediaLibraryName { get; set; }

    }
}