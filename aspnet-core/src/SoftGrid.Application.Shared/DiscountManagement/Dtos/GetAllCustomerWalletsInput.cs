using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetAllCustomerWalletsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public DateTime? MaxWalletOpeningDateFilter { get; set; }
        public DateTime? MinWalletOpeningDateFilter { get; set; }

        public DateTime? MaxBalanceDateFilter { get; set; }
        public DateTime? MinBalanceDateFilter { get; set; }

        public double? MaxBalanceAmountFilter { get; set; }
        public double? MinBalanceAmountFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string CurrencyNameFilter { get; set; }

    }
}