using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class CreateOrEditDiscountCodeGeneratorDto : EntityDto<long?>
    {

        [StringLength(DiscountCodeGeneratorConsts.MaxNameLength, MinimumLength = DiscountCodeGeneratorConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(DiscountCodeGeneratorConsts.MaxCouponCodeLength, MinimumLength = DiscountCodeGeneratorConsts.MinCouponCodeLength)]
        public string CouponCode { get; set; }

        public bool PercentageOrFixedAmount { get; set; }

        public double? DiscountPercentage { get; set; }

        public double? DiscountAmount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string AdminNotes { get; set; }

        public bool IsActive { get; set; }

        [StringLength(DiscountCodeGeneratorConsts.MaxStartTimeLength, MinimumLength = DiscountCodeGeneratorConsts.MinStartTimeLength)]
        public string StartTime { get; set; }

        [StringLength(DiscountCodeGeneratorConsts.MaxEndTimeLength, MinimumLength = DiscountCodeGeneratorConsts.MinEndTimeLength)]
        public string EndTime { get; set; }

    }
}