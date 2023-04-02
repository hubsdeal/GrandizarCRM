using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.OrderManagement.Dtos
{
    public class OrderProductVariantDto : EntityDto<long>
    {
        public double? Price { get; set; }

        public long? OrderProductInfoId { get; set; }

        public long? ProductVariantCategoryId { get; set; }

        public long? ProductVariantId { get; set; }

    }
}