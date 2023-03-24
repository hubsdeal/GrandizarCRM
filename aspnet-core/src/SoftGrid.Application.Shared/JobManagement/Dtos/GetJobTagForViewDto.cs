namespace SoftGrid.JobManagement.Dtos
{
    public class GetJobTagForViewDto
    {
        public JobTagDto JobTag { get; set; }

        public string JobTitle { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}