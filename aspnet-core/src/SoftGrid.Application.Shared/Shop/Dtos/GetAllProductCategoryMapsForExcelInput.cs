using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductCategoryMapsForExcelInput
    {
        public string Filter { get; set; }

        public string ProductNameFilter { get; set; }

        public string ProductCategoryNameFilter { get; set; }

    }
}