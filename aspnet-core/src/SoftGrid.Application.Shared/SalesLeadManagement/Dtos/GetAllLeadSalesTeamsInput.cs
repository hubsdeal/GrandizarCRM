using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetAllLeadSalesTeamsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? PrimaryFilter { get; set; }

        public DateTime? MaxAssignedDateFilter { get; set; }
        public DateTime? MinAssignedDateFilter { get; set; }

        public string LeadFirstNameFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

    }
}