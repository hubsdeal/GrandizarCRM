using SoftGrid.Territory;
using SoftGrid.Territory;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

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

    }
}