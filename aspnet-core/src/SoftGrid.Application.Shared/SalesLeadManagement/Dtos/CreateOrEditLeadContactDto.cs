using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class CreateOrEditLeadContactDto : EntityDto<long?>
    {

        public string Notes { get; set; }

        public int? InfluenceScore { get; set; }

        public long LeadId { get; set; }

        public long ContactId { get; set; }

    }
}