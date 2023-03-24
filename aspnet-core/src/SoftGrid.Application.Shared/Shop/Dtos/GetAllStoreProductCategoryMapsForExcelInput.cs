using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreProductCategoryMapsForExcelInput
    {
        public string Filter { get; set; }

        public int? PublishedFilter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string ProductCategoryNameFilter { get; set; }

    }
}