using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreReviewFeedbackForEditOutput
    {
        public CreateOrEditStoreReviewFeedbackDto StoreReviewFeedback { get; set; }

        public string StoreReviewReviewInfo { get; set; }

        public string ContactFullName { get; set; }

        public string RatingLikeName { get; set; }

    }
}