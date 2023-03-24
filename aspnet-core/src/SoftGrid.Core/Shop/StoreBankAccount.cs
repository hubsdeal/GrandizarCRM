using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreBankAccounts")]
    public class StoreBankAccount : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(StoreBankAccountConsts.MaxAccountNameLength, MinimumLength = StoreBankAccountConsts.MinAccountNameLength)]
        public virtual string AccountName { get; set; }

        [StringLength(StoreBankAccountConsts.MaxAccountNoLength, MinimumLength = StoreBankAccountConsts.MinAccountNoLength)]
        public virtual string AccountNo { get; set; }

        [StringLength(StoreBankAccountConsts.MaxBankNameLength, MinimumLength = StoreBankAccountConsts.MinBankNameLength)]
        public virtual string BankName { get; set; }

        [StringLength(StoreBankAccountConsts.MaxRoutingNoLength, MinimumLength = StoreBankAccountConsts.MinRoutingNoLength)]
        public virtual string RoutingNo { get; set; }

        [StringLength(StoreBankAccountConsts.MaxBankAddressLength, MinimumLength = StoreBankAccountConsts.MinBankAddressLength)]
        public virtual string BankAddress { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

    }
}