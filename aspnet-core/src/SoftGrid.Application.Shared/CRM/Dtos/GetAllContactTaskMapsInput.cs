using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllContactTaskMapsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string TaskEventNameFilter { get; set; }

    }
}