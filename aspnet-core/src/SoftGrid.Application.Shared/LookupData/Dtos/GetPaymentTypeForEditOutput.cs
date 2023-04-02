using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class GetPaymentTypeForEditOutput
    {
        public CreateOrEditPaymentTypeDto PaymentType { get; set; }

    }
}