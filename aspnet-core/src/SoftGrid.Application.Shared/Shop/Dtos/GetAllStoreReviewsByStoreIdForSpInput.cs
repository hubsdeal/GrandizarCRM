using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreReviewsByStoreIdForSpInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public long? StoreIdFilter { get; set; }
        public long? ContactIdFilter { get; set; }
        public int? IsPublish { get; set; }
    }
}