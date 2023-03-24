using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetEmployeeTagForEditOutput
    {
        public CreateOrEditEmployeeTagDto EmployeeTag { get; set; }

        public string EmployeeName { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}