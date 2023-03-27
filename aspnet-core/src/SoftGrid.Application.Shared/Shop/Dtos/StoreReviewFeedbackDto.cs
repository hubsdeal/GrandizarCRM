using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreReviewFeedbackDto : EntityDto<long>
    {
        public string ReplyText { get; set; }

        public bool IsPublished { get; set; }

        public long StoreReviewId { get; set; }

        public long? ContactId { get; set; }

        public long? RatingLikeId { get; set; }

    }
}