using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("SmsTemplates")]
    public class SmsTemplate : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(SmsTemplateConsts.MaxTitleLength, MinimumLength = SmsTemplateConsts.MinTitleLength)]
        public virtual string Title { get; set; }

        [Required]
        public virtual string Content { get; set; }

        public virtual bool Published { get; set; }

    }
}