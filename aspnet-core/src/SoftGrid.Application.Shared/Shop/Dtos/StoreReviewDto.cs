using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreReviewDto : EntityDto<long>
    {
        public string ReviewInfo { get; set; }

        public DateTime? PostDate { get; set; }

        public string PostTime { get; set; }

        public bool IsPublish { get; set; }

        public long StoreId { get; set; }

        public long? ContactId { get; set; }

        public long? RatingLikeId { get; set; }

    }
}