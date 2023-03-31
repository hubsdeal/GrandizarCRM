using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("PaymentTypes")]
    public class PaymentType : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(PaymentTypeConsts.MaxNameLength, MinimumLength = PaymentTypeConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}