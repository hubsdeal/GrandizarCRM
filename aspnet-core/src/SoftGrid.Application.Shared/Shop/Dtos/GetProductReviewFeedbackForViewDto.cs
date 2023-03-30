namespace SoftGrid.Shop.Dtos
{
    public class GetProductReviewFeedbackForViewDto
    {
        public ProductReviewFeedbackDto ProductReviewFeedback { get; set; }

        public string ContactFullName { get; set; }

        public string ProductReviewReviewInfo { get; set; }

        public string RatingLikeName { get; set; }

    }
}