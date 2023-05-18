using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.JobManagement.Dtos
{
    public class CreateOrEditJobDocumentDto : EntityDto<long?>
    {

        [Required]
        [StringLength(JobDocumentConsts.MaxDocumentTitleLength, MinimumLength = JobDocumentConsts.MinDocumentTitleLength)]
        public string DocumentTitle { get; set; }

        public Guid FileBinaryObjectId { get; set; }

        public long JobId { get; set; }

        public long? DocumentTypeId { get; set; }

    }
}