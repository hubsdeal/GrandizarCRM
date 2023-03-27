using SoftGrid.SalesLeadManagement;
using SoftGrid.SalesLeadManagement;
using SoftGrid.CRM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.SalesLeadManagement
{
    [Table("LeadPipelineStatuses")]
    public class LeadPipelineStatus : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual DateTime? EntryDate { get; set; }

        public virtual string ExitDate { get; set; }

        public virtual DateTime? EnteredAt { get; set; }

        public virtual long LeadId { get; set; }

        [ForeignKey("LeadId")]
        public Lead LeadFk { get; set; }

        public virtual long LeadPipelineStageId { get; set; }

        [ForeignKey("LeadPipelineStageId")]
        public LeadPipelineStage LeadPipelineStageFk { get; set; }

        public virtual long? EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee EmployeeFk { get; set; }

    }
}