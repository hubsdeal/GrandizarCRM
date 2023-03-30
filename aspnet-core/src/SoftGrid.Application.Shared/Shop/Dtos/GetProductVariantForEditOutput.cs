using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductVariantForEditOutput
    {
        public CreateOrEditProductVariantDto ProductVariant { get; set; }

        public string ProductVariantCategoryName { get; set; }

    }
}