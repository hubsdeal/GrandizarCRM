using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductReviewFeedbackDto : EntityDto<long>
    {
        public string ReplyText { get; set; }

        public bool Published { get; set; }

        public long? ContactId { get; set; }

        public long? ProductReviewId { get; set; }

        public long? RatingLikeId { get; set; }

    }
}