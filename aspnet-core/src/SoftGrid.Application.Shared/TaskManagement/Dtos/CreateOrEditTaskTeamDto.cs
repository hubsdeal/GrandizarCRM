using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class CreateOrEditTaskTeamDto : EntityDto<long?>
    {

        public DateTime? StartDate { get; set; }

        [StringLength(TaskTeamConsts.MaxStartTimeLength, MinimumLength = TaskTeamConsts.MinStartTimeLength)]
        public string StartTime { get; set; }

        [StringLength(TaskTeamConsts.MaxEndTimeLength, MinimumLength = TaskTeamConsts.MinEndTimeLength)]
        public string EndTime { get; set; }

        [StringLength(TaskTeamConsts.MaxHourMinutesLength, MinimumLength = TaskTeamConsts.MinHourMinutesLength)]
        public string HourMinutes { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsPrimary { get; set; }

        [StringLength(TaskTeamConsts.MaxEstimatedHourLength, MinimumLength = TaskTeamConsts.MinEstimatedHourLength)]
        public string EstimatedHour { get; set; }

        [StringLength(TaskTeamConsts.MaxSubTaskTitleLength, MinimumLength = TaskTeamConsts.MinSubTaskTitleLength)]
        public string SubTaskTitle { get; set; }

        public long TaskEventId { get; set; }

        public long? EmployeeId { get; set; }

        public long? ContactId { get; set; }

    }
}