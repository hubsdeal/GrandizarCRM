using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductReviewForEditOutput
    {
        public CreateOrEditProductReviewDto ProductReview { get; set; }

        public string ContactFullName { get; set; }

        public string ProductName { get; set; }

        public string StoreName { get; set; }

        public string RatingLikeName { get; set; }

    }
}