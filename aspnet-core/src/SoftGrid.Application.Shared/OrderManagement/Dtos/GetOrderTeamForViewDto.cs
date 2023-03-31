namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderTeamForViewDto
    {
        public OrderTeamDto OrderTeam { get; set; }

        public string OrderInvoiceNumber { get; set; }

        public string EmployeeName { get; set; }

    }
}