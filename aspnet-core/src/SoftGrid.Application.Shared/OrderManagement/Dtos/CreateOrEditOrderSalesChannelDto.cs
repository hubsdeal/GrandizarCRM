using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class CreateOrEditOrderSalesChannelDto : EntityDto<long?>
    {

        [Required]
        [StringLength(OrderSalesChannelConsts.MaxNameLength, MinimumLength = OrderSalesChannelConsts.MinNameLength)]
        public string Name { get; set; }

        public string LinkName { get; set; }

        public string ApiLink { get; set; }

        [StringLength(OrderSalesChannelConsts.MaxUserIdLength, MinimumLength = OrderSalesChannelConsts.MinUserIdLength)]
        public string UserId { get; set; }

        [StringLength(OrderSalesChannelConsts.MaxPasswordLength, MinimumLength = OrderSalesChannelConsts.MinPasswordLength)]
        public string Password { get; set; }

        public string Notes { get; set; }

    }
}