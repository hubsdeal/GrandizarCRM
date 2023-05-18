namespace SoftGrid.TaskManagement.Dtos
{
    public class GetTaskDocumentForViewDto
    {
        public TaskDocumentDto TaskDocument { get; set; }

        public string TaskEventName { get; set; }

        public string DocumentTypeName { get; set; }

    }
}