namespace SoftGrid.Shop.Dtos
{
    public class GetStoreTagForViewDto
    {
        public StoreTagDto StoreTag { get; set; }

        public string StoreName { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}