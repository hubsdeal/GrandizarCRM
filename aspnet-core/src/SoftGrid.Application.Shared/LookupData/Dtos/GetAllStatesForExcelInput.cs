using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.LookupData.Dtos
{
    public class GetAllStatesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string TickerFilter { get; set; }

        public string CountryNameFilter { get; set; }

    }
}