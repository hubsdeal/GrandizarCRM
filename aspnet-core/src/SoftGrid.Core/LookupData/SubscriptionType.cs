using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("SubscriptionTypes")]
    public class SubscriptionType : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(SubscriptionTypeConsts.MaxNameLength, MinimumLength = SubscriptionTypeConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual int? NumberOfDays { get; set; }

    }
}