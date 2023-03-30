using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class CreateOrEditHubZipCodeMapDto : EntityDto<long?>
    {

        [StringLength(HubZipCodeMapConsts.MaxCityNameLength, MinimumLength = HubZipCodeMapConsts.MinCityNameLength)]
        public string CityName { get; set; }

        [StringLength(HubZipCodeMapConsts.MaxZipCodeLength, MinimumLength = HubZipCodeMapConsts.MinZipCodeLength)]
        public string ZipCode { get; set; }

        public long HubId { get; set; }

        public long? CityId { get; set; }

        public long? ZipCodeId { get; set; }

    }
}