using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductSubscriptionMapsForExcelInput
    {
        public string Filter { get; set; }

        public double? MaxDiscountPercentageFilter { get; set; }
        public double? MinDiscountPercentageFilter { get; set; }

        public double? MaxDiscountAmountFilter { get; set; }
        public double? MinDiscountAmountFilter { get; set; }

        public double? MaxPriceFilter { get; set; }
        public double? MinPriceFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string SubscriptionTypeNameFilter { get; set; }

    }
}