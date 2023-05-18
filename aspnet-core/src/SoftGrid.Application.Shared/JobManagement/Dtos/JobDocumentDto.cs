using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.JobManagement.Dtos
{
    public class JobDocumentDto : EntityDto<long>
    {
        public string DocumentTitle { get; set; }

        public Guid FileBinaryObjectId { get; set; }

        public long JobId { get; set; }

        public long? DocumentTypeId { get; set; }

    }
}