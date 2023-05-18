using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditEmployeeDocumentDto : EntityDto<long?>
    {

        [Required]
        [StringLength(EmployeeDocumentConsts.MaxDocumentTitleLength, MinimumLength = EmployeeDocumentConsts.MinDocumentTitleLength)]
        public string DocumentTitle { get; set; }

        public Guid FileBinaryObjectId { get; set; }

        public long EmployeeId { get; set; }

        public long? DocumentTypeId { get; set; }

    }
}