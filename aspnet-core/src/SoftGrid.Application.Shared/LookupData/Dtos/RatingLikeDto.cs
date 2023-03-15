using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class RatingLikeDto : EntityDto<long>
    {
        public string Name { get; set; }

        public int? Score { get; set; }

        public string IconLink { get; set; }

    }
}