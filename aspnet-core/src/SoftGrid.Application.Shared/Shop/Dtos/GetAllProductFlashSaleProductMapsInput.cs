using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductFlashSaleProductMapsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public double? MaxFlashSalePriceFilter { get; set; }
        public double? MinFlashSalePriceFilter { get; set; }

        public double? MaxDiscountPercentageFilter { get; set; }
        public double? MinDiscountPercentageFilter { get; set; }

        public double? MaxDiscountAmountFilter { get; set; }
        public double? MinDiscountAmountFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public string EndTimeFilter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public string StartTimeFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string MembershipTypeNameFilter { get; set; }

    }
}