namespace SoftGrid.Shop.Dtos
{
    public class GetProductForViewDto
    {
        public ProductDto Product { get; set; }

        public string ProductCategoryName { get; set; }

        public string MediaLibraryName { get; set; }

        public string MeasurementUnitName { get; set; }

        public string CurrencyName { get; set; }

        public string RatingLikeName { get; set; }

    }
}