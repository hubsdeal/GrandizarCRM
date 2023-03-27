using SoftGrid.SalesLeadManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.SalesLeadManagement
{
    [Table("LeadNotes")]
    public class LeadNote : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Notes { get; set; }

        public virtual long? LeadId { get; set; }

        [ForeignKey("LeadId")]
        public Lead LeadFk { get; set; }

    }
}