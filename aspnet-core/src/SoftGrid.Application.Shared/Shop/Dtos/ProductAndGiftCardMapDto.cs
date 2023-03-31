using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductAndGiftCardMapDto : EntityDto<long>
    {
        public double? PurchaseAmount { get; set; }

        public double? GiftAmount { get; set; }

        public long ProductId { get; set; }

        public long? CurrencyId { get; set; }

    }
}