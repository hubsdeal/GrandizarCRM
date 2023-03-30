using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("ProductVariants")]
    public class ProductVariant : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ProductVariantConsts.MaxNameLength, MinimumLength = ProductVariantConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual long? ProductVariantCategoryId { get; set; }

        [ForeignKey("ProductVariantCategoryId")]
        public ProductVariantCategory ProductVariantCategoryFk { get; set; }

    }
}