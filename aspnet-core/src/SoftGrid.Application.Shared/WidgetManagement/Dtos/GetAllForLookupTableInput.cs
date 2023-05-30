using Abp.Application.Services.Dto;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}