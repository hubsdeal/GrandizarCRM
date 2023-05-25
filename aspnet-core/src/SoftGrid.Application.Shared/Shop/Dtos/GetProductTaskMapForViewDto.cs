namespace SoftGrid.Shop.Dtos
{
    public class GetProductTaskMapForViewDto
    {
        public ProductTaskMapDto ProductTaskMap { get; set; }

        public string ProductName { get; set; }

        public string TaskEventName { get; set; }

        public string ProductCategoryName { get; set; }

    }
}