namespace SoftGrid.TaskManagement.Dtos
{
    public class GetStoreTaskMapForViewDto
    {
        public StoreTaskMapDto StoreTaskMap { get; set; }

        public string StoreName { get; set; }

        public string TaskEventName { get; set; }

    }
}