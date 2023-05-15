using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductTeamsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? PrimaryFilter { get; set; }

        public int? ActiveFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

        public string ProductNameFilter { get; set; }

    }
}