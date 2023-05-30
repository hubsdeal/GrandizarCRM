using SoftGrid.WidgetManagement;
using SoftGrid.CMS;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.WidgetManagement
{
    [Table("HubWidgetContentMaps")]
    public class HubWidgetContentMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual long HubWidgetMapId { get; set; }

        [ForeignKey("HubWidgetMapId")]
        public HubWidgetMap HubWidgetMapFk { get; set; }

        public virtual long ContentId { get; set; }

        [ForeignKey("ContentId")]
        public Content ContentFk { get; set; }

    }
}