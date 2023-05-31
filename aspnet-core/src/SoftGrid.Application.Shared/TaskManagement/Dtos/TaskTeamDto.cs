using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.TaskManagement.Dtos
{
    public class TaskTeamDto : EntityDto<long>
    {
        public DateTime? StartDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string HourMinutes { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsPrimary { get; set; }

        public string EstimatedHour { get; set; }

        public string SubTaskTitle { get; set; }

        public long TaskEventId { get; set; }

        public long? EmployeeId { get; set; }

        public long? ContactId { get; set; }

    }
}