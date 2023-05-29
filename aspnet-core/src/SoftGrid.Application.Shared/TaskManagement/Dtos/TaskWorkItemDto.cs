using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.TaskManagement.Dtos
{
    public class TaskWorkItemDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string EstimatedHours { get; set; }

        public string ActualHours { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool OpenOrClosed { get; set; }

        public int? CompletionPercentage { get; set; }

        public long TaskEventId { get; set; }

        public long? EmployeeId { get; set; }

    }
}