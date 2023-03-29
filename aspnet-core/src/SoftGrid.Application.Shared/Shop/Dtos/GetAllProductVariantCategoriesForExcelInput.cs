using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductVariantCategoriesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string StoreNameFilter { get; set; }

    }
}