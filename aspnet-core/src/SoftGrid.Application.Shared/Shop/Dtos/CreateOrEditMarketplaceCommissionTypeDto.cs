using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditMarketplaceCommissionTypeDto : EntityDto<long?>
    {

        [StringLength(MarketplaceCommissionTypeConsts.MaxNameLength, MinimumLength = MarketplaceCommissionTypeConsts.MinNameLength)]
        public string Name { get; set; }

        public double? Percentage { get; set; }

        public double? FixedAmount { get; set; }

    }
}