using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.JobManagement.Dtos
{
    public class GetAllJobTaskMapsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string JobTitleFilter { get; set; }

        public string TaskEventNameFilter { get; set; }

    }
}