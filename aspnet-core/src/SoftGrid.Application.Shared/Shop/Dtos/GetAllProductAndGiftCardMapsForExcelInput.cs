using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductAndGiftCardMapsForExcelInput
    {
        public string Filter { get; set; }

        public double? MaxPurchaseAmountFilter { get; set; }
        public double? MinPurchaseAmountFilter { get; set; }

        public double? MaxGiftAmountFilter { get; set; }
        public double? MinGiftAmountFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string CurrencyNameFilter { get; set; }

    }
}