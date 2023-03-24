using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreBankAccountsForExcelInput
    {
        public string Filter { get; set; }

        public string AccountNameFilter { get; set; }

        public string AccountNoFilter { get; set; }

        public string BankNameFilter { get; set; }

        public string RoutingNoFilter { get; set; }

        public string BankAddressFilter { get; set; }

        public string StoreNameFilter { get; set; }

    }
}