using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetAllTaskTeamsForExcelInput
    {
        public string Filter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public string StartTimeFilter { get; set; }

        public string EndTimeFilter { get; set; }

        public string HourMinutesFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public int? IsPrimaryFilter { get; set; }

        public string EstimatedHourFilter { get; set; }

        public string SubTaskTitleFilter { get; set; }

        public string TaskEventNameFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

    }
}