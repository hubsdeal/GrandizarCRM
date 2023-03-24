using Abp.Application.Services.Dto;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}