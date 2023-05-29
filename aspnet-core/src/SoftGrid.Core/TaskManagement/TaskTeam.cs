using SoftGrid.TaskManagement;
using SoftGrid.CRM;
using SoftGrid.CRM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.TaskManagement
{
    [Table("TaskTeams")]
    public class TaskTeam : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual DateTime? StartDate { get; set; }

        [StringLength(TaskTeamConsts.MaxStartTimeLength, MinimumLength = TaskTeamConsts.MinStartTimeLength)]
        public virtual string StartTime { get; set; }

        [StringLength(TaskTeamConsts.MaxEndTimeLength, MinimumLength = TaskTeamConsts.MinEndTimeLength)]
        public virtual string EndTime { get; set; }

        [StringLength(TaskTeamConsts.MaxHourMinutesLength, MinimumLength = TaskTeamConsts.MinHourMinutesLength)]
        public virtual string HourMinutes { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual bool IsPrimary { get; set; }

        [StringLength(TaskTeamConsts.MaxEstimatedHourLength, MinimumLength = TaskTeamConsts.MinEstimatedHourLength)]
        public virtual string EstimatedHour { get; set; }

        [StringLength(TaskTeamConsts.MaxSubTaskTitleLength, MinimumLength = TaskTeamConsts.MinSubTaskTitleLength)]
        public virtual string SubTaskTitle { get; set; }

        public virtual long TaskEventId { get; set; }

        [ForeignKey("TaskEventId")]
        public TaskEvent TaskEventFk { get; set; }

        public virtual long? EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee EmployeeFk { get; set; }

        public virtual long? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

    }
}