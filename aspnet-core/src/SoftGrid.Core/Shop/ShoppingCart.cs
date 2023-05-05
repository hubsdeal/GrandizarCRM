using SoftGrid.CRM;
using SoftGrid.OrderManagement;
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
    [Table("ShoppingCarts")]
    public class ShoppingCart : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual double? UnitPrice { get; set; }

        public virtual double? TotalAmount { get; set; }

        public virtual double? UnitTotalPrice { get; set; }

        public virtual double? UnitDiscountAmount { get; set; }

        public virtual long? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

        public virtual long? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order OrderFk { get; set; }

        public virtual long? StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency CurrencyFk { get; set; }

    }
}