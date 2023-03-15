using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class StateDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public long CountryId { get; set; }

    }
}