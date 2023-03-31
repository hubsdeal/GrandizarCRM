using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetAllOrderProductVariantsForExcelInput
    {
        public string Filter { get; set; }

        public double? MaxPriceFilter { get; set; }
        public double? MinPriceFilter { get; set; }

        public long? MaxOrderProductInfoIdFilter { get; set; }
        public long? MinOrderProductInfoIdFilter { get; set; }

        public string ProductVariantCategoryNameFilter { get; set; }

        public string ProductVariantNameFilter { get; set; }

    }
}