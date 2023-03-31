namespace SoftGrid.Shop.Dtos
{
    public class GetProductByVariantForViewDto
    {
        public ProductByVariantDto ProductByVariant { get; set; }

        public string ProductName { get; set; }

        public string ProductVariantName { get; set; }

        public string ProductVariantCategoryName { get; set; }

        public string MediaLibraryName { get; set; }

    }
}