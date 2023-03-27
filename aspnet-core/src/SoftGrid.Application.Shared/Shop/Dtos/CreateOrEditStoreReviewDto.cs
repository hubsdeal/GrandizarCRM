using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreReviewDto : EntityDto<long?>
    {

        [Required]
        public string ReviewInfo { get; set; }

        public DateTime? PostDate { get; set; }

        [StringLength(StoreReviewConsts.MaxPostTimeLength, MinimumLength = StoreReviewConsts.MinPostTimeLength)]
        public string PostTime { get; set; }

        public bool IsPublish { get; set; }

        public long StoreId { get; set; }

        public long? ContactId { get; set; }

        public long? RatingLikeId { get; set; }

    }
}