using SoftGrid.CRM;
using SoftGrid.TaskManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.CRM
{
    [Table("BusinessTaskMaps")]
    public class BusinessTaskMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long BusinessId { get; set; }

        [ForeignKey("BusinessId")]
        public Business BusinessFk { get; set; }

        public virtual long TaskEventId { get; set; }

        [ForeignKey("TaskEventId")]
        public TaskEvent TaskEventFk { get; set; }

    }
}