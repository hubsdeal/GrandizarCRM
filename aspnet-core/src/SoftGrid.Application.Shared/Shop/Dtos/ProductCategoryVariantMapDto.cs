using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductCategoryVariantMapDto : EntityDto<long>
    {

        public long ProductCategoryId { get; set; }

        public long ProductVariantCategoryId { get; set; }

    }
}