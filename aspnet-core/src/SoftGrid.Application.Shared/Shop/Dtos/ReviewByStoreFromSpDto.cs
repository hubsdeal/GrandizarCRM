namespace SoftGrid.Shop.Dtos
{
    public class ReviewByStoreFromSpDto : StoreReviewDto
    {
        public string RatingLikeName { get; set; }
        public string ContactName { get; set; }
        public string StoreName { get; set; }
        public int NumberOfFeedbacks { get; set; }
        public decimal? RatingScore { get; set; }
    }
}