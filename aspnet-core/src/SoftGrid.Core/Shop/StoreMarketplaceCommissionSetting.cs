using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreMarketplaceCommissionSettings")]
    public class StoreMarketplaceCommissionSetting : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual double? Percentage { get; set; }

        public virtual double? FixedAmount { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long? MarketplaceCommissionTypeId { get; set; }

        [ForeignKey("MarketplaceCommissionTypeId")]
        public MarketplaceCommissionType MarketplaceCommissionTypeFk { get; set; }

        public virtual long? ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategoryFk { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

    }
}