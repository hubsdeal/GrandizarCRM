using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.CRM;
using SoftGrid.OrderManagement;
using SoftGrid.LookupData;
using SoftGrid.Shop;
using SoftGrid.OrderManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.OrderManagement
{
    [Table("Orders")]
    public class Order : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(OrderConsts.MaxInvoiceNumberLength, MinimumLength = OrderConsts.MinInvoiceNumberLength)]
        public virtual string InvoiceNumber { get; set; }

        public virtual bool DeliveryOrPickup { get; set; }

        public virtual bool PaymentCompleted { get; set; }

        [StringLength(OrderConsts.MaxFullNameLength, MinimumLength = OrderConsts.MinFullNameLength)]
        public virtual string FullName { get; set; }

        [StringLength(OrderConsts.MaxDeliveryAddressLength, MinimumLength = OrderConsts.MinDeliveryAddressLength)]
        public virtual string DeliveryAddress { get; set; }

        [StringLength(OrderConsts.MaxCityLength, MinimumLength = OrderConsts.MinCityLength)]
        public virtual string City { get; set; }

        [StringLength(OrderConsts.MaxZipCodeLength, MinimumLength = OrderConsts.MinZipCodeLength)]
        public virtual string ZipCode { get; set; }

        public virtual string Notes { get; set; }

        public virtual double? DeliveryFee { get; set; }

        public virtual double? SubTotalExcludedTax { get; set; }

        public virtual double? TotalDiscountAmount { get; set; }

        public virtual double? TotalTaxAmount { get; set; }

        public virtual double? TotalAmount { get; set; }

        [StringLength(OrderConsts.MaxEmailLength, MinimumLength = OrderConsts.MinEmailLength)]
        public virtual string Email { get; set; }

        public virtual double? DiscountAmountByCode { get; set; }

        public virtual double? GratuityAmount { get; set; }

        public virtual double? GratuityPercentage { get; set; }

        public virtual double? ServiceCharge { get; set; }

        public virtual long? StateId { get; set; }

        [ForeignKey("StateId")]
        public State StateFk { get; set; }

        public virtual long? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual long? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

        public virtual long? OrderStatusId { get; set; }

        [ForeignKey("OrderStatusId")]
        public OrderStatus OrderStatusFk { get; set; }

        public virtual long? CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency CurrencyFk { get; set; }

        public virtual long? StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long? OrderSalesChannelId { get; set; }

        [ForeignKey("OrderSalesChannelId")]
        public OrderSalesChannel OrderSalesChannelFk { get; set; }

    }
}