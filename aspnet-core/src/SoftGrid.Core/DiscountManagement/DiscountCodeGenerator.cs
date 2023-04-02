using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.DiscountManagement
{
    [Table("DiscountCodeGenerators")]
    public class DiscountCodeGenerator : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(DiscountCodeGeneratorConsts.MaxNameLength, MinimumLength = DiscountCodeGeneratorConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(DiscountCodeGeneratorConsts.MaxCouponCodeLength, MinimumLength = DiscountCodeGeneratorConsts.MinCouponCodeLength)]
        public virtual string CouponCode { get; set; }

        public virtual bool PercentageOrFixedAmount { get; set; }

        public virtual double? DiscountPercentage { get; set; }

        public virtual double? DiscountAmount { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual string AdminNotes { get; set; }

        public virtual bool IsActive { get; set; }

        [StringLength(DiscountCodeGeneratorConsts.MaxStartTimeLength, MinimumLength = DiscountCodeGeneratorConsts.MinStartTimeLength)]
        public virtual string StartTime { get; set; }

        [StringLength(DiscountCodeGeneratorConsts.MaxEndTimeLength, MinimumLength = DiscountCodeGeneratorConsts.MinEndTimeLength)]
        public virtual string EndTime { get; set; }

    }
}