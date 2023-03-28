namespace SoftGrid.Territory.Dtos
{
    public class GetHubSalesProjectionForViewDto
    {
        public HubSalesProjectionDto HubSalesProjection { get; set; }

        public string HubName { get; set; }

        public string ProductCategoryName { get; set; }

        public string StoreName { get; set; }

        public string CurrencyName { get; set; }

    }
}