using SoftGrid.SalesLeadManagement;
using SoftGrid.CRM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.SalesLeadManagement
{
    [Table("LeadSalesTeams")]
    public class LeadSalesTeam : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual bool Primary { get; set; }

        public virtual DateTime? AssignedDate { get; set; }

        public virtual long LeadId { get; set; }

        [ForeignKey("LeadId")]
        public Lead LeadFk { get; set; }

        public virtual long? EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee EmployeeFk { get; set; }

    }
}