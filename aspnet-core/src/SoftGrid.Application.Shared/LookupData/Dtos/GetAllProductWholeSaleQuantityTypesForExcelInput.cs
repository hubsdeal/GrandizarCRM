using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.LookupData.Dtos
{
    public class GetAllProductWholeSaleQuantityTypesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? MaxMinQtyFilter { get; set; }
        public int? MinMinQtyFilter { get; set; }

        public int? MaxMaxQtyFilter { get; set; }
        public int? MinMaxQtyFilter { get; set; }

    }
}