using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreDocumentDto : EntityDto<long?>
    {

        [Required]
        [StringLength(StoreDocumentConsts.MaxDocumentTitleLength, MinimumLength = StoreDocumentConsts.MinDocumentTitleLength)]
        public string DocumentTitle { get; set; }

        public Guid FileBinaryObjectId { get; set; }

        public long StoreId { get; set; }

        public long? DocumentTypeId { get; set; }

    }
}