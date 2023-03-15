using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}