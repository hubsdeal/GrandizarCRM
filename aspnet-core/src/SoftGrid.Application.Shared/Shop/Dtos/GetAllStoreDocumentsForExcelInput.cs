using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreDocumentsForExcelInput
    {
        public string Filter { get; set; }

        public string DocumentTitleFilter { get; set; }

        public Guid? FileBinaryObjectIdFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string DocumentTypeNameFilter { get; set; }

    }
}