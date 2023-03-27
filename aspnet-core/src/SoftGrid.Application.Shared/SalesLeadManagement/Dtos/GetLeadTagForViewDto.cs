namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetLeadTagForViewDto
    {
        public LeadTagDto LeadTag { get; set; }

        public string LeadTitle { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}