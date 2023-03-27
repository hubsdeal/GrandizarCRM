using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreReviewFeedbackDto : EntityDto<long?>
    {

        [Required]
        public string ReplyText { get; set; }

        public bool IsPublished { get; set; }

        public long StoreReviewId { get; set; }

        public long? ContactId { get; set; }

        public long? RatingLikeId { get; set; }

    }
}