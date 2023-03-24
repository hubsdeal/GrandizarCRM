using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.JobManagement
{
    [Table("JobStatusTypes")]
    public class JobStatusType : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(JobStatusTypeConsts.MaxNameLength, MinimumLength = JobStatusTypeConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}