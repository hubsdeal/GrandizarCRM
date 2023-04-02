using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetCustomerWalletForEditOutput
    {
        public CreateOrEditCustomerWalletDto CustomerWallet { get; set; }

        public string ContactFullName { get; set; }

        public string UserName { get; set; }

        public string CurrencyName { get; set; }

    }
}