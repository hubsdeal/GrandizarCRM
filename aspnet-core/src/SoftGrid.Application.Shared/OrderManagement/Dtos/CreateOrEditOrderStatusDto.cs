using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class CreateOrEditOrderStatusDto : EntityDto<long?>
    {

        [Required]
        [StringLength(OrderStatusConsts.MaxNameLength, MinimumLength = OrderStatusConsts.MinNameLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? SequenceNo { get; set; }

        [StringLength(OrderStatusConsts.MaxColorCodeLength, MinimumLength = OrderStatusConsts.MinColorCodeLength)]
        public string ColorCode { get; set; }

        public string Message { get; set; }

        public bool DeliveryOrPickup { get; set; }

        public int? RoleId { get; set; }

    }
}