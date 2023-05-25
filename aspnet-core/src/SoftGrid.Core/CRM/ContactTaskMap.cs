using SoftGrid.CRM;
using SoftGrid.TaskManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.CRM
{
    [Table("ContactTaskMaps")]
    public class ContactTaskMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

        public virtual long TaskEventId { get; set; }

        [ForeignKey("TaskEventId")]
        public TaskEvent TaskEventFk { get; set; }

    }
}