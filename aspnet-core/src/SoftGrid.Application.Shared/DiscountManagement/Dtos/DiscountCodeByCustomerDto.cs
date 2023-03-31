using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class DiscountCodeByCustomerDto : EntityDto<long>
    {

        public long? DiscountCodeGeneratorId { get; set; }

        public long? ContactId { get; set; }

    }
}