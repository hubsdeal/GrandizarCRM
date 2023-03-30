using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductReviewDto : EntityDto<long?>
    {

        [Required]
        public string ReviewInfo { get; set; }

        public DateTime? PostDate { get; set; }

        public bool Publish { get; set; }

        [StringLength(ProductReviewConsts.MaxPostTimeLength, MinimumLength = ProductReviewConsts.MinPostTimeLength)]
        public string PostTime { get; set; }

        public long? ContactId { get; set; }

        public long ProductId { get; set; }

        public long? StoreId { get; set; }

        public long? RatingLikeId { get; set; }

    }
}