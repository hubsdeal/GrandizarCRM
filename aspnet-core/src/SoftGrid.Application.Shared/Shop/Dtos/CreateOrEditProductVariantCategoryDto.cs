using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductVariantCategoryDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ProductVariantCategoryConsts.MaxNameLength, MinimumLength = ProductVariantCategoryConsts.MinNameLength)]
        public string Name { get; set; }

        public long? StoreId { get; set; }

    }
}