using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductWholeSalePricesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public double? MaxPriceFilter { get; set; }
        public double? MinPriceFilter { get; set; }

        public double? MaxExactQuantityFilter { get; set; }
        public double? MinExactQuantityFilter { get; set; }

        public string PackageInfoFilter { get; set; }

        public int? MaxPackageQuantityFilter { get; set; }
        public int? MinPackageQuantityFilter { get; set; }

        public string WholeSaleSkuIdFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string ProductWholeSaleQuantityTypeNameFilter { get; set; }

        public string MeasurementUnitNameFilter { get; set; }

        public string CurrencyNameFilter { get; set; }

    }
}