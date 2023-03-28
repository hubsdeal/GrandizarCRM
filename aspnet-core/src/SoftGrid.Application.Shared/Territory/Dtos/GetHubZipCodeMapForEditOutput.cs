using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class GetHubZipCodeMapForEditOutput
    {
        public CreateOrEditHubZipCodeMapDto HubZipCodeMap { get; set; }

        public string HubName { get; set; }

        public string CityName { get; set; }

        public string ZipCodeName { get; set; }

    }
}