using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Territory.Dtos
{
    public class GetAllHubSalesProjectionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxDurationTypeIdFilter { get; set; }
        public int? MinDurationTypeIdFilter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public double? MaxExpectedSalesAmountFilter { get; set; }
        public double? MinExpectedSalesAmountFilter { get; set; }

        public double? MaxActualSalesAmountFilter { get; set; }
        public double? MinActualSalesAmountFilter { get; set; }

        public string HubNameFilter { get; set; }

        public string ProductCategoryNameFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string CurrencyNameFilter { get; set; }

    }
}