using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class CreateOrEditTaskDocumentDto : EntityDto<long?>
    {

        [Required]
        [StringLength(TaskDocumentConsts.MaxDocumentTitleLength, MinimumLength = TaskDocumentConsts.MinDocumentTitleLength)]
        public string DocumentTitle { get; set; }

        public Guid FileBinaryObjectId { get; set; }

        public long TaskEventId { get; set; }

        public long? DocumentTypeId { get; set; }

    }
}