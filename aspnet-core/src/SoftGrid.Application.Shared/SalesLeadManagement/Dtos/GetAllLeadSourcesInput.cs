using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetAllLeadSourcesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}