using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class CurrencyDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public string Icon { get; set; }

    }
}