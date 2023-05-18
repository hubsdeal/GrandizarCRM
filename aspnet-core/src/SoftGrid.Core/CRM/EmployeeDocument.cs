using SoftGrid.CRM;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.CRM
{
    [Table("EmployeeDocuments")]
    public class EmployeeDocument : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(EmployeeDocumentConsts.MaxDocumentTitleLength, MinimumLength = EmployeeDocumentConsts.MinDocumentTitleLength)]
        public virtual string DocumentTitle { get; set; }

        public virtual Guid FileBinaryObjectId { get; set; }

        public virtual long EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee EmployeeFk { get; set; }

        public virtual long? DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]
        public DocumentType DocumentTypeFk { get; set; }

    }
}