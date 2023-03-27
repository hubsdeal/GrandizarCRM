using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class LeadPipelineStatusDto : EntityDto<long>
    {
        public DateTime? EntryDate { get; set; }

        public string ExitDate { get; set; }

        public DateTime? EnteredAt { get; set; }

        public long LeadId { get; set; }

        public long LeadPipelineStageId { get; set; }

        public long? EmployeeId { get; set; }

    }
}