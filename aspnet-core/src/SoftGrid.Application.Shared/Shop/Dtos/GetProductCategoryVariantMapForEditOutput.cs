using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductCategoryVariantMapForEditOutput
    {
        public CreateOrEditProductCategoryVariantMapDto ProductCategoryVariantMap { get; set; }

        public string ProductCategoryName { get; set; }

        public string ProductVariantCategoryName { get; set; }

    }
}