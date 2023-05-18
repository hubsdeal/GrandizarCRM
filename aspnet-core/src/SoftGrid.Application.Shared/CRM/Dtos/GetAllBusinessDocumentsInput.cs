using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllBusinessDocumentsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DocumentTitleFilter { get; set; }

        public Guid? FileBinaryObjectIdFilter { get; set; }

        public string BusinessNameFilter { get; set; }

        public string DocumentTypeNameFilter { get; set; }

    }
}