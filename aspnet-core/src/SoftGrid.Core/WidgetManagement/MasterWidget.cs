using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.WidgetManagement
{
    [Table("MasterWidgets")]
    public class MasterWidget : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(MasterWidgetConsts.MaxNameLength, MinimumLength = MasterWidgetConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string DesignCode { get; set; }

        public virtual bool Publish { get; set; }

        public virtual int? InternalDisplayNumber { get; set; }

        public virtual Guid ThumbnailImageId { get; set; }

    }
}