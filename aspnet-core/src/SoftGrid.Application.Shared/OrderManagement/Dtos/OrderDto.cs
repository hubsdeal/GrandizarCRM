using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.OrderManagement.Dtos
{
    public class OrderDto : EntityDto<long>
    {
        public string InvoiceNumber { get; set; }

        public bool DeliveryOrPickup { get; set; }

        public bool PaymentCompleted { get; set; }

        public string FullName { get; set; }

        public string DeliveryAddress { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string Notes { get; set; }

        public double? DeliveryFee { get; set; }

        public double? SubTotalExcludedTax { get; set; }

        public double? TotalDiscountAmount { get; set; }

        public double? TotalTaxAmount { get; set; }

        public double? TotalAmount { get; set; }

        public string Email { get; set; }

        public double? DiscountAmountByCode { get; set; }

        public double? GratuityAmount { get; set; }

        public double? GratuityPercentage { get; set; }

        public double? ServiceCharge { get; set; }

        public long? StateId { get; set; }

        public long? CountryId { get; set; }

        public long? ContactId { get; set; }

        public long? OrderStatusId { get; set; }

        public long? CurrencyId { get; set; }

        public long? StoreId { get; set; }

        public long? OrderSalesChannelId { get; set; }

    }
}