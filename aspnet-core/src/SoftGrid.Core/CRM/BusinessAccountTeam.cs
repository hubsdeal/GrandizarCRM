using SoftGrid.CRM;
using SoftGrid.CRM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.CRM
{
    [Table("BusinessAccountTeams")]
    public class BusinessAccountTeam : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual bool Primary { get; set; }

        public virtual long BusinessId { get; set; }

        [ForeignKey("BusinessId")]
        public Business BusinessFk { get; set; }

        public virtual long EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee EmployeeFk { get; set; }

    }
}