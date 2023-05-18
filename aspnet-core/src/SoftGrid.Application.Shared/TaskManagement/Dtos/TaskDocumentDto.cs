using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.TaskManagement.Dtos
{
    public class TaskDocumentDto : EntityDto<long>
    {
        public string DocumentTitle { get; set; }

        public Guid FileBinaryObjectId { get; set; }

        public long TaskEventId { get; set; }

        public long? DocumentTypeId { get; set; }

    }
}