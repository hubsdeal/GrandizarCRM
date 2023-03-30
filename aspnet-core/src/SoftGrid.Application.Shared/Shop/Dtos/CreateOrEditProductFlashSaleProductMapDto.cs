using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductFlashSaleProductMapDto : EntityDto<long?>
    {

        public double? FlashSalePrice { get; set; }

        public double? DiscountPercentage { get; set; }

        public double? DiscountAmount { get; set; }

        public DateTime? EndDate { get; set; }

        public string EndTime { get; set; }

        public DateTime? StartDate { get; set; }

        public string StartTime { get; set; }

        public long? ProductId { get; set; }

        public long? StoreId { get; set; }

        public long? MembershipTypeId { get; set; }

    }
}