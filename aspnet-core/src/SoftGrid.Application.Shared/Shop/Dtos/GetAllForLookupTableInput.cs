using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}