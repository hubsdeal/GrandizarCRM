using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetAllLeadSourcesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}