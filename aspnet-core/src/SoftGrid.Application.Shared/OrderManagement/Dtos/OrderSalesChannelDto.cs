using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.OrderManagement.Dtos
{
    public class OrderSalesChannelDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string LinkName { get; set; }

        public string ApiLink { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }

        public string Notes { get; set; }

    }
}