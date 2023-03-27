using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetLeadPipelineStatusForEditOutput
    {
        public CreateOrEditLeadPipelineStatusDto LeadPipelineStatus { get; set; }

        public string LeadTitle { get; set; }

        public string LeadPipelineStageName { get; set; }

        public string EmployeeName { get; set; }

    }
}