using Abp.Application.Services.Dto;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}