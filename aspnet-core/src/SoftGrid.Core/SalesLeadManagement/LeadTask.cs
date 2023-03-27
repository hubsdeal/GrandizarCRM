using SoftGrid.SalesLeadManagement;
using SoftGrid.TaskManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.SalesLeadManagement
{
    [Table("LeadTasks")]
    public class LeadTask : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long LeadId { get; set; }

        [ForeignKey("LeadId")]
        public Lead LeadFk { get; set; }

        public virtual long TaskEventId { get; set; }

        [ForeignKey("TaskEventId")]
        public TaskEvent TaskEventFk { get; set; }

    }
}