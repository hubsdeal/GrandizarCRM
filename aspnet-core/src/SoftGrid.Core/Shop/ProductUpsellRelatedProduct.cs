using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("ProductUpsellRelatedProducts")]
    public class ProductUpsellRelatedProduct : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long RelatedProductId { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual long PrimaryProductId { get; set; }

        [ForeignKey("PrimaryProductId")]
        public Product PrimaryProductFk { get; set; }

    }
}