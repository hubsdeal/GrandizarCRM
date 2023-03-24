using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("MarketplaceCommissionTypes")]
    public class MarketplaceCommissionType : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(MarketplaceCommissionTypeConsts.MaxNameLength, MinimumLength = MarketplaceCommissionTypeConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual double? Percentage { get; set; }

        public virtual double? FixedAmount { get; set; }

    }
}