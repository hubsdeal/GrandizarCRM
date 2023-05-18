using SoftGrid.Territory;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Territory
{
    [Table("HubDocuments")]
    public class HubDocument : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(HubDocumentConsts.MaxDocumentTitleLength, MinimumLength = HubDocumentConsts.MinDocumentTitleLength)]
        public virtual string DocumentTitle { get; set; }

        public virtual Guid FileBinaryObjectId { get; set; }

        public virtual long HubId { get; set; }

        [ForeignKey("HubId")]
        public Hub HubFk { get; set; }

        public virtual long? DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]
        public DocumentType DocumentTypeFk { get; set; }

    }
}