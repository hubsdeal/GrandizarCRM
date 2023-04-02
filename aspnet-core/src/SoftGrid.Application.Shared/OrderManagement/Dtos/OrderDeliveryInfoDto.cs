using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.OrderManagement.Dtos
{
    public class OrderDeliveryInfoDto : EntityDto<long>
    {
        public string TrackingNumber { get; set; }

        public double? TotalWeight { get; set; }

        public string DeliveryProviderId { get; set; }

        public DateTime? DispatchDate { get; set; }

        public string DispatchTime { get; set; }

        public DateTime? DeliverToCustomerDate { get; set; }

        public string DeliverToCustomerTime { get; set; }

        public string DeliveryNotes { get; set; }

        public bool CustomerAcknowledged { get; set; }

        public string CustomerSignature { get; set; }

        public DateTime? CateringDate { get; set; }

        public string CateringTime { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string DeliveryTime { get; set; }

        public DateTime? DineInDate { get; set; }

        public string DineInTime { get; set; }

        public bool IncludedChildren { get; set; }

        public bool IsAsap { get; set; }

        public bool IsPickupCatering { get; set; }

        public int? NumberOfGuests { get; set; }

        public DateTime? PickupDate { get; set; }

        public string PickupTime { get; set; }

        public long? EmployeeId { get; set; }

        public long? OrderId { get; set; }

    }
}