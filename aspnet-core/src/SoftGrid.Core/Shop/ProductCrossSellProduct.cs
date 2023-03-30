using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("ProductCrossSellProducts")]
    public class ProductCrossSellProduct : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long CrossProductId { get; set; }

        public virtual int? CrossSellScore { get; set; }

        public virtual long PrimaryProductId { get; set; }

        [ForeignKey("PrimaryProductId")]
        public Product PrimaryProductFk { get; set; }

    }
}