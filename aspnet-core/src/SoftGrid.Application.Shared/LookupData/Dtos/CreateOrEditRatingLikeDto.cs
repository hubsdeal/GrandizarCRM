using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditRatingLikeDto : EntityDto<long?>
    {

        [Required]
        [StringLength(RatingLikeConsts.MaxNameLength, MinimumLength = RatingLikeConsts.MinNameLength)]
        public string Name { get; set; }

        public int? Score { get; set; }

        [StringLength(RatingLikeConsts.MaxIconLinkLength, MinimumLength = RatingLikeConsts.MinIconLinkLength)]
        public string IconLink { get; set; }

    }
}