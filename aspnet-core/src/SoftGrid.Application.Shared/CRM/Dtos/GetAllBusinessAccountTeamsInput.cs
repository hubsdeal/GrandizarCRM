using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllBusinessAccountTeamsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? PrimaryFilter { get; set; }

        public string BusinessNameFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

    }
}