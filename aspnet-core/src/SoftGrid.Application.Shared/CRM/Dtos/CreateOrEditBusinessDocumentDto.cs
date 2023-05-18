using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditBusinessDocumentDto : EntityDto<long?>
    {

        [Required]
        [StringLength(BusinessDocumentConsts.MaxDocumentTitleLength, MinimumLength = BusinessDocumentConsts.MinDocumentTitleLength)]
        public string DocumentTitle { get; set; }

        public Guid FileBinaryObjectId { get; set; }

        public long BusinessId { get; set; }

        public long? DocumentTypeId { get; set; }

    }
}