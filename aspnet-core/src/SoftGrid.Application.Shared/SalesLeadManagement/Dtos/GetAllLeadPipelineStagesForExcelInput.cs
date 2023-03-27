using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetAllLeadPipelineStagesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? MaxStageOrderFilter { get; set; }
        public int? MinStageOrderFilter { get; set; }

    }
}