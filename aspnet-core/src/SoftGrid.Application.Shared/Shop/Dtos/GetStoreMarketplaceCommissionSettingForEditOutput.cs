using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreMarketplaceCommissionSettingForEditOutput
    {
        public CreateOrEditStoreMarketplaceCommissionSettingDto StoreMarketplaceCommissionSetting { get; set; }

        public string StoreName { get; set; }

        public string MarketplaceCommissionTypeName { get; set; }

        public string ProductCategoryName { get; set; }

        public string ProductName { get; set; }

    }
}