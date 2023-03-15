using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.LookupData.Dtos
{
    public class GetAllCountriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string TickerFilter { get; set; }

        public string FlagIconFilter { get; set; }

        public string PhoneCodeFilter { get; set; }

    }
}