using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreMarketplaceCommissionSettingsForExcelInput
    {
        public string Filter { get; set; }

        public double? MaxPercentageFilter { get; set; }
        public double? MinPercentageFilter { get; set; }

        public double? MaxFixedAmountFilter { get; set; }
        public double? MinFixedAmountFilter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string MarketplaceCommissionTypeNameFilter { get; set; }

        public string ProductCategoryNameFilter { get; set; }

        public string ProductNameFilter { get; set; }

    }
}