using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class CreateOrEditLeadTaskDto : EntityDto<long?>
    {

        public long LeadId { get; set; }

        public long TaskEventId { get; set; }

    }
}