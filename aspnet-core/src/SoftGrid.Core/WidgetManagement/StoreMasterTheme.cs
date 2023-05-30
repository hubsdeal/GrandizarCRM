using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.WidgetManagement
{
    [Table("StoreMasterThemes")]
    public class StoreMasterTheme : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(StoreMasterThemeConsts.MaxNameLength, MinimumLength = StoreMasterThemeConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string ThemeCode { get; set; }

        public virtual Guid ThumbnailImageId { get; set; }

    }
}