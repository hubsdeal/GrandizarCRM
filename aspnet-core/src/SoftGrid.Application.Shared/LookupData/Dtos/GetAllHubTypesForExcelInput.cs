using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.LookupData.Dtos
{
    public class GetAllHubTypesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}