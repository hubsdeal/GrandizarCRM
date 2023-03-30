using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductReviewFeedbackForEditOutput
    {
        public CreateOrEditProductReviewFeedbackDto ProductReviewFeedback { get; set; }

        public string ContactFullName { get; set; }

        public string ProductReviewReviewInfo { get; set; }

        public string RatingLikeName { get; set; }

    }
}