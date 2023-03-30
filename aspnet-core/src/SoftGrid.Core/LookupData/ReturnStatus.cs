using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("ReturnStatuses")]
    public class ReturnStatus : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ReturnStatusConsts.MaxNameLength, MinimumLength = ReturnStatusConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}