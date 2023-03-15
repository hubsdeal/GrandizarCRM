namespace SoftGrid.LookupData.Dtos
{
    public class GetZipCodeForViewDto
    {
        public ZipCodeDto ZipCode { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string CityName { get; set; }

        public string CountyName { get; set; }

    }
}