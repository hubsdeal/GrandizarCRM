using SoftGrid.Shop;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreProductMaps")]
    public class StoreProductMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual bool Published { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

    }
}