using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.CRM.Dtos
{
    public class ContactDocumentDto : EntityDto<long>
    {
        public string DocumentTitle { get; set; }

        public Guid FileBinaryObjectId { get; set; }

        public long ContactId { get; set; }

        public long? DocumentTypeId { get; set; }

    }
}