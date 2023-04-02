using SoftGrid.Shop;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.OrderManagement
{
    [Table("OrderProductVariants")]
    public class OrderProductVariant : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual double? Price { get; set; }

        public virtual long? OrderProductInfoId { get; set; }

        public virtual long? ProductVariantCategoryId { get; set; }

        [ForeignKey("ProductVariantCategoryId")]
        public ProductVariantCategory ProductVariantCategoryFk { get; set; }

        public virtual long? ProductVariantId { get; set; }

        [ForeignKey("ProductVariantId")]
        public ProductVariant ProductVariantFk { get; set; }

    }
}