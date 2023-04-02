using SoftGrid.OrderManagement;
using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.OrderManagement
{
    [Table("OrderProductInfos")]
    public class OrderProductInfo : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual double? UnitPrice { get; set; }

        public virtual double? ByProductDiscountAmount { get; set; }

        public virtual double? ByProductDiscountPercentage { get; set; }

        public virtual double? ByProductTaxAmount { get; set; }

        public virtual double? ByProductTotalAmount { get; set; }

        public virtual long? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order OrderFk { get; set; }

        public virtual long? StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? MeasurementUnitId { get; set; }

        [ForeignKey("MeasurementUnitId")]
        public MeasurementUnit MeasurementUnitFk { get; set; }

    }
}