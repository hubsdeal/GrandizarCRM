using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreTaxes")]
    public class StoreTax : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(StoreTaxConsts.MaxTaxNameLength, MinimumLength = StoreTaxConsts.MinTaxNameLength)]
        public virtual string TaxName { get; set; }

        public virtual bool PercentageOrAmount { get; set; }

        public virtual double? TaxRatePercentage { get; set; }

        public virtual double? TaxAmount { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

    }
}