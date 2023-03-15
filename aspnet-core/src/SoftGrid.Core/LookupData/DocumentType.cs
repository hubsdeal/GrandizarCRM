using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("DocumentTypes")]
    public class DocumentType : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(DocumentTypeConsts.MaxNameLength, MinimumLength = DocumentTypeConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}