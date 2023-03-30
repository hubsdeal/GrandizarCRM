using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("ProductByVariants")]
    public class ProductByVariant : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual double? Price { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual string Description { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? ProductVariantId { get; set; }

        [ForeignKey("ProductVariantId")]
        public ProductVariant ProductVariantFk { get; set; }

        public virtual long? ProductVariantCategoryId { get; set; }

        [ForeignKey("ProductVariantCategoryId")]
        public ProductVariantCategory ProductVariantCategoryFk { get; set; }

        public virtual long? MediaLibraryId { get; set; }

        [ForeignKey("MediaLibraryId")]
        public MediaLibrary MediaLibraryFk { get; set; }

    }
}