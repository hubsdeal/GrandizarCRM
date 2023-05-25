using System;
using System.Collections.Generic;
using System.Text;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreReviewsByStoreBySpForView
    {
        public int TotalCount { get; set; }
        public List<ReviewByStoreFromSpDto> StoreReviews { get; set; }

        public GetStoreReviewsByStoreBySpForView()
        {
            StoreReviews = new List<ReviewByStoreFromSpDto>();
        }
    }
}
