using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllEmployeeDocumentsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DocumentTitleFilter { get; set; }

        public Guid? FileBinaryObjectIdFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

        public string DocumentTypeNameFilter { get; set; }

    }
}