using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetAllOrderDeliveryInfosInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TrackingNumberFilter { get; set; }

        public double? MaxTotalWeightFilter { get; set; }
        public double? MinTotalWeightFilter { get; set; }

        public string DeliveryProviderIdFilter { get; set; }

        public DateTime? MaxDispatchDateFilter { get; set; }
        public DateTime? MinDispatchDateFilter { get; set; }

        public string DispatchTimeFilter { get; set; }

        public DateTime? MaxDeliverToCustomerDateFilter { get; set; }
        public DateTime? MinDeliverToCustomerDateFilter { get; set; }

        public string DeliverToCustomerTimeFilter { get; set; }

        public string DeliveryNotesFilter { get; set; }

        public int? CustomerAcknowledgedFilter { get; set; }

        public string CustomerSignatureFilter { get; set; }

        public DateTime? MaxCateringDateFilter { get; set; }
        public DateTime? MinCateringDateFilter { get; set; }

        public string CateringTimeFilter { get; set; }

        public DateTime? MaxDeliveryDateFilter { get; set; }
        public DateTime? MinDeliveryDateFilter { get; set; }

        public string DeliveryTimeFilter { get; set; }

        public DateTime? MaxDineInDateFilter { get; set; }
        public DateTime? MinDineInDateFilter { get; set; }

        public string DineInTimeFilter { get; set; }

        public int? IncludedChildrenFilter { get; set; }

        public int? IsAsapFilter { get; set; }

        public int? IsPickupCateringFilter { get; set; }

        public int? MaxNumberOfGuestsFilter { get; set; }
        public int? MinNumberOfGuestsFilter { get; set; }

        public DateTime? MaxPickupDateFilter { get; set; }
        public DateTime? MinPickupDateFilter { get; set; }

        public string PickupTimeFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

        public string OrderInvoiceNumberFilter { get; set; }

    }
}