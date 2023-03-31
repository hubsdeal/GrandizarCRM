using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderSalesChannelForEditOutput
    {
        public CreateOrEditOrderSalesChannelDto OrderSalesChannel { get; set; }

    }
}