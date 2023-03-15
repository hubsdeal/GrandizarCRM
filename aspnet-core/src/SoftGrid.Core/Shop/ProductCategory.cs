using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("ProductCategories")]
    public class ProductCategory : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ProductCategoryConsts.MaxNameLength, MinimumLength = ProductCategoryConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual bool HasParentCategory { get; set; }

        public virtual long? ParentCategoryId { get; set; }

        [StringLength(ProductCategoryConsts.MaxUrlLength, MinimumLength = ProductCategoryConsts.MinUrlLength)]
        public virtual string Url { get; set; }

        [StringLength(ProductCategoryConsts.MaxMetaTitleLength, MinimumLength = ProductCategoryConsts.MinMetaTitleLength)]
        public virtual string MetaTitle { get; set; }

        [StringLength(ProductCategoryConsts.MaxMetaKeywordsLength, MinimumLength = ProductCategoryConsts.MinMetaKeywordsLength)]
        public virtual string MetaKeywords { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual bool ProductOrService { get; set; }

        public virtual long? MediaLibraryId { get; set; }

        [ForeignKey("MediaLibraryId")]
        public MediaLibrary MediaLibraryFk { get; set; }

    }
}