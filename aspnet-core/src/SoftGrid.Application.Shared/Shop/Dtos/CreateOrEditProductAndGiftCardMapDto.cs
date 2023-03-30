using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductAndGiftCardMapDto : EntityDto<long?>
    {

        public double? PurchaseAmount { get; set; }

        public double? GiftAmount { get; set; }

        public long ProductId { get; set; }

        public long? CurrencyId { get; set; }

    }
}