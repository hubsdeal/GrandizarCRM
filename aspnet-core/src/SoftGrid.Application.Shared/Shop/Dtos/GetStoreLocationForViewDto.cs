namespace SoftGrid.Shop.Dtos
{
    public class GetStoreLocationForViewDto
    {
        public StoreLocationDto StoreLocation { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string CountryName { get; set; }

        public string StoreName { get; set; }

    }
}