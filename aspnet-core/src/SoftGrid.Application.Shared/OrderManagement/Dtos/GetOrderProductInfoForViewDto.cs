namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderProductInfoForViewDto
    {
        public OrderProductInfoDto OrderProductInfo { get; set; }

        public string OrderInvoiceNumber { get; set; }

        public string StoreName { get; set; }

        public string ProductName { get; set; }

        public string MeasurementUnitName { get; set; }

    }
}