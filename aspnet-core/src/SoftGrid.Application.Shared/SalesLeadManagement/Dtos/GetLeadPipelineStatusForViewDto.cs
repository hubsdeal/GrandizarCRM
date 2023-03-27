namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetLeadPipelineStatusForViewDto
    {
        public LeadPipelineStatusDto LeadPipelineStatus { get; set; }

        public string LeadTitle { get; set; }

        public string LeadPipelineStageName { get; set; }

        public string EmployeeName { get; set; }

    }
}