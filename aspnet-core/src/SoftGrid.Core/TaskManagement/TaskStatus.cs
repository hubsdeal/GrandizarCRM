using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.TaskManagement
{
    [Table("TaskStatuses")]
    public class TaskStatus : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(TaskStatusConsts.MaxNameLength, MinimumLength = TaskStatusConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}