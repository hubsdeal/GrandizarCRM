using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductVariantDto : EntityDto<long>
    {
        public string Name { get; set; }

        public long? ProductVariantCategoryId { get; set; }

    }
}