using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class CreateOrEditCustomerWalletDto : EntityDto<long?>
    {

        public DateTime? WalletOpeningDate { get; set; }

        public DateTime? BalanceDate { get; set; }

        public double? BalanceAmount { get; set; }

        public long? ContactId { get; set; }

        public long? UserId { get; set; }

        public long? CurrencyId { get; set; }

    }
}