using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderProductInfoForEditOutput
    {
        public CreateOrEditOrderProductInfoDto OrderProductInfo { get; set; }

        public string OrderInvoiceNumber { get; set; }

        public string StoreName { get; set; }

        public string ProductName { get; set; }

        public string MeasurementUnitName { get; set; }

    }
}