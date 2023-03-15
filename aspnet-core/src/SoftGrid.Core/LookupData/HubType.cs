using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("HubTypes")]
    public class HubType : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(HubTypeConsts.MaxNameLength, MinimumLength = HubTypeConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}