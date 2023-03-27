using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetLeadTaskForEditOutput
    {
        public CreateOrEditLeadTaskDto LeadTask { get; set; }

        public string LeadTitle { get; set; }

        public string TaskEventName { get; set; }

    }
}