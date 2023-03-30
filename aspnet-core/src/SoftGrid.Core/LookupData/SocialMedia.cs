using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("SocialMedias")]
    public class SocialMedia : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(SocialMediaConsts.MaxNameLength, MinimumLength = SocialMediaConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}