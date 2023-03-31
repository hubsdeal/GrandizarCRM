namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderfulfillmentTeamForViewDto
    {
        public OrderfulfillmentTeamDto OrderfulfillmentTeam { get; set; }

        public string OrderFullName { get; set; }

        public string EmployeeName { get; set; }

        public string ContactFullName { get; set; }

        public string UserName { get; set; }

    }
}