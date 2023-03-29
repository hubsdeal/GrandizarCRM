using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductVariantCategoryForEditOutput
    {
        public CreateOrEditProductVariantCategoryDto ProductVariantCategory { get; set; }

        public string StoreName { get; set; }

    }
}