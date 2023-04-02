using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class CreateOrEditOrderDeliveryInfoDto : EntityDto<long?>
    {

        [StringLength(OrderDeliveryInfoConsts.MaxTrackingNumberLength, MinimumLength = OrderDeliveryInfoConsts.MinTrackingNumberLength)]
        public string TrackingNumber { get; set; }

        public double? TotalWeight { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxDeliveryProviderIdLength, MinimumLength = OrderDeliveryInfoConsts.MinDeliveryProviderIdLength)]
        public string DeliveryProviderId { get; set; }

        public DateTime? DispatchDate { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxDispatchTimeLength, MinimumLength = OrderDeliveryInfoConsts.MinDispatchTimeLength)]
        public string DispatchTime { get; set; }

        public DateTime? DeliverToCustomerDate { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxDeliverToCustomerTimeLength, MinimumLength = OrderDeliveryInfoConsts.MinDeliverToCustomerTimeLength)]
        public string DeliverToCustomerTime { get; set; }

        public string DeliveryNotes { get; set; }

        public bool CustomerAcknowledged { get; set; }

        public string CustomerSignature { get; set; }

        public DateTime? CateringDate { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxCateringTimeLength, MinimumLength = OrderDeliveryInfoConsts.MinCateringTimeLength)]
        public string CateringTime { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxDeliveryTimeLength, MinimumLength = OrderDeliveryInfoConsts.MinDeliveryTimeLength)]
        public string DeliveryTime { get; set; }

        public DateTime? DineInDate { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxDineInTimeLength, MinimumLength = OrderDeliveryInfoConsts.MinDineInTimeLength)]
        public string DineInTime { get; set; }

        public bool IncludedChildren { get; set; }

        public bool IsAsap { get; set; }

        public bool IsPickupCatering { get; set; }

        public int? NumberOfGuests { get; set; }

        public DateTime? PickupDate { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxPickupTimeLength, MinimumLength = OrderDeliveryInfoConsts.MinPickupTimeLength)]
        public string PickupTime { get; set; }

        public long? EmployeeId { get; set; }

        public long? OrderId { get; set; }

    }
}