using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderStatusForEditOutput
    {
        public CreateOrEditOrderStatusDto OrderStatus { get; set; }

        public string RoleName { get; set; }

    }
}