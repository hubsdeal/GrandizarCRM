using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllBusinessProductMapsForExcelInput
    {
        public string Filter { get; set; }

        public string BusinessNameFilter { get; set; }

        public string ProductNameFilter { get; set; }

    }
}