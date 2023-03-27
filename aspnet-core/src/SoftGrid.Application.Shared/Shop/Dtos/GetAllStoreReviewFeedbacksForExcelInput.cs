using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreReviewFeedbacksForExcelInput
    {
        public string Filter { get; set; }

        public string ReplyTextFilter { get; set; }

        public int? IsPublishedFilter { get; set; }

        public string StoreReviewReviewInfoFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string RatingLikeNameFilter { get; set; }

    }
}