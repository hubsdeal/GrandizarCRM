using SoftGrid.WidgetManagement.Enums;
using SoftGrid.Territory;
using SoftGrid.WidgetManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.WidgetManagement
{
    [Table("HubWidgetMaps")]
    public class HubWidgetMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(HubWidgetMapConsts.MaxCustomNameLength, MinimumLength = HubWidgetMapConsts.MinCustomNameLength)]
        public virtual string CustomName { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual WidgetType WidgetTypeId { get; set; }

        public virtual long HubId { get; set; }

        [ForeignKey("HubId")]
        public Hub HubFk { get; set; }

        public virtual long MasterWidgetId { get; set; }

        [ForeignKey("MasterWidgetId")]
        public MasterWidget MasterWidgetFk { get; set; }

    }
}