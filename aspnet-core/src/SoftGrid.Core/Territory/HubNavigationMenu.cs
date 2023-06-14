using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftGrid.Territory
{
    [Table("HubNavigationMenus")]
    public class HubNavigationMenu : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(HubNavigationMenuConsts.MaxCustomNameLength, MinimumLength = HubNavigationMenuConsts.MinCustomNameLength)]
        public virtual string CustomName { get; set; }

        public virtual string NavigationLink { get; set; }

        public virtual long HubId { get; set; }

        [ForeignKey("HubId")]
        public Hub HubFk { get; set; }

        public virtual long? MasterNavigationMenuId { get; set; }

        [ForeignKey("MasterNavigationMenuId")]
        public MasterNavigationMenu MasterNavigationMenuFk { get; set; }

        public virtual bool? HasParentMenu { get; set; }
        public virtual long? ParentMenuId { get; set; }
        public virtual int? DisplaySequence { get; set; }
    }
}