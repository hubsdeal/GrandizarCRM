namespace SoftGrid.Shop.Dtos
{
    public class GetStoreReviewForViewDto
    {
        public StoreReviewDto StoreReview { get; set; }

        public string StoreName { get; set; }

        public string ContactFullName { get; set; }

        public string RatingLikeName { get; set; }

    }
}