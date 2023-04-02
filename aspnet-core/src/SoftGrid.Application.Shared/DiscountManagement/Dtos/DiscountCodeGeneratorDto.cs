using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class DiscountCodeGeneratorDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string CouponCode { get; set; }

        public bool PercentageOrFixedAmount { get; set; }

        public double? DiscountPercentage { get; set; }

        public double? DiscountAmount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string AdminNotes { get; set; }

        public bool IsActive { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

    }
}