using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllContactDocumentsForExcelInput
    {
        public string Filter { get; set; }

        public string DocumentTitleFilter { get; set; }

        public Guid? FileBinaryObjectIdFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string DocumentTypeNameFilter { get; set; }

    }
}