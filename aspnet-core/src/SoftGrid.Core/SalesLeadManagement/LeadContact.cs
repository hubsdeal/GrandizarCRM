using SoftGrid.SalesLeadManagement;
using SoftGrid.CRM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.SalesLeadManagement
{
    [Table("LeadContacts")]
    public class LeadContact : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Notes { get; set; }

        public virtual int? InfluenceScore { get; set; }

        public virtual long LeadId { get; set; }

        [ForeignKey("LeadId")]
        public Lead LeadFk { get; set; }

        public virtual long ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

    }
}