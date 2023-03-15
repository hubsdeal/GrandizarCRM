using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.LookupData.Dtos
{
    public class GetAllMasterTagCategoriesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string MediaLibraryNameFilter { get; set; }

    }
}