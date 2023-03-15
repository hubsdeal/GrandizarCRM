using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("MembershipTypes")]
    public class MembershipType : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(MembershipTypeConsts.MaxNameLength, MinimumLength = MembershipTypeConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MembershipTypeConsts.MaxDescriptionLength, MinimumLength = MembershipTypeConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

    }
}