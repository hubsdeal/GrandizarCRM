using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductVariantDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ProductVariantConsts.MaxNameLength, MinimumLength = ProductVariantConsts.MinNameLength)]
        public string Name { get; set; }

        public long? ProductVariantCategoryId { get; set; }

    }
}