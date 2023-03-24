using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class CreateOrEditTaskEventDto : EntityDto<long?>
    {

        [Required]
        [StringLength(TaskEventConsts.MaxNameLength, MinimumLength = TaskEventConsts.MinNameLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        public bool Priority { get; set; }

        public DateTime? EventDate { get; set; }

        [StringLength(TaskEventConsts.MaxStartTimeLength, MinimumLength = TaskEventConsts.MinStartTimeLength)]
        public string StartTime { get; set; }

        [StringLength(TaskEventConsts.MaxEndTimeLength, MinimumLength = TaskEventConsts.MinEndTimeLength)]
        public string EndTime { get; set; }

        public bool Template { get; set; }

        [StringLength(TaskEventConsts.MaxActualTimeLength, MinimumLength = TaskEventConsts.MinActualTimeLength)]
        public string ActualTime { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(TaskEventConsts.MaxEstimatedTimeLength, MinimumLength = TaskEventConsts.MinEstimatedTimeLength)]
        public string EstimatedTime { get; set; }

        [StringLength(TaskEventConsts.MaxHourAndMinutesLength, MinimumLength = TaskEventConsts.MinHourAndMinutesLength)]
        public string HourAndMinutes { get; set; }

        public long? TaskStatusId { get; set; }

    }
}