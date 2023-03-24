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
    [Table("ContactTags")]
    public class ContactTag : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(ContactTagConsts.MaxCustomTagLength, MinimumLength = ContactTagConsts.MinCustomTagLength)]
        public virtual string CustomTag { get; set; }

        [StringLength(ContactTagConsts.MaxTagValueLength, MinimumLength = ContactTagConsts.MinTagValueLength)]
        public virtual string TagValue { get; set; }

        public virtual bool Verified { get; set; }

        public virtual int? Sequence { get; set; }

        public virtual long ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

        public virtual long? MasterTagId { get; set; }

        [ForeignKey("MasterTagId")]
        public MasterTag MasterTagFk { get; set; }

    }
}