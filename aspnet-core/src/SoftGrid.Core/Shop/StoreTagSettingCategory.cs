using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreTagSettingCategories")]
    public class StoreTagSettingCategory : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(StoreTagSettingCategoryConsts.MaxNameLength, MinimumLength = StoreTagSettingCategoryConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual Guid ImageId { get; set; }

        [StringLength(StoreTagSettingCategoryConsts.MaxDescriptionLength, MinimumLength = StoreTagSettingCategoryConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

    }
}