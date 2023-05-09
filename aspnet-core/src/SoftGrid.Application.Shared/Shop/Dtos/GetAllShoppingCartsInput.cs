using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllShoppingCartsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxQuantityFilter { get; set; }
        public int? MinQuantityFilter { get; set; }

        public double? MaxUnitPriceFilter { get; set; }
        public double? MinUnitPriceFilter { get; set; }

        public double? MaxTotalAmountFilter { get; set; }
        public double? MinTotalAmountFilter { get; set; }

        public double? MaxUnitTotalPriceFilter { get; set; }
        public double? MinUnitTotalPriceFilter { get; set; }

        public double? MaxUnitDiscountAmountFilter { get; set; }
        public double? MinUnitDiscountAmountFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string OrderInvoiceNumberFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string CurrencyNameFilter { get; set; }

    }
}