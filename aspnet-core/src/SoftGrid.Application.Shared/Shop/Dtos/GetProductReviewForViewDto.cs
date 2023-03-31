namespace SoftGrid.Shop.Dtos
{
    public class GetProductReviewForViewDto
    {
        public ProductReviewDto ProductReview { get; set; }

        public string ContactFullName { get; set; }

        public string ProductName { get; set; }

        public string StoreName { get; set; }

        public string RatingLikeName { get; set; }

    }
}