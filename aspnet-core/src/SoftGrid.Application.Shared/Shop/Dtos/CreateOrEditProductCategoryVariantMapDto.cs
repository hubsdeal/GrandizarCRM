using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductCategoryVariantMapDto : EntityDto<long?>
    {

        public long ProductCategoryId { get; set; }

        public long ProductVariantCategoryId { get; set; }

    }
}