using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.LookupData.Dtos
{
    public class GetAllCountiesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string CountryNameFilter { get; set; }

        public string StateNameFilter { get; set; }

    }
}