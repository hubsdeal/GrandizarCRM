using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductCrossSellProductDto : EntityDto<long?>
    {

        public long CrossProductId { get; set; }

        public int? CrossSellScore { get; set; }

        public long PrimaryProductId { get; set; }

    }
}