using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductSubscriptionMapDto : EntityDto<long?>
    {

        public double? DiscountPercentage { get; set; }

        public double? DiscountAmount { get; set; }

        public double? Price { get; set; }

        public long? ProductId { get; set; }

        public long? SubscriptionTypeId { get; set; }

    }
}