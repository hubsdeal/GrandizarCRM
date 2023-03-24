using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllBusinessTaskMapsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string BusinessNameFilter { get; set; }

        public string TaskEventNameFilter { get; set; }

    }
}