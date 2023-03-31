using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductReviewDto : EntityDto<long>
    {
        public string ReviewInfo { get; set; }

        public DateTime? PostDate { get; set; }

        public bool Publish { get; set; }

        public string PostTime { get; set; }

        public long? ContactId { get; set; }

        public long ProductId { get; set; }

        public long? StoreId { get; set; }

        public long? RatingLikeId { get; set; }

    }
}