using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductByVariantForEditOutput
    {
        public CreateOrEditProductByVariantDto ProductByVariant { get; set; }

        public string ProductName { get; set; }

        public string ProductVariantName { get; set; }

        public string ProductVariantCategoryName { get; set; }

        public string MediaLibraryName { get; set; }

    }
}