using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetAllTaskWorkItemsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string EstimatedHoursFilter { get; set; }

        public string ActualHoursFilter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public string StartTimeFilter { get; set; }

        public string EndTimeFilter { get; set; }

        public int? OpenOrClosedFilter { get; set; }

        public int? MaxCompletionPercentageFilter { get; set; }
        public int? MinCompletionPercentageFilter { get; set; }

        public string TaskEventNameFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

    }
}