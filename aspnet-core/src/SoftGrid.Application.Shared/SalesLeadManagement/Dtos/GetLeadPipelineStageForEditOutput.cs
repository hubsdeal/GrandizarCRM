using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetLeadPipelineStageForEditOutput
    {
        public CreateOrEditLeadPipelineStageDto LeadPipelineStage { get; set; }

    }
}