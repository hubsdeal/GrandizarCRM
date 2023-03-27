namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetLeadTaskForViewDto
    {
        public LeadTaskDto LeadTask { get; set; }

        public string LeadTitle { get; set; }

        public string TaskEventName { get; set; }

    }
}