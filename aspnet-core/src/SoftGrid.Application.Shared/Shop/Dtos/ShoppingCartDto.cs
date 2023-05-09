using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ShoppingCartDto : EntityDto<long>
    {
        public int? Quantity { get; set; }

        public double? UnitPrice { get; set; }

        public double? TotalAmount { get; set; }

        public double? UnitTotalPrice { get; set; }

        public double? UnitDiscountAmount { get; set; }

        public long? ContactId { get; set; }

        public long? OrderId { get; set; }

        public long? StoreId { get; set; }

        public long? ProductId { get; set; }

        public long? CurrencyId { get; set; }

    }
}