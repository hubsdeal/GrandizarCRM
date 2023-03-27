using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetAllLeadTasksForExcelInput
    {
        public string Filter { get; set; }

        public string LeadTitleFilter { get; set; }

        public string TaskEventNameFilter { get; set; }

    }
}