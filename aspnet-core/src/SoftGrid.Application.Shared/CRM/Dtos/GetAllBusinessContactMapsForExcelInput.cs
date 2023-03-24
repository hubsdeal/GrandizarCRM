using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllBusinessContactMapsForExcelInput
    {
        public string Filter { get; set; }

        public string BusinessNameFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

    }
}