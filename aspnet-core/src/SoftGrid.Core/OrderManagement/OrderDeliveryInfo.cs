using SoftGrid.CRM;
using SoftGrid.OrderManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.OrderManagement
{
    [Table("OrderDeliveryInfos")]
    public class OrderDeliveryInfo : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxTrackingNumberLength, MinimumLength = OrderDeliveryInfoConsts.MinTrackingNumberLength)]
        public virtual string TrackingNumber { get; set; }

        public virtual double? TotalWeight { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxDeliveryProviderIdLength, MinimumLength = OrderDeliveryInfoConsts.MinDeliveryProviderIdLength)]
        public virtual string DeliveryProviderId { get; set; }

        public virtual DateTime? DispatchDate { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxDispatchTimeLength, MinimumLength = OrderDeliveryInfoConsts.MinDispatchTimeLength)]
        public virtual string DispatchTime { get; set; }

        public virtual DateTime? DeliverToCustomerDate { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxDeliverToCustomerTimeLength, MinimumLength = OrderDeliveryInfoConsts.MinDeliverToCustomerTimeLength)]
        public virtual string DeliverToCustomerTime { get; set; }

        public virtual string DeliveryNotes { get; set; }

        public virtual bool CustomerAcknowledged { get; set; }

        public virtual string CustomerSignature { get; set; }

        public virtual DateTime? CateringDate { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxCateringTimeLength, MinimumLength = OrderDeliveryInfoConsts.MinCateringTimeLength)]
        public virtual string CateringTime { get; set; }

        public virtual DateTime? DeliveryDate { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxDeliveryTimeLength, MinimumLength = OrderDeliveryInfoConsts.MinDeliveryTimeLength)]
        public virtual string DeliveryTime { get; set; }

        public virtual DateTime? DineInDate { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxDineInTimeLength, MinimumLength = OrderDeliveryInfoConsts.MinDineInTimeLength)]
        public virtual string DineInTime { get; set; }

        public virtual bool IncludedChildren { get; set; }

        public virtual bool IsAsap { get; set; }

        public virtual bool IsPickupCatering { get; set; }

        public virtual int? NumberOfGuests { get; set; }

        public virtual DateTime? PickupDate { get; set; }

        [StringLength(OrderDeliveryInfoConsts.MaxPickupTimeLength, MinimumLength = OrderDeliveryInfoConsts.MinPickupTimeLength)]
        public virtual string PickupTime { get; set; }

        public virtual long? EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee EmployeeFk { get; set; }

        public virtual long? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order OrderFk { get; set; }

    }
}