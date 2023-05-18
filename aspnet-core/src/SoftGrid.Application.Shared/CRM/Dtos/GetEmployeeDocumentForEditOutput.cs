using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetEmployeeDocumentForEditOutput
    {
        public CreateOrEditEmployeeDocumentDto EmployeeDocument { get; set; }

        public string EmployeeName { get; set; }

        public string DocumentTypeName { get; set; }

    }
}