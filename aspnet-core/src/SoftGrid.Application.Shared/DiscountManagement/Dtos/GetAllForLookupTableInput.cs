using Abp.Application.Services.Dto;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}