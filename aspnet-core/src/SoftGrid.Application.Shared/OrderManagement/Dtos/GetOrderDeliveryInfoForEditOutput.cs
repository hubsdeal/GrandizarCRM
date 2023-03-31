using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderDeliveryInfoForEditOutput
    {
        public CreateOrEditOrderDeliveryInfoDto OrderDeliveryInfo { get; set; }

        public string EmployeeName { get; set; }

        public string OrderInvoiceNumber { get; set; }

    }
}