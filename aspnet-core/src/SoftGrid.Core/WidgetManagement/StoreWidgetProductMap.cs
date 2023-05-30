using SoftGrid.WidgetManagement;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.WidgetManagement
{
    [Table("StoreWidgetProductMaps")]
    public class StoreWidgetProductMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual long StoreWidgetMapId { get; set; }

        [ForeignKey("StoreWidgetMapId")]
        public StoreWidgetMap StoreWidgetMapFk { get; set; }

        public virtual long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

    }
}