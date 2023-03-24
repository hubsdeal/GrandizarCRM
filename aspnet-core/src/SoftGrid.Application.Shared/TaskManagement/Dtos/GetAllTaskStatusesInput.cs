using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetAllTaskStatusesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}