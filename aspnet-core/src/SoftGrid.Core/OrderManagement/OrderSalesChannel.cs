using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.OrderManagement
{
    [Table("OrderSalesChannels")]
    public class OrderSalesChannel : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(OrderSalesChannelConsts.MaxNameLength, MinimumLength = OrderSalesChannelConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string LinkName { get; set; }

        public virtual string ApiLink { get; set; }

        [StringLength(OrderSalesChannelConsts.MaxUserIdLength, MinimumLength = OrderSalesChannelConsts.MinUserIdLength)]
        public virtual string UserId { get; set; }

        [StringLength(OrderSalesChannelConsts.MaxPasswordLength, MinimumLength = OrderSalesChannelConsts.MinPasswordLength)]
        public virtual string Password { get; set; }

        public virtual string Notes { get; set; }

    }
}