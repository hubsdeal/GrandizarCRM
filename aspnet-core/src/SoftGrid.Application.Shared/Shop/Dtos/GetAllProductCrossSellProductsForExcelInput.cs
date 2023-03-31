using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductCrossSellProductsForExcelInput
    {
        public string Filter { get; set; }

        public long? MaxCrossProductIdFilter { get; set; }
        public long? MinCrossProductIdFilter { get; set; }

        public int? MaxCrossSellScoreFilter { get; set; }
        public int? MinCrossSellScoreFilter { get; set; }

        public string ProductNameFilter { get; set; }

    }
}