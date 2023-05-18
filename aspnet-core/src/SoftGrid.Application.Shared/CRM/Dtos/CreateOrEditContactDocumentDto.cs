using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditContactDocumentDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ContactDocumentConsts.MaxDocumentTitleLength, MinimumLength = ContactDocumentConsts.MinDocumentTitleLength)]
        public string DocumentTitle { get; set; }

        public Guid FileBinaryObjectId { get; set; }

        public long ContactId { get; set; }

        public long? DocumentTypeId { get; set; }

    }
}