using SoftGrid.WidgetManagement;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.WidgetManagement
{
    [Table("HubWidgetStoreMaps")]
    public class HubWidgetStoreMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual long HubWidgetMapId { get; set; }

        [ForeignKey("HubWidgetMapId")]
        public HubWidgetMap HubWidgetMapFk { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

    }
}