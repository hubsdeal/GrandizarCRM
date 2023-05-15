using Abp.Application.Services.Dto;

namespace SoftGrid.CMS.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}