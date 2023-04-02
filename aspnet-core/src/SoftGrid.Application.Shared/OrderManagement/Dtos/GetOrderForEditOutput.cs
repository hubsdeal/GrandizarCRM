using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderForEditOutput
    {
        public CreateOrEditOrderDto Order { get; set; }

        public string StateName { get; set; }

        public string CountryName { get; set; }

        public string ContactFullName { get; set; }

        public string OrderStatusName { get; set; }

        public string CurrencyName { get; set; }

        public string StoreName { get; set; }

        public string OrderSalesChannelName { get; set; }

    }
}