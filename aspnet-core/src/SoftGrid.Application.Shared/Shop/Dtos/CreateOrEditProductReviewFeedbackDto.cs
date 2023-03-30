using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductReviewFeedbackDto : EntityDto<long?>
    {

        [Required]
        public string ReplyText { get; set; }

        public bool Published { get; set; }

        public long? ContactId { get; set; }

        public long? ProductReviewId { get; set; }

        public long? RatingLikeId { get; set; }

    }
}