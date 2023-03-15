using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("MasterTags")]
    public class MasterTag : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(MasterTagConsts.MaxNameLength, MinimumLength = MasterTagConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MasterTagConsts.MaxDescriptionLength, MinimumLength = MasterTagConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MasterTagConsts.MaxSynonymsLength, MinimumLength = MasterTagConsts.MinSynonymsLength)]
        public virtual string Synonyms { get; set; }

        public virtual Guid PictureId { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual long MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

    }
}