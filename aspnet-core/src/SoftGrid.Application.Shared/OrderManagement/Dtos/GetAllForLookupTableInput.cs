using Abp.Application.Services.Dto;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}