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
    [Table("ProductFlashSaleProductMaps")]
    public class ProductFlashSaleProductMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual double? FlashSalePrice { get; set; }

        public virtual double? DiscountPercentage { get; set; }

        public virtual double? DiscountAmount { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual string EndTime { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual string StartTime { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long? MembershipTypeId { get; set; }

        [ForeignKey("MembershipTypeId")]
        public MembershipType MembershipTypeFk { get; set; }

    }
}