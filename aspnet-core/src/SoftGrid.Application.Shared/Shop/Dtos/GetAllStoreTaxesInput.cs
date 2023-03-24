using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreTaxesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TaxNameFilter { get; set; }

        public int? PercentageOrAmountFilter { get; set; }

        public double? MaxTaxRatePercentageFilter { get; set; }
        public double? MinTaxRatePercentageFilter { get; set; }

        public double? MaxTaxAmountFilter { get; set; }
        public double? MinTaxAmountFilter { get; set; }

        public string StoreNameFilter { get; set; }

    }
}