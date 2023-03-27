using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetAllLeadPipelineStatusesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public DateTime? MaxEntryDateFilter { get; set; }
        public DateTime? MinEntryDateFilter { get; set; }

        public string ExitDateFilter { get; set; }

        public DateTime? MaxEnteredAtFilter { get; set; }
        public DateTime? MinEnteredAtFilter { get; set; }

        public string LeadTitleFilter { get; set; }

        public string LeadPipelineStageNameFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

    }
}