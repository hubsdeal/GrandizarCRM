using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductUpsellRelatedProductsForExcelInput
    {
        public string Filter { get; set; }

        public long? MaxRelatedProductIdFilter { get; set; }
        public long? MinRelatedProductIdFilter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public string ProductNameFilter { get; set; }

    }
}