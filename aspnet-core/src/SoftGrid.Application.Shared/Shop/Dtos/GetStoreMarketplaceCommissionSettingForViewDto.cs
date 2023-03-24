namespace SoftGrid.Shop.Dtos
{
    public class GetStoreMarketplaceCommissionSettingForViewDto
    {
        public StoreMarketplaceCommissionSettingDto StoreMarketplaceCommissionSetting { get; set; }

        public string StoreName { get; set; }

        public string MarketplaceCommissionTypeName { get; set; }

        public string ProductCategoryName { get; set; }

        public string ProductName { get; set; }

    }
}