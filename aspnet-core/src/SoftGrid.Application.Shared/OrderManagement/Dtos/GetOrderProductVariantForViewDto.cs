namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderProductVariantForViewDto
    {
        public OrderProductVariantDto OrderProductVariant { get; set; }

        public string ProductVariantCategoryName { get; set; }

        public string ProductVariantName { get; set; }

    }
}