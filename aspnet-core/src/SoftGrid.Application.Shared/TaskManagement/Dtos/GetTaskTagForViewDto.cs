namespace SoftGrid.TaskManagement.Dtos
{
    public class GetTaskTagForViewDto
    {
        public TaskTagDto TaskTag { get; set; }

        public string TaskEventName { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}