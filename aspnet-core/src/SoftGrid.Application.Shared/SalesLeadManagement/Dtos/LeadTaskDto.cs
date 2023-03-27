using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class LeadTaskDto : EntityDto<long>
    {

        public long LeadId { get; set; }

        public long TaskEventId { get; set; }

    }
}