using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllMarketplaceCommissionTypesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public double? MaxPercentageFilter { get; set; }
        public double? MinPercentageFilter { get; set; }

        public double? MaxFixedAmountFilter { get; set; }
        public double? MinFixedAmountFilter { get; set; }

    }
}