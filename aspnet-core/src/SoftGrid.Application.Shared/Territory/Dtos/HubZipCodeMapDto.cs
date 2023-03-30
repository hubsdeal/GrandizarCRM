using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Territory.Dtos
{
    public class HubZipCodeMapDto : EntityDto<long>
    {
        public string CityName { get; set; }

        public string ZipCode { get; set; }

        public long HubId { get; set; }

        public long? CityId { get; set; }

        public long? ZipCodeId { get; set; }

    }
}