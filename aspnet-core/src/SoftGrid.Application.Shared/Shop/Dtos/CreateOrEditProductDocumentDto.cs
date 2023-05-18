using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductDocumentDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ProductDocumentConsts.MaxDocumentTitleLength, MinimumLength = ProductDocumentConsts.MinDocumentTitleLength)]
        public string DocumentTitle { get; set; }

        public Guid FileBinaryObjectId { get; set; }

        public long ProductId { get; set; }

        public long? DocumentTypeId { get; set; }

    }
}