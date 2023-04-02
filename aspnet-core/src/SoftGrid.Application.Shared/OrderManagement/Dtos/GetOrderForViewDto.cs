namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderForViewDto
    {
        public OrderDto Order { get; set; }

        public string StateName { get; set; }

        public string CountryName { get; set; }

        public string ContactFullName { get; set; }

        public string OrderStatusName { get; set; }

        public string CurrencyName { get; set; }

        public string StoreName { get; set; }

        public string OrderSalesChannelName { get; set; }

    }
}