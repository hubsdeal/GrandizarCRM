using SoftGrid.OrderManagement;
using SoftGrid.OrderManagement;
using SoftGrid.CRM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.OrderManagement
{
    [Table("OrderFulfillmentStatuses")]
    public class OrderFulfillmentStatus : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual DateTime? EstimatedTime { get; set; }

        public virtual DateTime? ActualTime { get; set; }

        public virtual long? OrderStatusId { get; set; }

        [ForeignKey("OrderStatusId")]
        public OrderStatus OrderStatusFk { get; set; }

        public virtual long? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order OrderFk { get; set; }

        public virtual long? EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee EmployeeFk { get; set; }

    }
}