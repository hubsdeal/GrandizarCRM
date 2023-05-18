using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Territory.Dtos
{
    public class GetAllHubDocumentsForExcelInput
    {
        public string Filter { get; set; }

        public string DocumentTitleFilter { get; set; }

        public Guid? FileBinaryObjectIdFilter { get; set; }

        public string HubNameFilter { get; set; }

        public string DocumentTypeNameFilter { get; set; }

    }
}