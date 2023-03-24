using SoftGrid.CRM;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.CRM
{
    [Table("BusinessTags")]
    public class BusinessTag : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(BusinessTagConsts.MaxCustomTagLength, MinimumLength = BusinessTagConsts.MinCustomTagLength)]
        public virtual string CustomTag { get; set; }

        [StringLength(BusinessTagConsts.MaxTagValueLength, MinimumLength = BusinessTagConsts.MinTagValueLength)]
        public virtual string TagValue { get; set; }

        public virtual bool Verified { get; set; }

        public virtual int? Sequence { get; set; }

        public virtual long BusinessId { get; set; }

        [ForeignKey("BusinessId")]
        public Business BusinessFk { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

        public virtual long? MasterTagId { get; set; }

        [ForeignKey("MasterTagId")]
        public MasterTag MasterTagFk { get; set; }

    }
}