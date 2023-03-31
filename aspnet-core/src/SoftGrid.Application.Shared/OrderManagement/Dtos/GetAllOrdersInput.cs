using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetAllOrdersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string InvoiceNumberFilter { get; set; }

        public int? DeliveryOrPickupFilter { get; set; }

        public int? PaymentCompletedFilter { get; set; }

        public string FullNameFilter { get; set; }

        public string DeliveryAddressFilter { get; set; }

        public string CityFilter { get; set; }

        public string ZipCodeFilter { get; set; }

        public string NotesFilter { get; set; }

        public double? MaxDeliveryFeeFilter { get; set; }
        public double? MinDeliveryFeeFilter { get; set; }

        public double? MaxSubTotalExcludedTaxFilter { get; set; }
        public double? MinSubTotalExcludedTaxFilter { get; set; }

        public double? MaxTotalDiscountAmountFilter { get; set; }
        public double? MinTotalDiscountAmountFilter { get; set; }

        public double? MaxTotalTaxAmountFilter { get; set; }
        public double? MinTotalTaxAmountFilter { get; set; }

        public double? MaxTotalAmountFilter { get; set; }
        public double? MinTotalAmountFilter { get; set; }

        public string EmailFilter { get; set; }

        public double? MaxDiscountAmountByCodeFilter { get; set; }
        public double? MinDiscountAmountByCodeFilter { get; set; }

        public double? MaxGratuityAmountFilter { get; set; }
        public double? MinGratuityAmountFilter { get; set; }

        public double? MaxGratuityPercentageFilter { get; set; }
        public double? MinGratuityPercentageFilter { get; set; }

        public double? MaxServiceChargeFilter { get; set; }
        public double? MinServiceChargeFilter { get; set; }

        public string StateNameFilter { get; set; }

        public string CountryNameFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string OrderStatusNameFilter { get; set; }

        public string CurrencyNameFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string OrderSalesChannelNameFilter { get; set; }

    }
}