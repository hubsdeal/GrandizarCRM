using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductSubscriptionMapForEditOutput
    {
        public CreateOrEditProductSubscriptionMapDto ProductSubscriptionMap { get; set; }

        public string ProductName { get; set; }

        public string SubscriptionTypeName { get; set; }

    }
}