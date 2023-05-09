namespace SoftGrid.Shop.Dtos
{
    public class GetShoppingCartForViewDto
    {
        public ShoppingCartDto ShoppingCart { get; set; }

        public string ContactFullName { get; set; }

        public string OrderInvoiceNumber { get; set; }

        public string StoreName { get; set; }

        public string ProductName { get; set; }

        public string CurrencyName { get; set; }

    }
}