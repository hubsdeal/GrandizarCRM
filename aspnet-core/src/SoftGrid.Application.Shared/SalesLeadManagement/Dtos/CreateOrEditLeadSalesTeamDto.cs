using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class CreateOrEditLeadSalesTeamDto : EntityDto<long?>
    {

        public bool Primary { get; set; }

        public DateTime? AssignedDate { get; set; }

        public long LeadId { get; set; }

        public long? EmployeeId { get; set; }

    }
}