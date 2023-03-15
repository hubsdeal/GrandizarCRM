using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.LookupData.Dtos
{
    public class GetAllSmsTemplatesForExcelInput
    {
        public string Filter { get; set; }

        public string TitleFilter { get; set; }

        public string ContentFilter { get; set; }

        public int? PublishedFilter { get; set; }

    }
}