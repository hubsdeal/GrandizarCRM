namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderDeliveryInfoForViewDto
    {
        public OrderDeliveryInfoDto OrderDeliveryInfo { get; set; }

        public string EmployeeName { get; set; }

        public string OrderInvoiceNumber { get; set; }

    }
}