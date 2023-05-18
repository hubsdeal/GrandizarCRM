using SoftGrid.CRM;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.CRM
{
    [Table("BusinessDocuments")]
    public class BusinessDocument : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(BusinessDocumentConsts.MaxDocumentTitleLength, MinimumLength = BusinessDocumentConsts.MinDocumentTitleLength)]
        public virtual string DocumentTitle { get; set; }

        public virtual Guid FileBinaryObjectId { get; set; }

        public virtual long BusinessId { get; set; }

        [ForeignKey("BusinessId")]
        public Business BusinessFk { get; set; }

        public virtual long? DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]
        public DocumentType DocumentTypeFk { get; set; }

    }
}