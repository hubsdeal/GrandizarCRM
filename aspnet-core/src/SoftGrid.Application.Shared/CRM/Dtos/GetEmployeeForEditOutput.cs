using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetEmployeeForEditOutput
    {
        public CreateOrEditEmployeeDto Employee { get; set; }

        public string StateName { get; set; }

        public string CountryName { get; set; }

        public string ContactFullName { get; set; }

    }
}