using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreBankAccountDto : EntityDto<long?>
    {

        [StringLength(StoreBankAccountConsts.MaxAccountNameLength, MinimumLength = StoreBankAccountConsts.MinAccountNameLength)]
        public string AccountName { get; set; }

        [StringLength(StoreBankAccountConsts.MaxAccountNoLength, MinimumLength = StoreBankAccountConsts.MinAccountNoLength)]
        public string AccountNo { get; set; }

        [StringLength(StoreBankAccountConsts.MaxBankNameLength, MinimumLength = StoreBankAccountConsts.MinBankNameLength)]
        public string BankName { get; set; }

        [StringLength(StoreBankAccountConsts.MaxRoutingNoLength, MinimumLength = StoreBankAccountConsts.MinRoutingNoLength)]
        public string RoutingNo { get; set; }

        [StringLength(StoreBankAccountConsts.MaxBankAddressLength, MinimumLength = StoreBankAccountConsts.MinBankAddressLength)]
        public string BankAddress { get; set; }

        public long StoreId { get; set; }

    }
}