using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class LeadPipelineStageDto : EntityDto<long>
    {
        public string Name { get; set; }

        public int? StageOrder { get; set; }

    }
}