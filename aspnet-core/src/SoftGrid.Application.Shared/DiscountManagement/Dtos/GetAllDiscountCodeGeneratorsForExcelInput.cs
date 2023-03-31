using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetAllDiscountCodeGeneratorsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string CouponCodeFilter { get; set; }

        public int? PercentageOrFixedAmountFilter { get; set; }

        public double? MaxDiscountPercentageFilter { get; set; }
        public double? MinDiscountPercentageFilter { get; set; }

        public double? MaxDiscountAmountFilter { get; set; }
        public double? MinDiscountAmountFilter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public string AdminNotesFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public string StartTimeFilter { get; set; }

        public string EndTimeFilter { get; set; }

    }
}