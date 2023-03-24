using SoftGrid.JobManagement;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.JobManagement
{
    [Table("JobTags")]
    public class JobTag : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(JobTagConsts.MaxCustomTagLength, MinimumLength = JobTagConsts.MinCustomTagLength)]
        public virtual string CustomTag { get; set; }

        [StringLength(JobTagConsts.MaxTagValueLength, MinimumLength = JobTagConsts.MinTagValueLength)]
        public virtual string TagValue { get; set; }

        public virtual bool Verified { get; set; }

        public virtual int? Sequence { get; set; }

        public virtual long JobId { get; set; }

        [ForeignKey("JobId")]
        public Job JobFk { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

        public virtual long? MasterTagId { get; set; }

        [ForeignKey("MasterTagId")]
        public MasterTag MasterTagFk { get; set; }

    }
}