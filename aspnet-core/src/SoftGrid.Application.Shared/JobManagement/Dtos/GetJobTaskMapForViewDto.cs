namespace SoftGrid.JobManagement.Dtos
{
    public class GetJobTaskMapForViewDto
    {
        public JobTaskMapDto JobTaskMap { get; set; }

        public string JobTitle { get; set; }

        public string TaskEventName { get; set; }

    }
}