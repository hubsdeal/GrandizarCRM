using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class CountryDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public string FlagIcon { get; set; }

        public string PhoneCode { get; set; }

    }
}