using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class CreateOrEditLeadPipelineStageDto : EntityDto<long?>
    {

        [Required]
        [StringLength(LeadPipelineStageConsts.MaxNameLength, MinimumLength = LeadPipelineStageConsts.MinNameLength)]
        public string Name { get; set; }

        public int? StageOrder { get; set; }

    }
}