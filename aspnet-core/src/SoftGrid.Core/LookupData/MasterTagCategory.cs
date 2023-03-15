using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("MasterTagCategories")]
    public class MasterTagCategory : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(MasterTagCategoryConsts.MaxNameLength, MinimumLength = MasterTagCategoryConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MasterTagCategoryConsts.MaxDescriptionLength, MinimumLength = MasterTagCategoryConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual Guid PictureId { get; set; }

    }
}