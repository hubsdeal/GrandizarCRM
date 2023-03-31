using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderPaymentInfoForEditOutput
    {
        public CreateOrEditOrderPaymentInfoDto OrderPaymentInfo { get; set; }

        public string OrderInvoiceNumber { get; set; }

        public string CurrencyName { get; set; }

        public string PaymentTypeName { get; set; }

    }
}