using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductSubscriptionMapDto : EntityDto<long>
    {
        public double? DiscountPercentage { get; set; }

        public double? DiscountAmount { get; set; }

        public double? Price { get; set; }

        public long? ProductId { get; set; }

        public long? SubscriptionTypeId { get; set; }

    }
}