using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.SalesLeadManagement
{
    [Table("LeadPipelineStages")]
    public class LeadPipelineStage : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(LeadPipelineStageConsts.MaxNameLength, MinimumLength = LeadPipelineStageConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual int? StageOrder { get; set; }

    }
}