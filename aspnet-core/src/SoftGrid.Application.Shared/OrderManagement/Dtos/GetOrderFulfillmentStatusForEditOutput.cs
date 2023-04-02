using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderFulfillmentStatusForEditOutput
    {
        public CreateOrEditOrderFulfillmentStatusDto OrderFulfillmentStatus { get; set; }

        public string OrderStatusName { get; set; }

        public string OrderInvoiceNumber { get; set; }

        public string EmployeeName { get; set; }

    }
}