using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.OrderManagement.Dtos
{
    public class OrderStatusDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int? SequenceNo { get; set; }

        public string ColorCode { get; set; }

        public string Message { get; set; }

        public bool DeliveryOrPickup { get; set; }

        public int? RoleId { get; set; }

    }
}