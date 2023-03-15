using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class CountyDto : EntityDto<long>
    {
        public string Name { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

    }
}