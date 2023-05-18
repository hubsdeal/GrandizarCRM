using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetAllTaskDocumentsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DocumentTitleFilter { get; set; }

        public Guid? FileBinaryObjectIdFilter { get; set; }

        public string TaskEventNameFilter { get; set; }

        public string DocumentTypeNameFilter { get; set; }

    }
}