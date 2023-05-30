using SoftGrid.WidgetManagement.Enums;
using SoftGrid.WidgetManagement;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.WidgetManagement
{
    [Table("StoreWidgetMaps")]
    public class StoreWidgetMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual WidgetType WidgetTypeId { get; set; }

        [StringLength(StoreWidgetMapConsts.MaxCustomNameLength, MinimumLength = StoreWidgetMapConsts.MinCustomNameLength)]
        public virtual string CustomName { get; set; }

        public virtual long MasterWidgetId { get; set; }

        [ForeignKey("MasterWidgetId")]
        public MasterWidget MasterWidgetFk { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

    }
}