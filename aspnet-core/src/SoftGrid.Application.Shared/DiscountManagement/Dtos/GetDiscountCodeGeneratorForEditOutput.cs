using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetDiscountCodeGeneratorForEditOutput
    {
        public CreateOrEditDiscountCodeGeneratorDto DiscountCodeGenerator { get; set; }

    }
}