using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class MarketplaceCommissionTypeDto : EntityDto<long>
    {
        public string Name { get; set; }

        public double? Percentage { get; set; }

        public double? FixedAmount { get; set; }

    }
}