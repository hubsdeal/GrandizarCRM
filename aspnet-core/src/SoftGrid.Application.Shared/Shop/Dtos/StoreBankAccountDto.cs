using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreBankAccountDto : EntityDto<long>
    {
        public string AccountName { get; set; }

        public string AccountNo { get; set; }

        public string BankName { get; set; }

        public string RoutingNo { get; set; }

        public string BankAddress { get; set; }

        public long StoreId { get; set; }

    }
}