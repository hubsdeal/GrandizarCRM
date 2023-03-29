using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductCustomerQueryForEditOutput
    {
        public CreateOrEditProductCustomerQueryDto ProductCustomerQuery { get; set; }

        public string ProductName { get; set; }

        public string ContactFullName { get; set; }

        public string EmployeeName { get; set; }

    }
}