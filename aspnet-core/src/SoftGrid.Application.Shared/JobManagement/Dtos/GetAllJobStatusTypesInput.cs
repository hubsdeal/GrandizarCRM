using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.JobManagement.Dtos
{
    public class GetAllJobStatusTypesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}