using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class CreateOrEditHubDocumentDto : EntityDto<long?>
    {

        [Required]
        [StringLength(HubDocumentConsts.MaxDocumentTitleLength, MinimumLength = HubDocumentConsts.MinDocumentTitleLength)]
        public string DocumentTitle { get; set; }

        public Guid FileBinaryObjectId { get; set; }

        public long HubId { get; set; }

        public long? DocumentTypeId { get; set; }

    }
}