using System;
using System.Collections.Generic;
using System.Text;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductsBySpForView
    {
        public int TotalCount { get; set; }

        public int Published { get; set; }
        public int UnPublished { get; set; }
        public List<ProductFromSpDto> Products { get; set; }

        public GetProductsBySpForView()
        {
            Products = new List<ProductFromSpDto>();
        }

    }

    public class ProductFromSpDto : ProductDto
    {
        public string Picture { get; set; }
        public Guid? PictureId { get; set; }
        public string CategoryName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyTicker { get; set; }
        public string CurrencyIcon { get; set; }
        public string MeasurementUnitName { get; set; }
        public string StoreName { get; set; }

        public double? MembershipPrice { get; set; }
    }
}
