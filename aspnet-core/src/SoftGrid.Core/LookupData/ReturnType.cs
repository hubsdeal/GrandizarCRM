using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("ReturnTypes")]
    public class ReturnType : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ReturnTypeConsts.MaxNameLength, MinimumLength = ReturnTypeConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}