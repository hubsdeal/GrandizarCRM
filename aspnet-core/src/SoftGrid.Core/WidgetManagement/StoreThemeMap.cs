using SoftGrid.WidgetManagement;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.WidgetManagement
{
    [Table("StoreThemeMaps")]
    public class StoreThemeMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual bool Active { get; set; }

        public virtual long StoreMasterThemeId { get; set; }

        [ForeignKey("StoreMasterThemeId")]
        public StoreMasterTheme StoreMasterThemeFk { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

    }
}