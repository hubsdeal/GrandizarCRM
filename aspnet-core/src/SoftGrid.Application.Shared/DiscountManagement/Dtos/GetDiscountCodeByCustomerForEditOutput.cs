using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetDiscountCodeByCustomerForEditOutput
    {
        public CreateOrEditDiscountCodeByCustomerDto DiscountCodeByCustomer { get; set; }

        public string DiscountCodeGeneratorName { get; set; }

        public string ContactFullName { get; set; }

    }
}