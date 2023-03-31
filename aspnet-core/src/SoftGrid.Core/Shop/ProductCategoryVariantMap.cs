using SoftGrid.Shop;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("ProductCategoryVariantMaps")]
    public class ProductCategoryVariantMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategoryFk { get; set; }

        public virtual long ProductVariantCategoryId { get; set; }

        [ForeignKey("ProductVariantCategoryId")]
        public ProductVariantCategory ProductVariantCategoryFk { get; set; }

    }
}