using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class LeadNoteDto : EntityDto<long>
    {
        public string Notes { get; set; }

        public long? LeadId { get; set; }

    }
}