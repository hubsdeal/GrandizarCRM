using SoftGrid.Shop.Dtos;

using System.Collections.Generic;

namespace SoftGrid.PublicCommon.Dtos
{
    public class GetPublicProductForViewDto : GetProductForViewDto
    {
        public decimal? RatingScore { get; set; }
        public string CurrencyIcon { get; set; } = "$";
        public string ProductCategoryUrl { get; set; }
        public long? StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreLogo { get; set; }

        public string StoreUrl { get; set; }
        public double? TotalVariantPrice { get; set; } = 0;
        public int? CartQuantity { get; set; }
        public long? ShoppingCartId { get; set; }

        public List<string> StoreTags { get; set; }
        public int NumberOfRatings { get; set; }

        public List<GetProductMediaForViewDto> ProductMedias { get; set; }

        //public List<OrderProductVariantCategory> OrderProductVariantCategories { get; set; }
        public int? DisplaySequence { get; set; }

        public List<string> ProductTags { get; set; }
        public bool IsFlashSale { get; set; }
        public double FlashSaleRemainingDuration { get; set; }

        public List<string> PickupOrDeliveryTags { get; set; }
        public bool HasVariant { get; set; }
        public List<GetPackageProductForPublicViewDto> ProductPackages { get; set; }

        public double? MembershipPrice { get; set; }
        public string MembershipName { get; set; }

        public string ShareUrl { get; set; }

        public bool HasFaq { get; set; }

        public GetPublicProductForViewDto()
        {
            ProductMedias = new List<GetProductMediaForViewDto>();
            //OrderProductVariantCategories = new List<OrderProductVariantCategory>();
            ProductTags = new List<string>();
            PickupOrDeliveryTags = new List<string>();
            StoreTags = new List<string>();
            ProductPackages = new List<GetPackageProductForPublicViewDto>();
        }
    }

    public class GetPublicProductWidgetWithProductsForViewDto
    {
        public StoreDynamicWidgetMapDto StoreDynamicWidgetMap { get; set; }
        public List<GetPublicProductForViewDto> Products { get; set; }

        public GetPublicProductWidgetWithProductsForViewDto()
        {
            Products = new List<GetPublicProductForViewDto>();
        }
    }

    public class GetPublicCategoryWiseProductsForViewDto
    {
        public List<GetPublicStoreCategoriesForViewDto> Categories { get; set; }
        public GetPublicCategoryWiseProductsForViewDto()
        {
            Categories = new List<GetPublicStoreCategoriesForViewDto>();
        }
    }

    public class GetPublicStoreCategoriesForViewDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public int? DisplaySequence { get; set; }
        public int NumberOfProducts { get; set; }
        public List<GetPublicProductForViewDto> Products { get; set; }

        public GetPublicStoreCategoriesForViewDto()
        {
            Products = new List<GetPublicProductForViewDto>();

        }
    }

    public class GetRandomProductByHub
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
    }

    public class GetAllProductsForPublicProductsBySp
    {
        public int TotalCount { get; set; }
        public List<GetPublicProductForViewDto> Products { get; set; }
    }


    public class GetPackageProductForPublicViewDto
    {
        public long? ProductId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public int? Quantity { get; set; }
    }

}
