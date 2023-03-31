namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderFulfillmentStatusForViewDto
    {
        public OrderFulfillmentStatusDto OrderFulfillmentStatus { get; set; }

        public string OrderStatusName { get; set; }

        public string OrderInvoiceNumber { get; set; }

        public string EmployeeName { get; set; }

    }
}