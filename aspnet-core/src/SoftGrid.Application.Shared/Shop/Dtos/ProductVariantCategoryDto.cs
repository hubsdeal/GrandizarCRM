using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductVariantCategoryDto : EntityDto<long>
    {
        public string Name { get; set; }

        public long? StoreId { get; set; }

    }
}