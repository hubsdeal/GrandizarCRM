using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetAllTaskEventsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? StatusFilter { get; set; }

        public int? PriorityFilter { get; set; }

        public DateTime? MaxEventDateFilter { get; set; }
        public DateTime? MinEventDateFilter { get; set; }

        public string StartTimeFilter { get; set; }

        public string EndTimeFilter { get; set; }

        public int? TemplateFilter { get; set; }

        public string ActualTimeFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public string EstimatedTimeFilter { get; set; }

        public string HourAndMinutesFilter { get; set; }

        public string TaskStatusNameFilter { get; set; }

    }
}