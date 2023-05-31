using SoftGrid.TaskManagement;
using SoftGrid.CRM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.TaskManagement
{
    [Table("TaskWorkItems")]
    public class TaskWorkItem : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(TaskWorkItemConsts.MaxNameLength, MinimumLength = TaskWorkItemConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(TaskWorkItemConsts.MaxEstimatedHoursLength, MinimumLength = TaskWorkItemConsts.MinEstimatedHoursLength)]
        public virtual string EstimatedHours { get; set; }

        [StringLength(TaskWorkItemConsts.MaxActualHoursLength, MinimumLength = TaskWorkItemConsts.MinActualHoursLength)]
        public virtual string ActualHours { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        [StringLength(TaskWorkItemConsts.MaxStartTimeLength, MinimumLength = TaskWorkItemConsts.MinStartTimeLength)]
        public virtual string StartTime { get; set; }

        [StringLength(TaskWorkItemConsts.MaxEndTimeLength, MinimumLength = TaskWorkItemConsts.MinEndTimeLength)]
        public virtual string EndTime { get; set; }

        public virtual bool OpenOrClosed { get; set; }

        public virtual int? CompletionPercentage { get; set; }

        public virtual long TaskEventId { get; set; }

        [ForeignKey("TaskEventId")]
        public TaskEvent TaskEventFk { get; set; }

        public virtual long? EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee EmployeeFk { get; set; }

    }
}