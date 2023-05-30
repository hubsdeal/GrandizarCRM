using SoftGrid.WidgetManagement;
using SoftGrid.CMS;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.WidgetManagement
{
    [Table("StoreWidgetContentMaps")]
    public class StoreWidgetContentMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual long StoreWidgetMapId { get; set; }

        [ForeignKey("StoreWidgetMapId")]
        public StoreWidgetMap StoreWidgetMapFk { get; set; }

        public virtual long ContentId { get; set; }

        [ForeignKey("ContentId")]
        public Content ContentFk { get; set; }

    }
}