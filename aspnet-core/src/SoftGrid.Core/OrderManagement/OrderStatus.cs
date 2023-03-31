using SoftGrid.Authorization.Roles;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.OrderManagement
{
    [Table("OrderStatuses")]
    public class OrderStatus : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(OrderStatusConsts.MaxNameLength, MinimumLength = OrderStatusConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual int? SequenceNo { get; set; }

        [StringLength(OrderStatusConsts.MaxColorCodeLength, MinimumLength = OrderStatusConsts.MinColorCodeLength)]
        public virtual string ColorCode { get; set; }

        public virtual string Message { get; set; }

        public virtual bool DeliveryOrPickup { get; set; }

        public virtual int? RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role RoleFk { get; set; }

    }
}