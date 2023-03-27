namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetLeadForViewDto
    {
        public LeadDto Lead { get; set; }

        public string ContactFullName { get; set; }

        public string BusinessName { get; set; }

        public string ProductName { get; set; }

        public string ProductCategoryName { get; set; }

        public string StoreName { get; set; }

        public string EmployeeName { get; set; }

        public string LeadSourceName { get; set; }

        public string LeadPipelineStageName { get; set; }

        public string HubName { get; set; }

    }
}