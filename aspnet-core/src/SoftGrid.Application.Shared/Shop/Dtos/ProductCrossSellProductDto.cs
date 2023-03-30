using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductCrossSellProductDto : EntityDto<long>
    {
        public long CrossProductId { get; set; }

        public int? CrossSellScore { get; set; }

        public long PrimaryProductId { get; set; }

    }
}