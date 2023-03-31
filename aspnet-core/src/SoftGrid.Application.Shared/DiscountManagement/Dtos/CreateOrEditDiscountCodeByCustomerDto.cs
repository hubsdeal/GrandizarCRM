using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class CreateOrEditDiscountCodeByCustomerDto : EntityDto<long?>
    {

        public long? DiscountCodeGeneratorId { get; set; }

        public long? ContactId { get; set; }

    }
}