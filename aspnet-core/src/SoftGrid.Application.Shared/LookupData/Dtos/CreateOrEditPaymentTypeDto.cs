using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditPaymentTypeDto : EntityDto<long?>
    {

        [Required]
        [StringLength(PaymentTypeConsts.MaxNameLength, MinimumLength = PaymentTypeConsts.MinNameLength)]
        public string Name { get; set; }

    }
}