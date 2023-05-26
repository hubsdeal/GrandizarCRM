using SoftGrid.JobManagement;
using SoftGrid.TaskManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.JobManagement
{
    [Table("JobTaskMaps")]
    public class JobTaskMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long JobId { get; set; }

        [ForeignKey("JobId")]
        public Job JobFk { get; set; }

        public virtual long TaskEventId { get; set; }

        [ForeignKey("TaskEventId")]
        public TaskEvent TaskEventFk { get; set; }

    }
}