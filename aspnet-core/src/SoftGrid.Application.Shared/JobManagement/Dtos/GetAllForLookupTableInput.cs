using Abp.Application.Services.Dto;

namespace SoftGrid.JobManagement.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}