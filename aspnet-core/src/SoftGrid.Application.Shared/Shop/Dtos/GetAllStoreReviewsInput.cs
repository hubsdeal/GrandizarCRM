using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreReviewsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ReviewInfoFilter { get; set; }

        public DateTime? MaxPostDateFilter { get; set; }
        public DateTime? MinPostDateFilter { get; set; }

        public string PostTimeFilter { get; set; }

        public int? IsPublishFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string RatingLikeNameFilter { get; set; }

    }
}