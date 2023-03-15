using Abp.Application.Services.Dto;

namespace SoftGrid.Territory.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}