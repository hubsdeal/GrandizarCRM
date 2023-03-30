using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Territory.Dtos
{
    public class GetAllHubAccountTeamsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? PrimaryManagerFilter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public string HubNameFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

        public string UserNameFilter { get; set; }

    }
}