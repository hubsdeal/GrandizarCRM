using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class LeadSalesTeamDto : EntityDto<long>
    {
        public bool Primary { get; set; }

        public DateTime? AssignedDate { get; set; }

        public long LeadId { get; set; }

        public long? EmployeeId { get; set; }

    }
}