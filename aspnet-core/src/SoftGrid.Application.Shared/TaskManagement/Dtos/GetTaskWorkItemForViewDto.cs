namespace SoftGrid.TaskManagement.Dtos
{
    public class GetTaskWorkItemForViewDto
    {
        public TaskWorkItemDto TaskWorkItem { get; set; }

        public string TaskEventName { get; set; }

        public string EmployeeName { get; set; }

    }
}