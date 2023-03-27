using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetLeadSalesTeamForEditOutput
    {
        public CreateOrEditLeadSalesTeamDto LeadSalesTeam { get; set; }

        public string LeadFirstName { get; set; }

        public string EmployeeName { get; set; }

    }
}