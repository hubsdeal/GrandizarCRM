namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderPaymentInfoForViewDto
    {
        public OrderPaymentInfoDto OrderPaymentInfo { get; set; }

        public string OrderInvoiceNumber { get; set; }

        public string CurrencyName { get; set; }

        public string PaymentTypeName { get; set; }

    }
}