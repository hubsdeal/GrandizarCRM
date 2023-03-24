using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.TaskManagement.Dtos
{
    public class TaskEventDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        public bool Priority { get; set; }

        public DateTime? EventDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool Template { get; set; }

        public string ActualTime { get; set; }

        public DateTime? EndDate { get; set; }

        public string EstimatedTime { get; set; }

        public string HourAndMinutes { get; set; }

        public long? TaskStatusId { get; set; }

    }
}