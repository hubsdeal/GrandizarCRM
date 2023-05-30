using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Territory
{
    [Table("MasterNavigationMenus")]
    public class MasterNavigationMenu : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(MasterNavigationMenuConsts.MaxNameLength, MinimumLength = MasterNavigationMenuConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual bool HasParentMenu { get; set; }

        public virtual long? ParentMenuId { get; set; }

        public virtual Guid IconLink { get; set; }
        public virtual string ContentLink { get; set; }
        public virtual int? DisplaySequence { get; set; }
        public virtual string NavigationLink { get; set; }

    }
}