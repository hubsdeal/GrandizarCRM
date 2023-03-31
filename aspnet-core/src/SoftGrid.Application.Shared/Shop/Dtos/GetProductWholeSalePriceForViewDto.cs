namespace SoftGrid.Shop.Dtos
{
    public class GetProductWholeSalePriceForViewDto
    {
        public ProductWholeSalePriceDto ProductWholeSalePrice { get; set; }

        public string ProductName { get; set; }

        public string ProductWholeSaleQuantityTypeName { get; set; }

        public string MeasurementUnitName { get; set; }

        public string CurrencyName { get; set; }

    }
}