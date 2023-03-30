using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Territory.Dtos
{
    public class HubSalesProjectionDto : EntityDto<long>
    {
        public int? DurationTypeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public double? ExpectedSalesAmount { get; set; }

        public double? ActualSalesAmount { get; set; }

        public long HubId { get; set; }

        public long? ProductCategoryId { get; set; }

        public long? StoreId { get; set; }

        public long? CurrencyId { get; set; }

    }
}