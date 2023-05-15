namespace SoftGrid.Shop.Dtos
{
    public class GetStoreMediaForViewDto
    {
        public StoreMediaDto StoreMedia { get; set; }

        public string StoreName { get; set; }

        public string MediaLibraryName { get; set; }

        public string Picture { get; set; }

        public string VideoUrl { get; set; }

    }
}