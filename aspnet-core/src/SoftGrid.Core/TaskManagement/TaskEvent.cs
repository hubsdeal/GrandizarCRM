using SoftGrid.TaskManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.TaskManagement
{
    [Table("TaskEvents")]
    public class TaskEvent : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(TaskEventConsts.MaxNameLength, MinimumLength = TaskEventConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Status { get; set; }

        public virtual bool Priority { get; set; }

        public virtual DateTime? EventDate { get; set; }

        [StringLength(TaskEventConsts.MaxStartTimeLength, MinimumLength = TaskEventConsts.MinStartTimeLength)]
        public virtual string StartTime { get; set; }

        [StringLength(TaskEventConsts.MaxEndTimeLength, MinimumLength = TaskEventConsts.MinEndTimeLength)]
        public virtual string EndTime { get; set; }

        public virtual bool Template { get; set; }

        [StringLength(TaskEventConsts.MaxActualTimeLength, MinimumLength = TaskEventConsts.MinActualTimeLength)]
        public virtual string ActualTime { get; set; }

        public virtual DateTime? EndDate { get; set; }

        [StringLength(TaskEventConsts.MaxEstimatedTimeLength, MinimumLength = TaskEventConsts.MinEstimatedTimeLength)]
        public virtual string EstimatedTime { get; set; }

        [StringLength(TaskEventConsts.MaxHourAndMinutesLength, MinimumLength = TaskEventConsts.MinHourAndMinutesLength)]
        public virtual string HourAndMinutes { get; set; }

        public virtual long? TaskStatusId { get; set; }

        [ForeignKey("TaskStatusId")]
        public TaskStatus TaskStatusFk { get; set; }

    }
}