using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreMarketplaceCommissionSettingDto : EntityDto<long>
    {
        public double? Percentage { get; set; }

        public double? FixedAmount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public long StoreId { get; set; }

        public long? MarketplaceCommissionTypeId { get; set; }

        public long? ProductCategoryId { get; set; }

        public long? ProductId { get; set; }

    }
}