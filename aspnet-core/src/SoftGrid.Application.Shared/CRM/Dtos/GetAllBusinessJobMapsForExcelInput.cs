using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllBusinessJobMapsForExcelInput
    {
        public string Filter { get; set; }

        public string BusinessNameFilter { get; set; }

        public string JobTitleFilter { get; set; }

    }
}