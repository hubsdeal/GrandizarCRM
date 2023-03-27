using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.SalesLeadManagement
{
    [Table("LeadSources")]
    public class LeadSource : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(LeadSourceConsts.MaxNameLength, MinimumLength = LeadSourceConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}