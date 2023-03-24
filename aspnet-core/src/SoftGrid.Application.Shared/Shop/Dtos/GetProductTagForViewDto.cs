namespace SoftGrid.Shop.Dtos
{
    public class GetProductTagForViewDto
    {
        public ProductTagDto ProductTag { get; set; }

        public string ProductName { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}