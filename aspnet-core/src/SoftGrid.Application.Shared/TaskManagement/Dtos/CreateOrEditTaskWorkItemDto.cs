using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class CreateOrEditTaskWorkItemDto : EntityDto<long?>
    {

        [Required]
        [StringLength(TaskWorkItemConsts.MaxNameLength, MinimumLength = TaskWorkItemConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(TaskWorkItemConsts.MaxEstimatedHoursLength, MinimumLength = TaskWorkItemConsts.MinEstimatedHoursLength)]
        public string EstimatedHours { get; set; }

        [StringLength(TaskWorkItemConsts.MaxActualHoursLength, MinimumLength = TaskWorkItemConsts.MinActualHoursLength)]
        public string ActualHours { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(TaskWorkItemConsts.MaxStartTimeLength, MinimumLength = TaskWorkItemConsts.MinStartTimeLength)]
        public string StartTime { get; set; }

        [StringLength(TaskWorkItemConsts.MaxEndTimeLength, MinimumLength = TaskWorkItemConsts.MinEndTimeLength)]
        public string EndTime { get; set; }

        public bool OpenOrClosed { get; set; }

        public int? CompletionPercentage { get; set; }

        public long TaskEventId { get; set; }

        public long? EmployeeId { get; set; }

    }
}