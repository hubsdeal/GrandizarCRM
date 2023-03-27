using SoftGrid.SalesLeadManagement;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.SalesLeadManagement
{
    [Table("LeadTags")]
    public class LeadTag : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(LeadTagConsts.MaxCustomTagLength, MinimumLength = LeadTagConsts.MinCustomTagLength)]
        public virtual string CustomTag { get; set; }

        [StringLength(LeadTagConsts.MaxTagValueLength, MinimumLength = LeadTagConsts.MinTagValueLength)]
        public virtual string TagValue { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual long LeadId { get; set; }

        [ForeignKey("LeadId")]
        public Lead LeadFk { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

        public virtual long? MasterTagId { get; set; }

        [ForeignKey("MasterTagId")]
        public MasterTag MasterTagFk { get; set; }

    }
}