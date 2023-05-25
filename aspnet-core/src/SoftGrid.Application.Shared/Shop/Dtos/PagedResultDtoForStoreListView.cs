using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class PagedResultDtoForStoreListView<T> : ListResultDto<T>, IPagedResult<T>, IListResult<T>, IHasTotalCount
    {
        public PagedResultDtoForStoreListView(int totalCount, int numberOfPublished, int numberOfUnpublished, int numberOfpickupPoint, List<GetStoreForViewDto> items)
        {
            this.TotalCount = totalCount;
            this.NumberOfPublished = numberOfPublished;
            this.NumberOfUnpublished = numberOfUnpublished;
            this.NumberOfPickUpPoint = numberOfpickupPoint;
            this.Items = items;
        }
        public int TotalCount { get; set; }
        public int NumberOfPublished { get; set; }
        public int NumberOfUnpublished { get; set; }
        public int NumberOfPickUpPoint { get; set; }
        public List<GetStoreForViewDto> Items { get; set; }
    }
}