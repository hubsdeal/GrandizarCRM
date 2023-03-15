using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("EmailTemplates")]
    public class EmailTemplate : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(EmailTemplateConsts.MaxSubjectLength, MinimumLength = EmailTemplateConsts.MinSubjectLength)]
        public virtual string Subject { get; set; }

        [Required]
        public virtual string Content { get; set; }

        public virtual bool Published { get; set; }

    }
}