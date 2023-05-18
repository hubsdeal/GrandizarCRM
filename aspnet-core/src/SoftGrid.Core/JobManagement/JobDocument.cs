using SoftGrid.JobManagement;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.JobManagement
{
    [Table("JobDocuments")]
    public class JobDocument : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(JobDocumentConsts.MaxDocumentTitleLength, MinimumLength = JobDocumentConsts.MinDocumentTitleLength)]
        public virtual string DocumentTitle { get; set; }

        public virtual Guid FileBinaryObjectId { get; set; }

        public virtual long JobId { get; set; }

        [ForeignKey("JobId")]
        public Job JobFk { get; set; }

        public virtual long? DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]
        public DocumentType DocumentTypeFk { get; set; }

    }
}