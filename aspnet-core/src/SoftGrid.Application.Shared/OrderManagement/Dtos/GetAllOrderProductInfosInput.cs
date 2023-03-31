using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetAllOrderProductInfosInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxQuantityFilter { get; set; }
        public int? MinQuantityFilter { get; set; }

        public double? MaxUnitPriceFilter { get; set; }
        public double? MinUnitPriceFilter { get; set; }

        public double? MaxByProductDiscountAmountFilter { get; set; }
        public double? MinByProductDiscountAmountFilter { get; set; }

        public double? MaxByProductDiscountPercentageFilter { get; set; }
        public double? MinByProductDiscountPercentageFilter { get; set; }

        public double? MaxByProductTaxAmountFilter { get; set; }
        public double? MinByProductTaxAmountFilter { get; set; }

        public double? MaxByProductTotalAmountFilter { get; set; }
        public double? MinByProductTotalAmountFilter { get; set; }

        public string OrderInvoiceNumberFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string MeasurementUnitNameFilter { get; set; }

    }
}