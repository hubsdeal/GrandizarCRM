namespace SoftGrid.Shop.Dtos
{
    public class GetStoreBusinessHourForViewDto
    {
        public StoreBusinessHourDto StoreBusinessHour { get; set; }

        public string StoreName { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}