using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetAllLeadPipelineStagesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? MaxStageOrderFilter { get; set; }
        public int? MinStageOrderFilter { get; set; }

    }
}